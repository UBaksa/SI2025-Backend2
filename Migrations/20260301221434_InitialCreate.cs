using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace carGooBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Preduzeca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPIB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preduzeca", x => x.Id);
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreduzeceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Preduzeca_PreduzeceId",
                        column: x => x.PreduzeceId,
                        principalTable: "Preduzeca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "Obavestenje",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatumObjavljivanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavestenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obavestenje_AspNetUsers_AutorId",
                        column: x => x.AutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PonudaVozila",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrzavaU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrzavaI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RadiusI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Istovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duzina = table.Column<double>(type: "float", nullable: false),
                    Tezina = table.Column<double>(type: "float", nullable: false),
                    TipNadogradnje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipKamiona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPreduzeca = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKorisnika = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vreme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonudaVozila", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PonudaVozila_AspNetUsers_IdKorisnika",
                        column: x => x.IdKorisnika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PonudaVozila_Preduzeca_IdPreduzeca",
                        column: x => x.IdPreduzeca,
                        principalTable: "Preduzeca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ponude",
                columns: table => new
                {
                    PonudaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrzavaU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrzavaI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Istovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duzina = table.Column<double>(type: "float", nullable: false),
                    Tezina = table.Column<double>(type: "float", nullable: false),
                    TipNadogradnje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipKamiona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VrstaTereta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZamenaPaleta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    IdPreduzeca = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKorisnika = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vreme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponude", x => x.PonudaId);
                    table.ForeignKey(
                        name: "FK_Ponude_AspNetUsers_IdKorisnika",
                        column: x => x.IdKorisnika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ponude_Preduzeca_IdPreduzeca",
                        column: x => x.IdPreduzeca,
                        principalTable: "Preduzeca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "415a7c65-81dd-4fe3-9c44-9493db860c4b", "415a7c65-81dd-4fe3-9c44-9493db860c4b", "Prevoznik", "PREVOZNIK" },
                    { "415a7c65-81dd-4fe3-9c44-9493db860c4c", "415a7c65-81dd-4fe3-9c44-9493db860c4c", "Kontroler", "KONTROLER" },
                    { "8a000e3b-b915-43f1-b90e-b28075ec8cac", "8a000e3b-b915-43f1-b90e-b28075ec8cac", "Dispecer", "DISPECER" }
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
                name: "IX_AspNetUsers_PreduzeceId",
                table: "AspNetUsers",
                column: "PreduzeceId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Obavestenje_AutorId",
                table: "Obavestenje",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_PonudaVozila_IdKorisnika",
                table: "PonudaVozila",
                column: "IdKorisnika");

            migrationBuilder.CreateIndex(
                name: "IX_PonudaVozila_IdPreduzeca",
                table: "PonudaVozila",
                column: "IdPreduzeca");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_IdKorisnika",
                table: "Ponude",
                column: "IdKorisnika");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_IdPreduzeca",
                table: "Ponude",
                column: "IdPreduzeca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Obavestenje");

            migrationBuilder.DropTable(
                name: "PonudaVozila");

            migrationBuilder.DropTable(
                name: "Ponude");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Preduzeca");
        }
    }
}
