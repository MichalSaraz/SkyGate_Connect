using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "BaggageTagsSequence",
                maxValue: 999999L,
                cyclic: true);

            migrationBuilder.CreateTable(
                name: "AircraftTypes",
                columns: table => new
                {
                    AircraftTypeIATACode = table.Column<string>(type: "text", nullable: false),
                    ModelName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftTypes", x => x.AircraftTypeIATACode);
                });

            migrationBuilder.CreateTable(
                name: "BookingReferences",
                columns: table => new
                {
                    PNR = table.Column<string>(type: "text", nullable: false),
                    FlightItinerary = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReferences", x => x.PNR);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Country2LetterCode = table.Column<string>(type: "text", nullable: false),
                    Country3LetterCode = table.Column<string>(type: "text", nullable: true),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    AircraftRegistrationPrefix = table.Column<string[]>(type: "text[]", nullable: true),
                    IsEEACountry = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Country2LetterCode);
                });

            migrationBuilder.CreateTable(
                name: "PredefinedComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatMaps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FlightClassesSpecification = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledFlights",
                columns: table => new
                {
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    Codeshare = table.Column<string[]>(type: "text[]", nullable: true),
                    DepartureTimes = table.Column<string>(type: "jsonb", nullable: true),
                    ArrivalTimes = table.Column<string>(type: "jsonb", nullable: true),
                    FlightDuration = table.Column<string>(type: "jsonb", nullable: true),
                    DestinationFrom = table.Column<string>(type: "text", nullable: true),
                    DestinationTo = table.Column<string>(type: "text", nullable: true),
                    Airline = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlights", x => x.FlightNumber);
                });

            migrationBuilder.CreateTable(
                name: "SSRCodes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsFreeTextMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSRCodes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "PassengerBookingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    PNRId = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    BaggageAllowance = table.Column<int>(type: "integer", nullable: false),
                    PriorityBoarding = table.Column<bool>(type: "boolean", nullable: false),
                    FrequentFlyerCardNumber = table.Column<string>(type: "text", nullable: true),
                    PassengerOrItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssociatedPassengerBookingDetailsId = table.Column<Guid>(type: "uuid", nullable: true),
                    BookedClass = table.Column<string>(type: "jsonb", nullable: true),
                    ReservedSeats = table.Column<string>(type: "jsonb", nullable: true),
                    BookedSSR = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerBookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassengerBookingDetails_BookingReferences_PNRId",
                        column: x => x.PNRId,
                        principalTable: "BookingReferences",
                        principalColumn: "PNR",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedP~",
                        column: x => x.AssociatedPassengerBookingDetailsId,
                        principalTable: "PassengerBookingDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    CarrierCode = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    AccountingCode = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AirlinePrefix = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.CarrierCode);
                    table.ForeignKey(
                        name: "FK_Airlines_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Country2LetterCode");
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    IATAAirportCode = table.Column<string>(type: "text", nullable: false),
                    AirportName = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.IATAAirportCode);
                    table.ForeignKey(
                        name: "FK_Destinations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Country2LetterCode");
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    BookingDetailsId = table.Column<Guid>(type: "uuid", nullable: true),
                    PassengerOrItemType = table.Column<string>(type: "text", nullable: false),
                    AssociatedAdultPassengerId = table.Column<Guid>(type: "uuid", nullable: true),
                    FrequentFlyerCardId = table.Column<Guid>(type: "uuid", nullable: true),
                    InfantId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaggageAllowance = table.Column<int>(type: "integer", nullable: true),
                    PriorityBoarding = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                        column: x => x.BookingDetailsId,
                        principalTable: "PassengerBookingDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                        column: x => x.AssociatedAdultPassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    RegistrationCode = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    AircraftTypeId = table.Column<string>(type: "text", nullable: true),
                    AirlineId = table.Column<string>(type: "text", nullable: true),
                    SeatMapId = table.Column<string>(type: "text", nullable: true),
                    JumpSeatsAvailable = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.RegistrationCode);
                    table.ForeignKey(
                        name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                        column: x => x.AircraftTypeId,
                        principalTable: "AircraftTypes",
                        principalColumn: "AircraftTypeIATACode");
                    table.ForeignKey(
                        name: "FK_Aircrafts_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "CarrierCode");
                    table.ForeignKey(
                        name: "FK_Aircrafts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Country2LetterCode");
                    table.ForeignKey(
                        name: "FK_Aircrafts_SeatMaps_SeatMapId",
                        column: x => x.SeatMapId,
                        principalTable: "SeatMaps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "APISData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: true),
                    NationalityId = table.Column<string>(type: "text", nullable: true),
                    CountryOfIssueId = table.Column<string>(type: "text", nullable: true),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<string>(type: "text", nullable: false),
                    DateOfIssue = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APISData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_APISData_Countries_CountryOfIssueId",
                        column: x => x.CountryOfIssueId,
                        principalTable: "Countries",
                        principalColumn: "Country2LetterCode");
                    table.ForeignKey(
                        name: "FK_APISData_Countries_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Countries",
                        principalColumn: "Country2LetterCode");
                    table.ForeignKey(
                        name: "FK_APISData_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Baggage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagNumber = table.Column<string>(type: "text", nullable: true),
                    TagType = table.Column<string>(type: "text", nullable: true),
                    SpecialBagType = table.Column<string>(type: "text", nullable: true),
                    SpecialBagDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DestinationId = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baggage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baggage_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_Baggage_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PredefinedCommentId = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CommentType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_PredefinedComments_PredefinedCommentId",
                        column: x => x.PredefinedCommentId,
                        principalTable: "PredefinedComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FrequentFlyerCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AirlineId = table.Column<string>(type: "text", nullable: true),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    CardholderFirstName = table.Column<string>(type: "text", nullable: false),
                    CardholderLastName = table.Column<string>(type: "text", nullable: false),
                    TierLever = table.Column<string>(type: "text", nullable: false),
                    MilesAvailable = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequentFlyerCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrequentFlyerCards_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "CarrierCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrequentFlyerCards_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationFromId = table.Column<string>(type: "text", nullable: true),
                    DestinationToId = table.Column<string>(type: "text", nullable: true),
                    AirlineId = table.Column<string>(type: "text", nullable: true),
                    DepartureDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ArrivalDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FlightType = table.Column<string>(type: "text", nullable: false),
                    ScheduledFlightId = table.Column<string>(type: "text", nullable: true),
                    AircraftId = table.Column<string>(type: "text", nullable: true),
                    DividerPlacedBehindRow = table.Column<int>(type: "integer", nullable: true),
                    BoardingStatus = table.Column<string>(type: "text", nullable: true),
                    FlightStatus = table.Column<string>(type: "text", nullable: true),
                    OtherFlightFltNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "RegistrationCode");
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "CarrierCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_DestinationFromId",
                        column: x => x.DestinationFromId,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_DestinationToId",
                        column: x => x.DestinationToId,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_Flights_ScheduledFlights_ScheduledFlightId",
                        column: x => x.ScheduledFlightId,
                        principalTable: "ScheduledFlights",
                        principalColumn: "FlightNumber");
                });

            migrationBuilder.CreateTable(
                name: "FlightBaggage",
                columns: table => new
                {
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaggageId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaggageType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightBaggage", x => new { x.FlightId, x.BaggageId });
                    table.ForeignKey(
                        name: "FK_FlightBaggage_Baggage_BaggageId",
                        column: x => x.BaggageId,
                        principalTable: "Baggage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightBaggage_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightComment",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightComment", x => new { x.CommentId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_FlightComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightComment_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerFlight",
                columns: table => new
                {
                    PassengerOrItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardingSequenceNumber = table.Column<int>(type: "integer", nullable: true),
                    BoardingZone = table.Column<string>(type: "text", nullable: true),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    AcceptanceStatus = table.Column<string>(type: "text", nullable: false),
                    NotTravellingReason = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerFlight", x => new { x.PassengerOrItemId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_PassengerFlight_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerFlight_Passengers_PassengerOrItemId",
                        column: x => x.PassengerOrItemId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerOrItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatNumber = table.Column<string>(type: "text", nullable: true),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    Letter = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    SeatType = table.Column<string>(type: "text", nullable: false),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    SeatStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seats_Passengers_PassengerOrItemId",
                        column: x => x.PassengerOrItemId,
                        principalTable: "Passengers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecialServiceRequests",
                columns: table => new
                {
                    SSRCodeId = table.Column<string>(type: "text", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    FreeText = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialServiceRequests", x => new { x.PassengerId, x.FlightId, x.SSRCodeId });
                    table.ForeignKey(
                        name: "FK_SpecialServiceRequests_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialServiceRequests_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                        column: x => x.SSRCodeId,
                        principalTable: "SSRCodes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AircraftTypeId",
                table: "Aircrafts",
                column: "AircraftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_CountryId",
                table: "Aircrafts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_CountryId",
                table: "Airlines",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_APISData_CountryOfIssueId",
                table: "APISData",
                column: "CountryOfIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_APISData_NationalityId",
                table: "APISData",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_APISData_PassengerId",
                table: "APISData",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_DestinationId",
                table: "Baggage",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_PassengerId",
                table: "Baggage",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PredefinedCommentId",
                table: "Comments",
                column: "PredefinedCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CountryId",
                table: "Destinations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightBaggage_BaggageId",
                table: "FlightBaggage",
                column: "BaggageId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightComment_FlightId",
                table: "FlightComment",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationFromId",
                table: "Flights",
                column: "DestinationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationToId",
                table: "Flights",
                column: "DestinationToId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ScheduledFlightId",
                table: "Flights",
                column: "ScheduledFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyerCards_AirlineId",
                table: "FrequentFlyerCards",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyerCards_PassengerId",
                table: "FrequentFlyerCards",
                column: "PassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookingDetails_AssociatedPassengerBookingDetailsId",
                table: "PassengerBookingDetails",
                column: "AssociatedPassengerBookingDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookingDetails_PNRId",
                table: "PassengerBookingDetails",
                column: "PNRId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerFlight_FlightId",
                table: "PassengerFlight",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_AssociatedAdultPassengerId",
                table: "Passengers",
                column: "AssociatedAdultPassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                table: "Seats",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_PassengerOrItemId",
                table: "Seats",
                column: "PassengerOrItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialServiceRequests_FlightId",
                table: "SpecialServiceRequests",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialServiceRequests_SSRCodeId",
                table: "SpecialServiceRequests",
                column: "SSRCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APISData");

            migrationBuilder.DropTable(
                name: "FlightBaggage");

            migrationBuilder.DropTable(
                name: "FlightComment");

            migrationBuilder.DropTable(
                name: "FrequentFlyerCards");

            migrationBuilder.DropTable(
                name: "PassengerFlight");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "SpecialServiceRequests");

            migrationBuilder.DropTable(
                name: "Baggage");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "SSRCodes");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "PredefinedComments");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "ScheduledFlights");

            migrationBuilder.DropTable(
                name: "PassengerBookingDetails");

            migrationBuilder.DropTable(
                name: "AircraftTypes");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "SeatMaps");

            migrationBuilder.DropTable(
                name: "BookingReferences");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropSequence(
                name: "BaggageTagsSequence");
        }
    }
}
