using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.PassengerContext.Booking;
using Web.Errors;

namespace Web.Api.SeatManagement.Controllers
{
    [ApiController]
    [Route("seat-management/flight/{flightId:guid}")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ICommentService _commentService;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public SeatController(
            ISeatRepository seatRepository,
            ICommentService commentService,
            IBasePassengerOrItemRepository basePassengerOrItemRepository,
            IFlightRepository flightRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _commentService = commentService;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _flightRepository = flightRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Updates the seat status for selected seats on a specific flight.
        /// </summary>
        /// <param name="flightId">The unique identifier of the flight.</param>
        /// <param name="selectedSeats">A list of seat numbers that need to be updated.</param>
        /// <param name="blockSeats">A boolean value indicating whether to block the seats (<c>true</c>) or unblock
        /// them (<c>false</c>).</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="SeatDto"/>
        ///  objects representing the updated seats.</returns>
        [HttpPatch("update-seat-status")]
        public async Task<ActionResult<List<SeatDto>>> UpdateSeatStatus(Guid flightId, List<string> selectedSeats,
            bool blockSeats)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with id {flightId} not found"));
            }

            var targetStatus = blockSeats ? SeatStatusEnum.Empty : SeatStatusEnum.Blocked;
            var newStatus = blockSeats ? SeatStatusEnum.Blocked : SeatStatusEnum.Empty;

            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                selectedSeats.Contains(c.SeatNumber) && c.SeatStatus == targetStatus && c.FlightId == flightId);

            if (seats.Count < selectedSeats.Count)
            {
                return NotFound(new ApiResponse(404, "Seats not found"));
            }

            foreach (var seat in seats)
            {
                seat.SeatStatus = newStatus;
            }

            await _seatRepository.UpdateAsync(seats.ToArray());
            
            var seatsToUpdateDto = _mapper.Map<List<SeatDto>>(seats);

