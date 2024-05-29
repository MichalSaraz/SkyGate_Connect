#nullable enable
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
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

        public SeatController(ISeatRepository seatRepository, ICommentService commentService,
            IBasePassengerOrItemRepository basePassengerOrItemRepository)
        {
            _seatRepository = seatRepository;
            _commentService = commentService;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
        }

        /// <summary>
        /// Updates the seat status for selected seats on a specific flight.
        /// </summary>
        /// <param name="flightId">The unique identifier of the flight.</param>
        /// <param name="selectedSeats">A list of seat numbers that need to be updated.</param>
        /// <param name="blockSeats">A boolean value indicating whether to block the seats (<c>true</c>) or unblock
        /// them (<c>false</c>).</param>
        /// <returns>An <see cref="IActionResult"/> representing the response of the update operation.</returns>
        [HttpPatch("update-seat-status")]
        public async Task<IActionResult> UpdateSeatStatus(Guid flightId, List<string> selectedSeats, bool blockSeats)
        {
            var targetStatus = blockSeats ? SeatStatusEnum.Empty : SeatStatusEnum.Blocked;
            var newStatus = blockSeats ? SeatStatusEnum.Blocked : SeatStatusEnum.Empty;

            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                selectedSeats.Contains(c.SeatNumber) && c.SeatStatus == targetStatus && c.FlightId == flightId);

            if (seats == null || seats.Count < selectedSeats.Count)
            {
                return NotFound(new ApiResponse(404, "Seats not found"));
            }

            foreach (var seat in seats)
            {
                seat.SeatStatus = newStatus;
            }

            await _seatRepository.UpdateAsync(seats.ToArray());

            return Ok();
        }

        /// <summary>
        /// Changes the seats for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="newSeatNumbers">A dictionary containing the passenger or item IDs as keys and the new seat
        /// numbers as values.</param>
        /// <param name="swapSeats">A boolean value indicating whether to swap seats or not.</param>
        /// <returns>An IActionResult indicating the success or failure of the seat changes.</returns>
        [HttpPatch("change-seats")]
        public async Task<IActionResult> ChangeSeats(Guid flightId, Dictionary<Guid, string> newSeatNumbers,
            bool swapSeats)
        {
            var passengerIds = newSeatNumbers.Keys.ToList();
            var seatNumbers = newSeatNumbers.Values.ToList();
            var flightIds = new List<Guid> { flightId };

            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                (seatNumbers.Contains(c.SeatNumber) && c.FlightId == flightId));

            if (seats == null || seats.Count < newSeatNumbers.Count)
            {
                return NotFound(new ApiResponse(404, "Seats not found"));
            }

            var currentSeats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                c.PassengerOrItemId != null && passengerIds.Contains(c.PassengerOrItemId.Value) &&
                c.FlightId == flightId);

            if (currentSeats == null || currentSeats.Count < newSeatNumbers.Count)
            {
                return NotFound(new ApiResponse(404, "Current seats not found"));
            }

            var newSeatsDict = seats.ToDictionary(s => s.SeatNumber);
            var currentSeatsDict = currentSeats.ToDictionary(c => c.PassengerOrItemId ?? Guid.Empty);
            var passengersToSwap = new Dictionary<Guid, Guid>();
            var currentToNewSeatMapping = new Dictionary<Guid, Guid>();

            foreach (var allocation in newSeatNumbers)
            {
                var newSeat = newSeatsDict[allocation.Value];
                var currentSeat = currentSeatsDict[allocation.Key];

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

                var commentResult = await _AddSeatChangeRelatedComment(newSeat, flightIds);
                if (commentResult != null)
                {
                    return commentResult;
                }
            }

            foreach (var allocation in newSeatNumbers)
            {
                var currentSeat = currentSeatsDict[allocation.Key];

                if (currentSeat.SeatNumber == allocation.Value)
                {
                    return BadRequest(new ApiResponse(400, "Seat number is the same"));
                }

                if (swapSeats)
                {
                    currentSeat.PassengerOrItemId = passengersToSwap[currentToNewSeatMapping[currentSeat.Id]];
                    var commentResult = await _AddSeatChangeRelatedComment(currentSeat, flightIds);
                    if (commentResult != null)
                    {
                        return commentResult;
                    }
                }
                else
                {
                    currentSeat.SeatStatus = SeatStatusEnum.Empty;
                    currentSeat.PassengerOrItemId = null;
                }
            }

            var seatsToUpdate = currentSeats.Concat(seats).ToList();
            await _seatRepository.UpdateAsync(seatsToUpdate.ToArray());

            return Ok();
        }

        /// <summary>
        /// Allocates seats to passengers for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="seatsToAllocate">A dictionary of passenger IDs and corresponding seat numbers to allocate.
        /// </param>
        /// <returns>An IActionResult representing the result of the allocation.</returns>
        [HttpPatch("allocate-seats")]
        public async Task<IActionResult> AllocateSeats(Guid flightId, Dictionary<Guid, string> seatsToAllocate)
        {
            Expression<Func<BasePassengerOrItem, bool>> passengerCriteria = c => seatsToAllocate.Keys.Contains(c.Id) &&
                c.Flights.Any(f => f.FlightId == flightId && f.AcceptanceStatus == AcceptanceStatusEnum.NotAccepted);

            var passengers =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemByCriteriaAsync(passengerCriteria);
            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
                seatsToAllocate.Values.Contains(c.SeatNumber) && c.FlightId == flightId);

            if (seats == null || seats.Count != seatsToAllocate.Count || passengers == null ||
                passengers.Count != seatsToAllocate.Count)
            {
                return NotFound(new ApiResponse(404, "Seats or passengers not found"));
            }

            var seatsDict = seats.ToDictionary(s => s.SeatNumber);
            var passengersDict = passengers.ToDictionary(p => p.Id);

            foreach (var allocation in seatsToAllocate)
            {
                var seat = seatsDict[allocation.Value];
                var passenger = passengersDict[allocation.Key];

                if (seat.SeatStatus != SeatStatusEnum.Empty)
                {
                    return BadRequest(new ApiResponse(400, "Seat is not available"));
                }

                seat.SeatStatus = SeatStatusEnum.Occupied;
                seat.PassengerOrItemId = passenger.Id;
            }

            await _seatRepository.UpdateAsync(seats.ToArray());

            return Ok();
        }

        /// <summary>
        /// Deallocates seats for the specified flight and passenger IDs.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="passengerIds">The IDs of the passengers.</param>
        /// <returns>Returns an IActionResult representing the result of the operation.</returns>
        [HttpPatch("deallocate-seats")]
        public async Task<IActionResult> DeallocateSeats(Guid flightId, List<Guid> passengerIds)
        {
            Expression<Func<Seat, bool>> seatCriteria = c => passengerIds.Contains(c.PassengerOrItemId ?? Guid.Empty) &&
                                                             c.PassengerOrItem.Flights.Any(f =>
                                                                 f.FlightId == flightId && f.AcceptanceStatus ==
                                                                 AcceptanceStatusEnum.NotAccepted);

            var seatsToDeallocate = await _seatRepository.GetSeatsByCriteriaAsync(seatCriteria);

            if (seatsToDeallocate == null || seatsToDeallocate.Count != passengerIds.Count)
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
            }

            await _seatRepository.UpdateAsync(seatsToDeallocate.ToArray());

            return Ok();
        }

        private async Task<IActionResult?> _AddSeatChangeRelatedComment(Seat newSeat, List<Guid> flightIds)
        {
            if (newSeat.SeatType == SeatTypeEnum.EmergencyExit)
            {
                try
                {
                    await _commentService.AddCommentAsync(newSeat.PassengerOrItemId ?? Guid.Empty, CommentTypeEnum.Gate,
                        null, flightIds, "Exit");
                }
                catch (Exception e)
                {
                    return BadRequest(new ApiResponse(400, e.Message));
                }
            }

            try
            {
                await _commentService.AddCommentAsync(newSeat.PassengerOrItemId ?? Guid.Empty, CommentTypeEnum.Gate,
                    null, flightIds, "SeatChng");
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }

            return null;
        }
    }
}