using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001AddTableProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Documents_DocumentoId",
                table: "FileDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Masters_MasterId",
                table: "FileDocuments");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    UrlProduct = table.Column<string>(nullable: true),
                    Icono = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FileDocuments_Documents_DocumentoId",
                table: "FileDocuments",
                column: "DocumentoId",
                principalTable: "Documents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileDocuments_Masters_MasterId",
                table: "FileDocuments",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Documents_DocumentoId",
                table: "FileDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Masters_MasterId",
                table: "FileDocuments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDocuments_Documents_DocumentoId",
                table: "FileDocuments",
                column: "DocumentoId",
                principalTable: "Documents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileDocuments_Masters_MasterId",
                table: "FileDocuments",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
