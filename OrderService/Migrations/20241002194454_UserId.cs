using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 22, 44, 53, 836, DateTimeKind.Local).AddTicks(3528),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 2, 22, 32, 7, 871, DateTimeKind.Local).AddTicks(9074));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 22, 32, 7, 871, DateTimeKind.Local).AddTicks(9074),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 2, 22, 44, 53, 836, DateTimeKind.Local).AddTicks(3528));
        }
    }
}
