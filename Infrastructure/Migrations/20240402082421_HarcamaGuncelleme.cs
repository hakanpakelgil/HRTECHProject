using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HarcamaGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserHarcama");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Harcamalar",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Harcamalar_ApplicationUserId",
                table: "Harcamalar",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harcamalar_AspNetUsers_ApplicationUserId",
                table: "Harcamalar",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Harcamalar_AspNetUsers_ApplicationUserId",
                table: "Harcamalar");

            migrationBuilder.DropIndex(
                name: "IX_Harcamalar_ApplicationUserId",
                table: "Harcamalar");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Harcamalar");

            migrationBuilder.CreateTable(
                name: "ApplicationUserHarcama",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HarcamalarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserHarcama", x => new { x.ApplicationUsersId, x.HarcamalarId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserHarcama_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserHarcama_Harcamalar_HarcamalarId",
                        column: x => x.HarcamalarId,
                        principalTable: "Harcamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHarcama_HarcamalarId",
                table: "ApplicationUserHarcama",
                column: "HarcamalarId");
        }
    }
}
