using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class OrderSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ItemSequence");

            migrationBuilder.CreateSequence(
                name: "ItemSequence",
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence<int>(
                name: "OrderSequence",
                maxValue: 2147483647L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ItemSequence");

            migrationBuilder.DropSequence(
                name: "OrderSequence");

            migrationBuilder.CreateSequence<int>(
                name: "ItemSequence",
                maxValue: 2147483647L);
        }
    }
}
