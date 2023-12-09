using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AircraftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftTypeIATACode = table.Column<string>(type: "text", nullable: true),
                    ModelName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PNR = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentType = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsMarkedAsRead = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country3LetterCode = table.Column<string>(type: "text", nullable: true),
                    Country2LetterCode = table.Column<string>(type: "text", nullable: true),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    AircraftRegistrationPrefix = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightClassSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    SeatPositionsAvailable = table.Column<string>(type: "jsonb", nullable: true),
                    ExitRowSeats = table.Column<string>(type: "jsonb", nullable: true),
                    BassinetSeats = table.Column<string>(type: "jsonb", nullable: true),
                    NotExistingSeats = table.Column<string>(type: "jsonb", nullable: true),
                    RowRange = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightClassSpecifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialServiceRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FreeText = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsFreeTextMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialServiceRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    PNRId = table.Column<int>(type: "integer", nullable: false),
                    FrequentFlyerId = table.Column<int>(type: "integer", nullable: false),
                    BoardingSequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    BoardingZone = table.Column<string>(type: "text", nullable: false),
                    AcceptanceStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passengers_BookingReferences_PNRId",
                        column: x => x.PNRId,
                        principalTable: "BookingReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarrierCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AccountingCode = table.Column<int>(type: "integer", nullable: true),
                    AirlinePrefix = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airlines_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IATAAirportCode = table.Column<string>(type: "text", nullable: true),
                    AirportName = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SSRCodes",
                columns: table => new
                {
                    SpecialServiceRequestId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSRCodes", x => x.SpecialServiceRequestId);
                    table.ForeignKey(
                        name: "FK_SSRCodes_SpecialServiceRequest_SpecialServiceRequestId",
                        column: x => x.SpecialServiceRequestId,
                        principalTable: "SpecialServiceRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APISData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    NationalityId = table.Column<int>(type: "integer", nullable: false),
                    IssueCountryId = table.Column<int>(type: "integer", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false),
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
                        name: "FK_APISData_Countries_IssueCountryId",
                        column: x => x.IssueCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_APISData_Countries_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SpecialBagType = table.Column<string>(type: "text", nullable: true),
                    SpecialBagDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    IsSpecialBag = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    BaggageType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baggage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baggage_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerComment",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerComment", x => new { x.PassengerId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_PassengerComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerComment_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerSpecialServiceRequest",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SpecialServiceRequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerSpecialServiceRequest", x => new { x.PassengerId, x.SpecialServiceRequestId });
                    table.ForeignKey(
                        name: "FK_PassengerSpecialServiceRequest_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerSpecialServiceRequest_SpecialServiceRequest_Specia~",
                        column: x => x.SpecialServiceRequestId,
                        principalTable: "SpecialServiceRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrequentFlyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FrequentFlyerNumber = table.Column<string>(type: "text", nullable: true),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    CardholderName = table.Column<string>(type: "text", nullable: false),
                    TierLever = table.Column<string>(type: "text", nullable: false),
                    MilesAvailable = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequentFlyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrequentFlyers_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrequentFlyers_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeatMapId = table.Column<string>(type: "text", nullable: true),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    AircraftTypeId = table.Column<int>(type: "integer", nullable: false),
                    FlightClassesSpecification = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                        column: x => x.AircraftTypeId,
                        principalTable: "AircraftTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeatMaps_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    APISDataId = table.Column<int>(type: "integer", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.APISDataId);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_APISData_APISDataId",
                        column: x => x.APISDataId,
                        principalTable: "APISData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagNumbers",
                columns: table => new
                {
                    BaggageId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    TagNumber = table.Column<string>(type: "text", nullable: true),
                    AirlineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagNumbers", x => x.BaggageId);
                    table.ForeignKey(
                        name: "FK_TagNumbers_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagNumbers_Baggage_BaggageId",
                        column: x => x.BaggageId,
                        principalTable: "Baggage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    AircraftTypeId = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    SeatMapId = table.Column<int>(type: "integer", nullable: false),
                    JumpSeatsAvailable = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                        column: x => x.AircraftTypeId,
                        principalTable: "AircraftTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aircrafts_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aircrafts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aircrafts_SeatMaps_SeatMapId",
                        column: x => x.SeatMapId,
                        principalTable: "SeatMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<string>(type: "text", nullable: true),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    SeatType = table.Column<string>(type: "text", nullable: false),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    SeatStatus = table.Column<string>(type: "text", nullable: false),
                    SeatMapId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seat_SeatMaps_SeatMapId",
                        column: x => x.SeatMapId,
                        principalTable: "SeatMaps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "text", nullable: true),
                    DepartureAirportId = table.Column<int>(type: "integer", nullable: false),
                    ArrivalAirportId = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    BoardingStatus = table.Column<string>(type: "text", nullable: false),
                    FlightIdentifier = table.Column<int>(type: "integer", nullable: false),
                    DepartureDateTime = table.Column<string>(type: "text", nullable: false),
                    ArrivalDateTime = table.Column<string>(type: "text", nullable: false),
                    FlightType = table.Column<string>(type: "text", nullable: false),
                    FlightStatus = table.Column<string>(type: "text", nullable: false),
                    TotalBookedPassengers = table.Column<int>(type: "integer", nullable: false),
                    TotalCheckedBaggage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Codeshares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    CodeshareFlightNumber = table.Column<string>(type: "text", nullable: true),
                    CodeshareAirlineId = table.Column<int>(type: "integer", nullable: false),
                    CodeshareFlightIdentifier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codeshares", x => new { x.FlightId, x.Id });
                    table.ForeignKey(
                        name: "FK_Codeshares_Airlines_CodeshareAirlineId",
                        column: x => x.CodeshareAirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Codeshares_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightBaggage",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    BaggageId = table.Column<int>(type: "integer", nullable: false)
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
                name: "PassengerFlight",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    FlightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerFlight", x => new { x.PassengerId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_PassengerFlight_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerFlight_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
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
                name: "IX_APISData_IssueCountryId",
                table: "APISData",
                column: "IssueCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_APISData_NationalityId",
                table: "APISData",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_APISData_PassengerId",
                table: "APISData",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_PassengerId",
                table: "Baggage",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Codeshares_CodeshareAirlineId",
                table: "Codeshares",
                column: "CodeshareAirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CountryId",
                table: "Destinations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightBaggage_BaggageId",
                table: "FlightBaggage",
                column: "BaggageId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalAirportId",
                table: "Flights",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureAirportId",
                table: "Flights",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyers_AirlineId",
                table: "FrequentFlyers",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyers_CardNumber",
                table: "FrequentFlyers",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyers_PassengerId",
                table: "FrequentFlyers",
                column: "PassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassengerComment_CommentId",
                table: "PassengerComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerFlight_FlightId",
                table: "PassengerFlight",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_PNRId",
                table: "Passengers",
                column: "PNRId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerSpecialServiceRequest_SpecialServiceRequestId",
                table: "PassengerSpecialServiceRequest",
                column: "SpecialServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_PassengerId",
                table: "Seat",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_SeatMapId",
                table: "Seat",
                column: "SeatMapId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_AircraftTypeId",
                table: "SeatMaps",
                column: "AircraftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_AirlineId",
                table: "SeatMaps",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_TagNumbers_AirlineId",
                table: "TagNumbers",
                column: "AirlineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codeshares");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "FlightBaggage");

            migrationBuilder.DropTable(
                name: "FlightClassSpecifications");

            migrationBuilder.DropTable(
                name: "FrequentFlyers");

            migrationBuilder.DropTable(
                name: "PassengerComment");

            migrationBuilder.DropTable(
                name: "PassengerFlight");

            migrationBuilder.DropTable(
                name: "PassengerSpecialServiceRequest");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "SSRCodes");

            migrationBuilder.DropTable(
                name: "TagNumbers");

            migrationBuilder.DropTable(
                name: "APISData");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "SpecialServiceRequest");

            migrationBuilder.DropTable(
                name: "Baggage");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "SeatMaps");

            migrationBuilder.DropTable(
                name: "BookingReferences");

            migrationBuilder.DropTable(
                name: "AircraftTypes");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
