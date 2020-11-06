using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001UpdateAtributePQRSD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptio",
                table: "PQRSDs");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PQRSDs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PQRSDs");

            migrationBuilder.AddColumn<string>(
                name: "Descriptio",
                table: "PQRSDs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
