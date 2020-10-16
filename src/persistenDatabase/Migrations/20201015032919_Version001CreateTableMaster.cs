using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001CreateTableMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Masters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameMaster = table.Column<string>(nullable: true),
                    UrlMaster = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Statud = table.Column<bool>(nullable: false),
                    Brigade = table.Column<bool>(nullable: false),
                    DateBrigade = table.Column<DateTime>(nullable: false),
                    Blog = table.Column<bool>(nullable: false),
                    Bidding = table.Column<bool>(nullable: false),
                    BiddingStartDate = table.Column<DateTime>(nullable: false),
                    BiddingEndDate = table.Column<DateTime>(nullable: false),
                    BiddingFile = table.Column<DateTime>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Masters");
        }
    }
}
