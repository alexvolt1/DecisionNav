using Microsoft.EntityFrameworkCore.Migrations;

namespace DecisionNav.Data.Migrations
{
    public partial class addNavigation_ViewGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ParentID = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    Href = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
