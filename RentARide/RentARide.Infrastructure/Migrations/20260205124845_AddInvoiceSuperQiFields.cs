using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentARide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceSuperQiFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SuperQiPaymentId",
                table: "Invoices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuperQiPaymentRequestId",
                table: "Invoices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuperQiPaymentId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SuperQiPaymentRequestId",
                table: "Invoices");
        }
    }
}
