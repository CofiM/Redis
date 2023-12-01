using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumApp.Migrations
{
    public partial class m4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionCommunityUser_Communities_CommunityID",
                table: "ConnectionCommunityUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionCommunityUser_Users_UserID",
                table: "ConnectionCommunityUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectionCommunityUser",
                table: "ConnectionCommunityUser");

            migrationBuilder.RenameTable(
                name: "ConnectionCommunityUser",
                newName: "Connections");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectionCommunityUser_UserID",
                table: "Connections",
                newName: "IX_Connections_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectionCommunityUser_CommunityID",
                table: "Connections",
                newName: "IX_Connections_CommunityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Communities_CommunityID",
                table: "Connections",
                column: "CommunityID",
                principalTable: "Communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_UserID",
                table: "Connections",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Communities_CommunityID",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_UserID",
                table: "Connections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.RenameTable(
                name: "Connections",
                newName: "ConnectionCommunityUser");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_UserID",
                table: "ConnectionCommunityUser",
                newName: "IX_ConnectionCommunityUser_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_CommunityID",
                table: "ConnectionCommunityUser",
                newName: "IX_ConnectionCommunityUser_CommunityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectionCommunityUser",
                table: "ConnectionCommunityUser",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionCommunityUser_Communities_CommunityID",
                table: "ConnectionCommunityUser",
                column: "CommunityID",
                principalTable: "Communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionCommunityUser_Users_UserID",
                table: "ConnectionCommunityUser",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
