using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class OrderPaymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 4, 16, 35, 5, 769, DateTimeKind.Local).AddTicks(6886),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 4, 16, 31, 39, 345, DateTimeKind.Local).AddTicks(2768));

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 4, 16, 31, 39, 345, DateTimeKind.Local).AddTicks(2768),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 4, 16, 35, 5, 769, DateTimeKind.Local).AddTicks(6886));
        }
    }
}
