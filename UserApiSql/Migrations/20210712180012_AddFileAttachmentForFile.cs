using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserApiSql.Migrations
{
    public partial class AddFileAttachmentForFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Attachment",
                table: "Documents",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Documents");
        }
    }
}
