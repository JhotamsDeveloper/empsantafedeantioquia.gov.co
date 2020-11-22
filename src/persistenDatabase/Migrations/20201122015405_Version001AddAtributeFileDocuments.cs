using Microsoft.EntityFrameworkCore.Migrations;

namespace persistenDatabase.Migrations
{
    public partial class Version001AddAtributeFileDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Documents_DocumentID",
                table: "FileDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FileDocuments_DocumentID",
                table: "FileDocuments");

            migrationBuilder.DropColumn(
                name: "NacionLicitantegFile",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "DocumentID",
                table: "FileDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "RouteFile",
                table: "FileDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameFile",
                table: "FileDocuments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "FileDocuments",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileDocuments_DocumentoId",
                table: "FileDocuments",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDocuments_MasterId",
                table: "FileDocuments",
                column: "MasterId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Documents_DocumentoId",
                table: "FileDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileDocuments_Masters_MasterId",
                table: "FileDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FileDocuments_DocumentoId",
                table: "FileDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FileDocuments_MasterId",
                table: "FileDocuments");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "FileDocuments");

            migrationBuilder.AddColumn<string>(
                name: "NacionLicitantegFile",
                table: "Masters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RouteFile",
                table: "FileDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "NameFile",
                table: "FileDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "DocumentID",
                table: "FileDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_FileDocuments_DocumentID",
                table: "FileDocuments",
                column: "DocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDocuments_Documents_DocumentID",
                table: "FileDocuments",
                column: "DocumentID",
                principalTable: "Documents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
