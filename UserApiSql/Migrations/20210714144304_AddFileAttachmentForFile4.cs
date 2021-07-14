using Microsoft.EntityFrameworkCore.Migrations;

namespace UserApiSql.Migrations
{
    public partial class AddFileAttachmentForFile4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attachment",
                table: "Documents",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Documents",
                newName: "Attachment");
        }
    }
}
