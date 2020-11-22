using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version01WithManyDocumentsMasters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MasterId",
                table: "Documents",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Masters_MasterId",
                table: "Documents",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Masters_MasterId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_MasterId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Documents");
        }
    }
}
