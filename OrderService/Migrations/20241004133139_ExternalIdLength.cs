using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class ExternalIdLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 4, 16, 31, 39, 345, DateTimeKind.Local).AddTicks(2768),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 2, 22, 44, 53, 836, DateTimeKind.Local).AddTicks(3528));

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "Orders",
                type: "VARCHAR(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(40)",
                oldMaxLength: 40,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 22, 44, 53, 836, DateTimeKind.Local).AddTicks(3528),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 4, 16, 31, 39, 345, DateTimeKind.Local).AddTicks(2768));

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "Orders",
                type: "VARCHAR(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}
