using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QM.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Measurements",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Measurements");
        }
    }
}
