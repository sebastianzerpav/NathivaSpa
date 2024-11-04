using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebSpa.Migrations
{
    /// <inheritdoc />
    public partial class spadb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "spaService",
                type: "decimal(38,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "spaService",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,2)");
        }
    }
}
