using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001TableBiddingsParticipantsRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "BiddingParticipants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingParticipants_MasterId",
                table: "BiddingParticipants",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingParticipants_Masters_MasterId",
                table: "BiddingParticipants",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingParticipants_Masters_MasterId",
                table: "BiddingParticipants");

            migrationBuilder.DropIndex(
                name: "IX_BiddingParticipants_MasterId",
                table: "BiddingParticipants");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "BiddingParticipants");
        }
    }
}
