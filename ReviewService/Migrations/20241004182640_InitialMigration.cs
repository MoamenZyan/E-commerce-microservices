using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ReviewerId = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    ProductId = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    Rate = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
