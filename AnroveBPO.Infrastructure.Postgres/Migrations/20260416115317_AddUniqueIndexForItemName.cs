using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnroveBPO.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexForItemName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ux_items_name",
                table: "items",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_items_name",
                table: "items");
        }
    }
}
