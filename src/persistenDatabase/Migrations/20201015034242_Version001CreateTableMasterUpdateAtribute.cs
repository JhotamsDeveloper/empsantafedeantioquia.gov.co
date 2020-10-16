using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001CreateTableMasterUpdateAtribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bidding",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "BiddingEndDate",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "BiddingFile",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "BiddingStartDate",
                table: "Masters");

            migrationBuilder.AddColumn<bool>(
                name: "NacionLicitante",
                table: "Masters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NacionLicitanteEndDate",
                table: "Masters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NacionLicitantegFile",
                table: "Masters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NacionLicitantegStartDate",
                table: "Masters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NacionLicitante",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "NacionLicitanteEndDate",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "NacionLicitantegFile",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "NacionLicitantegStartDate",
                table: "Masters");

            migrationBuilder.AddColumn<bool>(
                name: "Bidding",
                table: "Masters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BiddingEndDate",
                table: "Masters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BiddingFile",
                table: "Masters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BiddingStartDate",
                table: "Masters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
