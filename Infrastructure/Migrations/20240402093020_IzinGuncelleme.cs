using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IzinGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIzin");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Izinler",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BitisTarihi",
                table: "Izinler",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Izinler_ApplicationUserId",
                table: "Izinler",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Izinler_AspNetUsers_ApplicationUserId",
                table: "Izinler",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Izinler_AspNetUsers_ApplicationUserId",
                table: "Izinler");

            migrationBuilder.DropIndex(
                name: "IX_Izinler_ApplicationUserId",
                table: "Izinler");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Izinler");

            migrationBuilder.DropColumn(
                name: "BitisTarihi",
                table: "Izinler");

            migrationBuilder.CreateTable(
                name: "ApplicationUserIzin",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IzinlerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserIzin", x => new { x.ApplicationUsersId, x.IzinlerId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserIzin_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIzin_Izinler_IzinlerId",
                        column: x => x.IzinlerId,
                        principalTable: "Izinler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIzin_IzinlerId",
                table: "ApplicationUserIzin",
                column: "IzinlerId");
        }
    }
}
