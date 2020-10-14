using Microsoft.EntityFrameworkCore.Migrations;

namespace Contacts.Web.API.Migrations
{
    public partial class RequiredContactForSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills");

            migrationBuilder.AlterColumn<long>(
                name: "ContactId",
                table: "Skills",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills");

            migrationBuilder.AlterColumn<long>(
                name: "ContactId",
                table: "Skills",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Contacts_ContactId",
                table: "Skills",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
