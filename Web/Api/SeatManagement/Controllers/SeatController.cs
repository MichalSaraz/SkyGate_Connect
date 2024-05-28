using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using Web.Errors;

namespace Web.Api.SeatManagement.Controllers
{
    [ApiController]
    [Route("seat-management")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ICommentService _commentService;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;

        public SeatController(
            ISeatRepository seatRepository,
            ICommentService commentService,
            IBasePassengerOrItemRepository basePassengerOrItemRepository)
        {
            _seatRepository = seatRepository;
            _commentService = commentService;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
        }

        [HttpPatch("flight/{flightId:guid}/update-seat-status")]
        public async Task<IActionResult> UpdateSeatStatus(Guid flightId, List<string> selectedSeats, bool blockSeats)
        {
            var targetStatus = blockSeats ? SeatStatusEnum.Empty : SeatStatusEnum.Blocked;
            var newStatus = blockSeats ? SeatStatusEnum.Blocked : SeatStatusEnum.Empty;

            var seats = await _seatRepository.GetSeatsByCriteriaAsync(c => selectedSeats.Contains(c.SeatNumber)
                && c.SeatStatus == targetStatus && c.FlightId == flightId);

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

        //[HttpPatch("flight/{flightId:guid}/change-seats")]
        //public async Task<IActionResult> ChangeSeats(Guid flightId, Dictionary<Guid, string> newSeatNumbers, bool swapSeats)
        //{
        //    var seatIds = newSeatNumbers.Keys.ToList();
        //    var seatNumbers = newSeatNumbers.Values.ToList();
        //    var flightIds = new List<Guid> { flightId };
        //    var passengersToSwap = new Dictionary<Guid, Guid>();

        //    var seats = await _seatRepository.GetSeatsByCriteriaAsync(c =>
        //        seatIds.Contains(c.Id) || (seatNumbers.Contains(c.SeatNumber) && c.FlightId == flightId));

        //    if (seats == null || seats.Count < newSeatNumbers.Count * 2)
        //    {
        //        return NotFound(new ApiResponse(404, "Seats not found"));
        //    }

        //    var newSeats = seats.Where(s => seatNumbers.Contains(s.SeatNumber)).ToList();
        //    var currentSeats = seats.Where(s => seatIds.Contains(s.Id)).ToList();

        //    foreach (var newSeat in newSeats)
        //    {
        //        if (!swapSeats && newSeat.SeatStatus != SeatStatusEnum.Empty)
        //        {
        //            return BadRequest(new ApiResponse(400, "Seat is not available"));
        //        }

        //        if (swapSeats && newSeat.SeatStatus == SeatStatusEnum.Empty)
        //        {
        //            return BadRequest(new ApiResponse(400, "Seat is not occupied"));
        //        }

        //        if (swapSeats)
        //        {
        //            passengersToSwap.Add(newSeat.Id, newSeat.PassengerOrItemId.Value);
        //        }

        //        var correspondingCurrentSeat = currentSeats.FirstOrDefault(c => c.Id == newSeatNumbers.FirstOrDefault(s => s.Value == newSeat.SeatNumber).Key);
        //        newSeat.SeatStatus = SeatStatusEnum.Occupied;
        //        newSeat.PassengerOrItemId = correspondingCurrentSeat.PassengerOrItemId;

        //        var commentResult = await _AddSeatChangeRelatedComment(newSeat, flightIds);

        //        if (commentResult != null)
        //        {
        //            return commentResult;
        //        }
        //    }

        //    foreach (var currentSeat in currentSeats)
        //    {
        //        if (currentSeat.SeatNumber == newSeatNumbers[currentSeat.Id])
        //        {
        //            return BadRequest(new ApiResponse(400, "Seat number is the same"));
        //        }

        //        if (swapSeats)
        //        {
        //            currentSeat.PassengerOrItemId = passengersToSwap[currentSeat.Id];
        //            var commentResult = await _AddSeatChangeRelatedComment(currentSeat, flightIds);

        //            if (commentResult != null)
        //            {
        //                return commentResult;
        //            }
        //        }
        //        else
        //        {
        //            currentSeat.SeatStatus = SeatStatusEnum.Empty;
        //            currentSeat.PassengerOrItemId = null;
        //        }
        //    }

        //    var seatsToUpdate = currentSeats.Concat(newSeats).ToList();
        //    await _seatRepository.UpdateAsync(seatsToUpdate.ToArray());

        //    return Ok();
        //}


        [HttpPatch("flight/{flightId:guid}/change-seats")]
        public async Task<IActionResult> ChangeSeats(Guid flightId, Dictionary<Guid, string> newSeatNumbers, bool swapSeats)
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
                passengerIds.Contains(c.PassengerOrItemId.Value) && c.FlightId == flightId);

            if (currentSeats == null || currentSeats.Count < newSeatNumbers.Count)
            {
                return NotFound(new ApiResponse(404, "Current seats not found"));
            }

            var newSeatsDict = seats.ToDictionary(s => s.SeatNumber);
            var currentSeatsDict = currentSeats.ToDictionary(c => c.PassengerOrItemId.Value);
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
                    passengersToSwap.Add(newSeat.Id, newSeat.PassengerOrItemId.Value);
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


        [HttpPatch("flight/{flightId:guid}/allocate-seats")]
        public async Task<IActionResult> AllocateSeats(Guid flightId, Dictionary<Guid, string> seatsToAllocate)
        {
            Expression<Func<BasePassengerOrItem, bool>> passengerCriteria = c => seatsToAllocate.Keys.Contains(c.Id) && 
                c.Flights.FirstOrDefault(f => f.FlightId == flightId).AcceptanceStatus == AcceptanceStatusEnum.NotAccepted;

            var passengers =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemByCriteriaAsync(passengerCriteria);
            var seats = 
                await _seatRepository.GetSeatsByCriteriaAsync(c => seatsToAllocate.Values.Contains(c.SeatNumber) && c.FlightId == flightId);

            if (seats == null || seats.Count != seatsToAllocate.Count || 
                passengers == null || passengers.Count != seatsToAllocate.Count)
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

        [HttpPatch("flight/{flightId:guid}/deallocate-seats")]
        public async Task<IActionResult> DeallocateSeats(Guid flightId, List<Guid> passengerIds)
        {
            Expression<Func<Seat, bool>> seatCriteria = c => passengerIds.Contains(c.PassengerOrItemId.Value) &&
                c.PassengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flightId).AcceptanceStatus == AcceptanceStatusEnum.NotAccepted;
            
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
                    await _commentService.AddCommentAsync(newSeat.PassengerOrItemId.Value, CommentTypeEnum.Gate, null, flightIds, "Exit");
                }
                catch (Exception e)
                {
                    return BadRequest(new ApiResponse(400, e.Message));
                }
            }

            try
            {
                await _commentService.AddCommentAsync(newSeat.PassengerOrItemId.Value, CommentTypeEnum.Gate, null, flightIds, "SeatChng");
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }
            return null;
        }
    }
}
