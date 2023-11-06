using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsClosedPeriodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Periods",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Periods");
        }
    }
}
