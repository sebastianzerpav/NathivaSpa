using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWebSpa.Migrations
{
    /// <inheritdoc />
    public partial class columncategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "spaService",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryServiceCategoryId",
                table: "spaService",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CategoryServices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "CategoryServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_spaService_CategoryServiceCategoryId",
                table: "spaService",
                column: "CategoryServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_spaService_CategoryServices_CategoryServiceCategoryId",
                table: "spaService",
                column: "CategoryServiceCategoryId",
                principalTable: "CategoryServices",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spaService_CategoryServices_CategoryServiceCategoryId",
                table: "spaService");

            migrationBuilder.DropIndex(
                name: "IX_spaService_CategoryServiceCategoryId",
                table: "spaService");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "spaService");

            migrationBuilder.DropColumn(
                name: "CategoryServiceCategoryId",
                table: "spaService");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "CategoryServices");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CategoryServices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
