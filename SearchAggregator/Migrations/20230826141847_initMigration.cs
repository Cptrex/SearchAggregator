using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAggregator.Migrations
{
    /// <inheritdoc />
    public partial class initMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchAggregatorResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YandexResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BingResult = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchAggregatorResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchAggregatorResults");
        }
    }
}
