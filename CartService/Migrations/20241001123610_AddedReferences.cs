using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class AddedReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "ProductCarts",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 1, 15, 36, 9, 804, DateTimeKind.Local).AddTicks(9295),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 1, 0, 15, 4, 427, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.CreateIndex(
                name: "IX_ProductCarts_CartId",
                table: "ProductCarts",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCarts_Carts_CartId",
                table: "ProductCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCarts_Carts_CartId",
                table: "ProductCarts");

            migrationBuilder.DropIndex(
                name: "IX_ProductCarts_CartId",
                table: "ProductCarts");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "ProductCarts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Outbox",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 1, 0, 15, 4, 427, DateTimeKind.Local).AddTicks(9746),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2024, 10, 1, 15, 36, 9, 804, DateTimeKind.Local).AddTicks(9295));
        }
    }
}
