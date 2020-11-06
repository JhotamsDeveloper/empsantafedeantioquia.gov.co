using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001CreatePQRSD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PQRSDs",
                columns: table => new
                {
                    PQRSDID = table.Column<Guid>(nullable: false),
                    NamePerson = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PQRSDName = table.Column<string>(nullable: true),
                    Descriptio = table.Column<string>(nullable: true),
                    NameSotypeOfRequest = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    Reply = table.Column<string>(nullable: true),
                    IsAnswered = table.Column<bool>(nullable: false),
                    AnswerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PQRSDs", x => x.PQRSDID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PQRSDs");
        }
    }
}
