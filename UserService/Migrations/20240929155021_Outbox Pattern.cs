using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class OutboxPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValue: new DateTime(2024, 9, 29, 18, 50, 20, 850, DateTimeKind.Local).AddTicks(1092)),
                    Processed = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox");
        }
    }
}
