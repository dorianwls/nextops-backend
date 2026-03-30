using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextOps.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoty_id",
                table: "product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoty_id",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
