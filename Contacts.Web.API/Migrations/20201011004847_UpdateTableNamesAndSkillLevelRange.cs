using Microsoft.EntityFrameworkCore.Migrations;

namespace Contacts.Web.API.Migrations
{
    public partial class UpdateTableNamesAndSkillLevelRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillItems_ContactItems_ContactId",
                table: "SkillItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillItems",
                table: "SkillItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactItems",
                table: "ContactItems");

            migrationBuilder.RenameTable(
                name: "SkillItems",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "ContactItems",
                newName: "Contacts");

            migrationBuilder.RenameIndex(
                name: "IX_SkillItems_ContactId",
                table: "Skills",
                newName: "IX_Skills_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "SkillItems");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "ContactItems");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_ContactId",
                table: "SkillItems",
                newName: "IX_SkillItems_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillItems",
                table: "SkillItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactItems",
                table: "ContactItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillItems_ContactItems_ContactId",
                table: "SkillItems",
                column: "ContactId",
                principalTable: "ContactItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
