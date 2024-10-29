using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSCodeDatabase.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelPropertyToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Categories");
        }
    }
}
