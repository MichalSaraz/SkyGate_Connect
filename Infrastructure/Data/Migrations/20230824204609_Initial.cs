using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarrierCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
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
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialBags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpecialBagType = table.Column<string>(type: "text", nullable: false),
                    SpecialBagDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsDescriptionRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialBags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SSRCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSRCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaggageId = table.Column<int>(type: "integer", nullable: false),
                    LeadingDigit = table.Column<byte>(type: "smallint", nullable: false),
                    BagTagIssuerCode = table.Column<int>(type: "integer", nullable: false),
                    AirlineCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagNumbers", x => x.Id);
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
                    AcceptanceStatus = table.Column<string>(type: "text", nullable: false),
                    BoardingSequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    FrequentFlyerId = table.Column<int>(type: "integer", nullable: false)
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
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeatMapId = table.Column<int>(type: "integer", nullable: false),
                    AircraftTypeIATACode = table.Column<string>(type: "text", nullable: true),
                    AircraftType = table.Column<string>(type: "text", nullable: true),
                    AircraftRegistration = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aircrafts_SeatMaps_SeatMapId",
                        column: x => x.SeatMapId,
                        principalTable: "SeatMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialServiceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SSRCodeId = table.Column<int>(type: "integer", nullable: false),
                    FreeText = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsFreeTextMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                        column: x => x.SSRCodeId,
                        principalTable: "SSRCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APISData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<string>(type: "text", nullable: false),
                    DateOfIssue = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<string>(type: "text", nullable: false),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    NationalityId = table.Column<int>(type: "integer", nullable: false),
                    IssueCountryId = table.Column<int>(type: "integer", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false)
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
                        name: "FK_APISData_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
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
                    TagNumberId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    BaggageType = table.Column<string>(type: "text", nullable: false),
                    IsSpecialBag = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SpecialBagId = table.Column<int>(type: "integer", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Baggage_SpecialBags_SpecialBagId",
                        column: x => x.SpecialBagId,
                        principalTable: "SpecialBags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Baggage_TagNumbers_TagNumberId",
                        column: x => x.TagNumberId,
                        principalTable: "TagNumbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrequentFlyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    FrequentFlyerNumber = table.Column<string>(type: "text", nullable: false),
                    TierLever = table.Column<string>(type: "text", nullable: false),
                    MilesAvailable = table.Column<long>(type: "bigint", nullable: false),
                    CardholderName = table.Column<string>(type: "text", nullable: false)
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
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SeatMapId = table.Column<int>(type: "integer", nullable: false),
                    Row = table.Column<byte>(type: "smallint", nullable: false),
                    Position = table.Column<char>(type: "character(1)", nullable: false),
                    SeatType = table.Column<string>(type: "text", nullable: false),
                    SeatStatus = table.Column<string>(type: "text", nullable: false),
                    BoardingZone = table.Column<string>(type: "text", nullable: false),
                    IsChargeable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seats_SeatMaps_SeatMapId",
                        column: x => x.SeatMapId,
                        principalTable: "SeatMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    Class = table.Column<string>(type: "text", nullable: false),
                    NumberOfRows = table.Column<int>(type: "integer", nullable: false),
                    SeatPositionsAvailable = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassConfigurations_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    FlightNumber = table.Column<int>(type: "integer", nullable: false),
                    DepartureDateTime = table.Column<string>(type: "text", nullable: false),
                    ArrivalDateTime = table.Column<string>(type: "text", nullable: false),
                    DepartureAirportId = table.Column<int>(type: "integer", nullable: false),
                    ArrivalAirportId = table.Column<int>(type: "integer", nullable: false),
                    FlightType = table.Column<string>(type: "text", nullable: false),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    FlightStatus = table.Column<string>(type: "text", nullable: false),
                    BoardingId = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_PassengerSpecialServiceRequest_SpecialServiceRequests_Speci~",
                        column: x => x.SpecialServiceRequestId,
                        principalTable: "SpecialServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    BoardingStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boarding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boarding_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
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
                    CodeshareAirlineId = table.Column<int>(type: "integer", nullable: false),
                    CodeshareFlightNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codeshares", x => x.Id);
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
                name: "IX_Aircrafts_AircraftRegistration",
                table: "Aircrafts",
                column: "AircraftRegistration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_APISData_DocumentTypeId",
                table: "APISData",
                column: "DocumentTypeId");

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
                name: "IX_Baggage_SpecialBagId",
                table: "Baggage",
                column: "SpecialBagId");

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_TagNumberId",
                table: "Baggage",
                column: "TagNumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boarding_FlightId",
                table: "Boarding",
                column: "FlightId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingReferences_PNR",
                table: "BookingReferences",
                column: "PNR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassConfigurations_AircraftId",
                table: "ClassConfigurations",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Codeshares_CodeshareAirlineId",
                table: "Codeshares",
                column: "CodeshareAirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Codeshares_FlightId",
                table: "Codeshares",
                column: "FlightId");

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
                name: "IX_FrequentFlyers_FrequentFlyerNumber",
                table: "FrequentFlyers",
                column: "FrequentFlyerNumber",
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
                name: "IX_Seats_PassengerId",
                table: "Seats",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatMapId",
                table: "Seats",
                column: "SeatMapId");

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
                name: "Boarding");

            migrationBuilder.DropTable(
                name: "ClassConfigurations");

            migrationBuilder.DropTable(
                name: "Codeshares");

            migrationBuilder.DropTable(
                name: "FlightBaggage");

            migrationBuilder.DropTable(
                name: "FrequentFlyers");

            migrationBuilder.DropTable(
                name: "PassengerComment");

            migrationBuilder.DropTable(
                name: "PassengerFlight");

            migrationBuilder.DropTable(
                name: "PassengerSpecialServiceRequest");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Baggage");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "SpecialServiceRequests");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "SpecialBags");

            migrationBuilder.DropTable(
                name: "TagNumbers");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "SSRCodes");

            migrationBuilder.DropTable(
                name: "BookingReferences");

            migrationBuilder.DropTable(
                name: "SeatMaps");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