            return Ok(seatsToUpdateDto);
        }

        /// <summary>
        /// Changes the seats for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="newSeatNumbers">A dictionary containing the passenger or item IDs as keys and the new seat
        /// numbers as values.</param>
        /// <param name="swapSeats">A boolean value indicating whether to swap seats or not.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="SeatDto"/>
        ///  objects representing the changed seats.</returns>
        [HttpPatch("change-seats")]
        public async Task<ActionResult<List<SeatDto>>> ChangeSeats(Guid flightId,
            Dictionary<Guid, string> newSeatNumbers, bool swapSeats)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with id {flightId} not found"));
            }

            var passengerIds = newSeatNumbers.Keys.ToList();
            var seatNumbers = newSeatNumbers.Values.ToList();
            var flightIds = new List<Guid> { flightId };

            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                (seatNumbers.Contains(c.SeatNumber) && c.FlightId == flightId));

            if (seats.Count < newSeatNumbers.Count)
            {
                return NotFound(new ApiResponse(404, "Seats not found"));
            }

            var currentSeats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                c.PassengerOrItemId != null && passengerIds.Contains(c.PassengerOrItemId.Value) &&
                c.FlightId == flightId);

            if (currentSeats.Count < newSeatNumbers.Count)
            {
                return NotFound(new ApiResponse(404, "Current seats not found"));
            }

            var newSeatsDictionary = seats.ToDictionary(s => s.SeatNumber);
            var currentSeatsDictionary = currentSeats.ToDictionary(c => c.PassengerOrItemId ?? Guid.Empty);
            var passengersToSwap = new Dictionary<Guid, Guid>();
            var currentToNewSeatMapping = new Dictionary<Guid, Guid>();

            foreach (var allocation in newSeatNumbers)
            {
                var newSeat = newSeatsDictionary[allocation.Value];
                var currentSeat = currentSeatsDictionary[allocation.Key];

                if (!swapSeats && newSeat.SeatStatus != SeatStatusEnum.Empty)
                {
                    return BadRequest(new ApiResponse(400, "Seat is not available"));
                }

                if (swapSeats && newSeat.SeatStatus == SeatStatusEnum.Empty)
                {
                    return BadRequest(new ApiResponse(400, "Seat is not occupied"));
                }

                if (swapSeats)
                {
                    passengersToSwap.Add(newSeat.Id, newSeat.PassengerOrItemId ?? Guid.Empty);
                }

                newSeat.SeatStatus = SeatStatusEnum.Occupied;
                newSeat.PassengerOrItemId = allocation.Key;
                currentToNewSeatMapping[currentSeat.Id] = newSeat.Id;

                await _AddSeatChangeRelatedComment(newSeat, flightIds);

                _RemoveExitCommentsIfNotEmergencyExit(newSeat);
            }

            foreach (var allocation in newSeatNumbers)
            {
                var currentSeat = currentSeatsDictionary[allocation.Key];

                if (currentSeat.SeatNumber == allocation.Value)
                {
                    return BadRequest(new ApiResponse(400, "Seat number is the same"));
                }

                if (swapSeats)
                {
                    currentSeat.PassengerOrItemId = passengersToSwap[currentToNewSeatMapping[currentSeat.Id]];

                    await _AddSeatChangeRelatedComment(currentSeat, flightIds);

                    _RemoveExitCommentsIfNotEmergencyExit(currentSeat);
                }
                else
                {
                    currentSeat.SeatStatus = SeatStatusEnum.Empty;
                    currentSeat.PassengerOrItemId = null;
                }
            }

            var seatsToUpdate = currentSeats.Concat(seats).ToList();
            await _seatRepository.UpdateAsync(seatsToUpdate.ToArray());
            
            var seatsToUpdateDto = _mapper.Map<List<SeatDto>>(seatsToUpdate);

            return Ok(seatsToUpdateDto);
        }

        /// <summary>
        /// Allocates seats to passengers for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="seatsToAllocate">A dictionary of passenger IDs and corresponding seat numbers to allocate.
        /// </param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="SeatDto"/>
        ///  objects representing the allocated seats.</returns>
        [HttpPatch("allocate-seats")]
        public async Task<ActionResult<List<SeatDto>>> AllocateSeats(Guid flightId,
            Dictionary<Guid, string> seatsToAllocate)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with id {flightId} not found"));
            }

            Expression<Func<BasePassengerOrItem, bool>> passengerCriteria = c => seatsToAllocate.Keys.Contains(c.Id) &&
                c.Flights.Any(f => f.FlightId == flightId && f.AcceptanceStatus == AcceptanceStatusEnum.NotAccepted);

            var passengerOrItems =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(passengerCriteria);
            var seatsToAllocateList = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                seatsToAllocate.Values.Contains(c.SeatNumber) && c.FlightId == flightId);
            var flightIds = new List<Guid> { flightId };

            if (seatsToAllocateList.Count != seatsToAllocate.Count || passengerOrItems.Count != seatsToAllocate.Count)
            {
                return NotFound(new ApiResponse(404, "Seats or passengers not found"));
            }

            var seatsDictionary = seatsToAllocateList.ToDictionary(s => s.SeatNumber);
            var passengersDictionary = passengerOrItems.ToDictionary(p => p.Id);

            foreach (var allocation in seatsToAllocate)
            {
                var seatToAllocate = seatsDictionary[allocation.Value];
                var passenger = passengersDictionary[allocation.Key];

                if (seatToAllocate.SeatStatus != SeatStatusEnum.Empty)
                {
                    return BadRequest(new ApiResponse(400, "Seat is not available"));
                }

                seatToAllocate.SeatStatus = SeatStatusEnum.Occupied;
                seatToAllocate.PassengerOrItemId = passenger.Id;

                if (seatToAllocate.SeatType == SeatTypeEnum.EmergencyExit)
                {
                    var comment = await _AddEmergencyExitSuitabilityCheckComment(seatToAllocate, flightIds);
                }
            }

            await _seatRepository.UpdateAsync(seatsToAllocateList.ToArray());

            var seatsToAllocateDto = _mapper.Map<List<SeatDto>>(seatsToAllocateList);

            return Ok(seatsToAllocateDto);
        }

        /// <summary>
        /// Deallocates seats for the specified flight and passenger IDs.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="passengerIds">The IDs of the passengers.</param>
        /// <returns> An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="SeatDto"/>
        /// objects representing the deallocated seats.</returns>
        [HttpPatch("deallocate-seats")]
        public async Task<ActionResult<List<SeatDto>>> DeallocateSeats(Guid flightId, List<Guid> passengerIds)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with id {flightId} not found"));
            }

            Expression<Func<Seat, bool>> seatCriteria = c => passengerIds.Contains(c.PassengerOrItemId ?? Guid.Empty) &&
                                                             c.PassengerOrItem.Flights.Any(f =>
                                                                 f.FlightId == flightId && f.AcceptanceStatus ==
                                                                 AcceptanceStatusEnum.NotAccepted);

            var seatsToDeallocate = await _seatRepository.GetSeatsByCriteriaAsync(seatCriteria);

            if (seatsToDeallocate.Count != passengerIds.Count)
            {
                return NotFound(new ApiResponse(404, "Seats not found"));
            }

            foreach (var seat in seatsToDeallocate)
            {
                if (seat.SeatStatus == SeatStatusEnum.Empty)
                {
                    return BadRequest(new ApiResponse(400, "Seat is not occupied"));
                }

                seat.SeatStatus = SeatStatusEnum.Empty;
                seat.PassengerOrItemId = null;

                _RemoveExitCommentsIfNotEmergencyExit(seat);
            }

            await _seatRepository.UpdateAsync(seatsToDeallocate.ToArray());
            
            var seatsToDeallocateDto = _mapper.Map<List<SeatDto>>(seatsToDeallocate);

            return Ok(seatsToDeallocateDto);
        }

        /// <summary>
        /// Removes the exit comments from a seat if it is not an emergency exit seat.
        /// </summary>
        /// <param name="seat">The seat to remove exit comments from.</param>
        private static void _RemoveExitCommentsIfNotEmergencyExit(Seat seat)
        {
            if (seat.SeatType != SeatTypeEnum.EmergencyExit &&
                seat.PassengerOrItem.Comments.Any(c => c.PredefinedCommentId == "Exit"))
            {
                seat.PassengerOrItem.Comments.RemoveAll(c => c.PredefinedCommentId == "Exit");
            }
        }

        /// <summary>
        /// Adds a seat change related comment to a seat.
        /// </summary>
        /// <param name="newSeat">The new seat to add the comment to.</param>
        /// <param name="flightIds">The list of flight IDs the seat is associated with.</param>
        /// <returns>
        /// Returns an <see cref="OkObjectResult"/> if the comment is added successfully.
        /// Returns a <see cref="BadRequestObjectResult"/> if an exception occurs.
        /// </returns>
        private async Task _AddSeatChangeRelatedComment(Seat newSeat, List<Guid> flightIds)
        {
            if (newSeat.SeatType == SeatTypeEnum.EmergencyExit)
            {
                await _AddEmergencyExitSuitabilityCheckComment(newSeat, flightIds);
            }

            try
            {
                var seatChangeComment =
                    await _commentRepository.GetCommentByCriteriaAsync(c =>
                        c.PassengerId == newSeat.PassengerOrItemId && c.PredefinedCommentId == "SeatChng") ??
                    await _commentService.AddCommentAsync(newSeat.PassengerOrItemId ?? Guid.Empty, CommentTypeEnum.Gate,
                        null, flightIds, "SeatChng");

                Ok(seatChangeComment);
            }
            catch (Exception e)
            {
                BadRequest(new ApiResponse(400, e.Message));
            }
        }

        /// <summary>
        /// Adds a comment for emergency exit suitability check to a seat.
        /// </summary>
        /// <param name="newSeat">The seat to add the comment to.</param>
        /// <param name="flightIds">The list of flight IDs to associate the comment with.</param>
        /// <returns>
        /// Returns an <see cref="OkObjectResult"/> if the comment is added successfully.
        /// Returns a <see cref="BadRequestObjectResult"/> if an exception occurs.
        /// </returns>
        private async Task<ActionResult<Comment>> _AddEmergencyExitSuitabilityCheckComment(Seat newSeat,
            List<Guid> flightIds)
        {
            try
            {
                var suitabilityCheckComment =
                    await _commentRepository.GetCommentByCriteriaAsync(c =>
                        c.PassengerId == newSeat.PassengerOrItemId && c.PredefinedCommentId == "Exit") ??
                    await _commentService.AddCommentAsync(newSeat.PassengerOrItemId ?? Guid.Empty, CommentTypeEnum.Gate,
                        null, flightIds, "Exit");

                return Ok(suitabilityCheckComment);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }
        }
    }
}