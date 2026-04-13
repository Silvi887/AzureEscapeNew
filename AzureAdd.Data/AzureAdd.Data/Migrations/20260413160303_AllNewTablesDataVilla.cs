using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzureAdd.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllNewTablesDataVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    IdAmenity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAmenity = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.IdAmenity);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    IdLocation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameLocation = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.IdLocation);
                });

            migrationBuilder.CreateTable(
                name: "TypePlaces",
                columns: table => new
                {
                    IdTypePlace = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamePlace = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypePlaces", x => x.IdTypePlace);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VillasPenthhouses",
                columns: table => new
                {
                    IdVilla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVilla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdPlace = table.Column<int>(type: "int", nullable: false),
                    VillaInfo = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    VillaAddress = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountRooms = table.Column<int>(type: "int", nullable: false),
                    CountAdults = table.Column<int>(type: "int", nullable: false),
                    CountChildren = table.Column<int>(type: "int", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Parking = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IDManager = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillasPenthhouses", x => x.IdVilla);
                    table.ForeignKey(
                        name: "FK_VillasPenthhouses_AspNetUsers_IDManager",
                        column: x => x.IDManager,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VillasPenthhouses_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "IdLocation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VillasPenthhouses_TypePlaces_IdPlace",
                        column: x => x.IdPlace,
                        principalTable: "TypePlaces",
                        principalColumn: "IdTypePlace",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    IdBooking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdultsCount = table.Column<int>(type: "int", nullable: false),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    TotalPricePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.IdBooking);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_VillasPenthhouses_VillaId",
                        column: x => x.VillaId,
                        principalTable: "VillasPenthhouses",
                        principalColumn: "IdVilla",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVilla",
                columns: table => new
                {
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVilla", x => new { x.UserId, x.VillaId });
                    table.ForeignKey(
                        name: "FK_UserVilla_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserVilla_VillasPenthhouses_VillaId",
                        column: x => x.VillaId,
                        principalTable: "VillasPenthhouses",
                        principalColumn: "IdVilla",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedBacks",
                columns: table => new
                {
                    IdFeedBack = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeedbackMessage = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBacks", x => x.IdFeedBack);
                    table.ForeignKey(
                        name: "FK_FeedBacks_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedBacks_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "IdBooking",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedBacks_VillasPenthhouses_VillaId",
                        column: x => x.VillaId,
                        principalTable: "VillasPenthhouses",
                        principalColumn: "IdVilla",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7699db7d-964f-4782-8209-d76562e0fece", 0, "0e6f3e3c-d052-4a3e-9c68-c295fdc20e89", "admin@horizons.com", true, false, null, "ADMIN@HORIZONS.COM", "ADMIN@HORIZONS.COM", "AQAAAAIAAYagAAAAEPiv+09a1ZOteP6ME55H2EXwJD/RPqoXWWdjfRs+iyFIyCzn70rwq6FnKMy8ntmPCA==", null, false, "4d8e3cac-096a-4b42-82eb-3662256f72d0", false, "admin@horizons.com" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "IdLocation", "NameLocation" },
                values: new object[,]
                {
                    { 1, "Sunny Beach" },
                    { 2, "Golden Sands" },
                    { 3, "Sozopol" },
                    { 4, "Nessebar" },
                    { 5, "Albena" },
                    { 6, "Borovets" },
                    { 7, "Bansko" },
                    { 8, "Pamporovo" },
                    { 9, "Varna" },
                    { 10, "Burgas" }
                });

            migrationBuilder.InsertData(
                table: "TypePlaces",
                columns: new[] { "IdTypePlace", "NamePlace" },
                values: new object[,]
                {
                    { 1, "vila" },
                    { 2, "penthhouse" },
                    { 3, "apartment" },
                    { 4, "Studio" },
                    { 5, "House" },
                    { 6, "Bungalow" },
                    { 7, "Hotel Room" },
                    { 8, "Guest House" }
                });

            migrationBuilder.InsertData(
                table: "VillasPenthhouses",
                columns: new[] { "IdVilla", "Area", "Bathrooms", "Bedrooms", "CountAdults", "CountChildren", "CountRooms", "IDManager", "IdPlace", "ImageUrl", "IsDeleted", "LocationId", "NameVilla", "Parking", "PricePerNight", "VillaAddress", "VillaInfo" },
                values: new object[,]
                {
                    { 1, "200m2", 4, 3, 2, 3, 4, "7699db7d-964f-4782-8209-d76562e0fece", 1, "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2a/44/d7/42/sol-nessebar-palace-all.jpg?w=900&h=500&s=1", false, 2, "Villa Rio", "Yes", 100m, "New str 17", "This is Fantastic Place for relax and enjoy!" },
                    { 2, "400m2", 4, 3, 4, 2, 4, "7699db7d-964f-4782-8209-d76562e0fece", 3, "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2f/ab/45/e6/caption.jpg?w=900&h=500&s=1", false, 2, "Relax", "Yes", 180m, "Balcan str 25", "This is Fantastic Place for relax and enjoy!" },
                    { 3, "500m2", 4, 3, 2, 2, 6, "7699db7d-964f-4782-8209-d76562e0fece", 2, "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/18/46/67/88/cook-s-club-sunny-beach.jpg?w=900&h=500&s=1", false, 2, "Aphrodita", "Yes", 340m, "New str 15", "This is Fantastic Place for relax and enjoy!" },
                    { 4, "140m2", 2, 2, 4, 2, 3, "7699db7d-964f-4782-8209-d76562e0fece", 5, "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85", false, 7, "Mountain Escape", "Yes", 130m, "Pine Street 8", "Cozy mountain house with fireplace and forest view." },
                    { 5, "300m2", 3, 3, 6, 2, 6, "7699db7d-964f-4782-8209-d76562e0fece", 2, "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688", false, 9, "Luxury Penthouse Sky", "Yes", 350m, "City Center 101", "Modern penthouse with panoramic city views." },
                    { 6, "180m2", 2, 3, 5, 3, 4, "7699db7d-964f-4782-8209-d76562e0fece", 5, "https://images.unsplash.com/photo-1572120360610-d971b9d7767c", false, 3, "Family Holiday Home", "Yes", 160m, "Green Park 5", "Perfect for families with kids, large garden included." },
                    { 7, "270m2", 3, 4, 6, 2, 5, "7699db7d-964f-4782-8209-d76562e0fece", 1, "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2", false, 2, "Sunset Paradise", "Yes", 240m, "Sunset Blvd 77", "Enjoy stunning sunsets over the sea every evening." },
                    { 8, "45m2", 1, 1, 2, 0, 1, "7699db7d-964f-4782-8209-d76562e0fece", 4, "https://images.unsplash.com/photo-1554995207-c18c203602cb", false, 10, "Budget Stay Studio", "No", 60m, "Beach Street 3", "Affordable and comfortable place near the beach." },
                    { 9, "260m2", 3, 4, 6, 2, 5, "7699db7d-964f-4782-8209-d76562e0fece", 1, "https://images.unsplash.com/photo-1502005229762-cf1b2da7c5d6", false, 2, "Ocean Breeze Villa", "Yes", 280m, "Ocean Drive 12", "Beautiful seaside villa with private pool." },
                    { 10, "210m2", 2, 2, 4, 1, 4, "7699db7d-964f-4782-8209-d76562e0fece", 2, "https://images.unsplash.com/photo-1493809842364-78817add7ffb", false, 9, "City Lights Penthouse", "Yes", 320m, "Downtown 55", "Luxury penthouse with skyline view." },
                    { 11, "150m2", 2, 2, 4, 2, 3, "7699db7d-964f-4782-8209-d76562e0fece", 5, "https://images.unsplash.com/photo-1568605114967-8130f3a36994", false, 4, "Green Garden House", "Yes", 120m, "Garden Road 6", "Quiet house surrounded by nature." },
                    { 12, "90m2", 1, 1, 3, 1, 2, "7699db7d-964f-4782-8209-d76562e0fece", 6, "https://images.unsplash.com/photo-1505691723518-36a5ac3be353", false, 2, "Beachfront Bungalow", "No", 140m, "Coastline 1", "Relax right on the beach with amazing views." },
                    { 13, "80m2", 1, 1, 2, 1, 2, "7699db7d-964f-4782-8209-d76562e0fece", 7, "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa", false, 8, "Luxury Hotel Suite", "Yes", 200m, "Hotel Avenue 99", "Premium hotel room with all services included." },
                    { 14, "130m2", 2, 2, 4, 2, 3, "7699db7d-964f-4782-8209-d76562e0fece", 8, "https://images.unsplash.com/photo-1523217582562-09d0def993a6", false, 6, "Cozy Guest House", "Yes", 110m, "Village Center 10", "Warm and welcoming guest house." },
                    { 15, "120m2", 2, 2, 4, 1, 3, "7699db7d-964f-4782-8209-d76562e0fece", 3, "https://images.unsplash.com/photo-1493666438817-866a91353ca9", false, 9, "Modern Apartment Plus", "No", 150m, "Central Blvd 45", "Stylish apartment in the heart of the city." },
                    { 16, "320m2", 3, 3, 6, 2, 5, "7699db7d-964f-4782-8209-d76562e0fece", 2, "https://images.unsplash.com/photo-1501183638710-841dd1904471", false, 9, "Elite Sky Penthouse", "Yes", 400m, "Sky Tower 200", "Top floor penthouse with private jacuzzi." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VillaId",
                table: "Bookings",
                column: "VillaId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_BookingId",
                table: "FeedBacks",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_GuestId",
                table: "FeedBacks",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_VillaId",
                table: "FeedBacks",
                column: "VillaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVilla_VillaId",
                table: "UserVilla",
                column: "VillaId");

            migrationBuilder.CreateIndex(
                name: "IX_VillasPenthhouses_IDManager",
                table: "VillasPenthhouses",
                column: "IDManager");

            migrationBuilder.CreateIndex(
                name: "IX_VillasPenthhouses_IdPlace",
                table: "VillasPenthhouses",
                column: "IdPlace");

            migrationBuilder.CreateIndex(
                name: "IX_VillasPenthhouses_LocationId",
                table: "VillasPenthhouses",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FeedBacks");

            migrationBuilder.DropTable(
                name: "UserVilla");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "VillasPenthhouses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "TypePlaces");
        }
    }
}
