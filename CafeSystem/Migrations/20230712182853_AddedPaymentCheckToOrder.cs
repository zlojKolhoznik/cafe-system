using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentCheckToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentDone",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaymentDone",
                table: "Orders");
        }
    }
}
