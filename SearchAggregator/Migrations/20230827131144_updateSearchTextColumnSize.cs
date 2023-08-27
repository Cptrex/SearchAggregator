using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAggregator.Migrations
{
    /// <inheritdoc />
    public partial class updateSearchTextColumnSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SearchText",
                table: "SearchAggregatorResult",
                type: "nvarchar(2048)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SearchText",
                table: "SearchAggregatorResult",
                type: "nvarchar(1024)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)");
        }
    }
}
