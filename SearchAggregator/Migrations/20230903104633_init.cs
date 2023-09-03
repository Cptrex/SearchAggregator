using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAggregator.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchAggregatorResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchText = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    SearchResult = table.Column<string>(type: "nvarchar(MAX)", nullable: true, defaultValue: "[]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchAggregatorResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchAggregatorResult_SearchText",
                table: "SearchAggregatorResult",
                column: "SearchText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchAggregatorResult");
        }
    }
}
