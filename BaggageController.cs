<<<<<<< Updated upstream:Web/Api/PassengerContext/Controllers/PassengerController.cs
        [HttpDelete("{id:guid}/delete-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> DeleteConnectingFlight(Guid id, [FromBody] List<int> flightIds)
=======
        [HttpDelete("{id}/flight/{flightId}/delete-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> DeleteConnectingFlight(Guid id, int flightId, 
            [FromBody] List<int> flightIds)
>>>>>>> Stashed changes:API/Api/PassengerContext/Controllers/PassengerController.cs
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

            foreach (var iteratedFlightId in flightIds)
            {
<<<<<<< Updated upstream:Web/Api/PassengerContext/Controllers/PassengerController.cs
                var flight = passenger.Flights.FirstOrDefault(pf => pf.Flight.Id == flightId);
=======
                var flight = passenger.Flights.FirstOrDefault(f => f.Flight.Id == iteratedFlightId);
>>>>>>> Stashed changes:API/Api/PassengerContext/Controllers/PassengerController.cs

                if (flight != null)
                {
                    passenger.Flights.Remove(flight);
                }
                else if (iteratedFlightId == flightId)
                {
                    return BadRequest(new ApiResponse(400, "Current flight cannot be removed."));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Invalid flight IDs."));
                }
            }

            await _passengerRepository.UpdateAsync(passenger);

            return NoContent();
        }

        [HttpPost("{id}/add-special-service-request")]
        public async Task<ActionResult<Passenger>> AddSpecialServiceRequest(Guid id,
            [FromBody] JObject data)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var SSRCode = _sSRCodeRepository.GetSSRCodeAsync(data["SSRCode"]?.ToString());
            var flightId = data["flightId"]?.ToString();
            var freeText = data["freeText"]?.ToString();
            var passengerId = passenger.Id;

            if (SSRCode == null || string.IsNullOrEmpty(flightId))
            {
                return BadRequest(new ApiResponse(400, "All fields must be filled in for the special service request."));
            }

            var specialServiceRequest = new SpecialServiceRequest
            {
                SSRCode = SSRCode,
                FlightId = int.Parse(flightId),
                FreeText = freeText,
                PassengerId = passengerId
            };

            specialServiceRequest = new SpecialServiceRequestDto
            {
                SSRCode = SSRCode,
                FlightId = int.Parse(flightId),
                FreeText = freeText,
                PassengerId = passengerId
            };

            return Ok();
        }
