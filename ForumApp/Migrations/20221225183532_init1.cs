using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumApp.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityID",
                table: "LikePosts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_CommunityID",
                table: "LikePosts",
                column: "CommunityID");

            migrationBuilder.AddForeignKey(
                name: "FK_LikePosts_Communities_CommunityID",
                table: "LikePosts",
                column: "CommunityID",
                principalTable: "Communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikePosts_Communities_CommunityID",
                table: "LikePosts");

            migrationBuilder.DropIndex(
                name: "IX_LikePosts_CommunityID",
                table: "LikePosts");

            migrationBuilder.DropColumn(
                name: "CommunityID",
                table: "LikePosts");
        }
    }
}
