using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumApp.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_CommentPostID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CommentUserID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_UserID",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikeComments_Comments_CommnetDislikeID",
                table: "DislikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikeComments_Users_CommentUserID",
                table: "DislikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikePosts_Posts_DislikePostPostID",
                table: "DislikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikePosts_Users_DislikePostUserID",
                table: "DislikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeComments_Comments_CommnetLikeID",
                table: "LikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeComments_Users_CommentUserID",
                table: "LikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikePosts_Posts_LikePostPostID",
                table: "LikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikePosts_Users_LikePostUserID",
                table: "LikePosts");

            migrationBuilder.DropIndex(
                name: "IX_Communities_UserID",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Communities");

            migrationBuilder.RenameColumn(
                name: "LikePostUserID",
                table: "LikePosts",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "LikePostPostID",
                table: "LikePosts",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_LikePosts_LikePostUserID",
                table: "LikePosts",
                newName: "IX_LikePosts_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_LikePosts_LikePostPostID",
                table: "LikePosts",
                newName: "IX_LikePosts_PostID");

            migrationBuilder.RenameColumn(
                name: "CommnetLikeID",
                table: "LikeComments",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CommentUserID",
                table: "LikeComments",
                newName: "CommnetID");

            migrationBuilder.RenameIndex(
                name: "IX_LikeComments_CommnetLikeID",
                table: "LikeComments",
                newName: "IX_LikeComments_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_LikeComments_CommentUserID",
                table: "LikeComments",
                newName: "IX_LikeComments_CommnetID");

            migrationBuilder.RenameColumn(
                name: "DislikePostUserID",
                table: "DislikePosts",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "DislikePostPostID",
                table: "DislikePosts",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikePosts_DislikePostUserID",
                table: "DislikePosts",
                newName: "IX_DislikePosts_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikePosts_DislikePostPostID",
                table: "DislikePosts",
                newName: "IX_DislikePosts_PostID");

            migrationBuilder.RenameColumn(
                name: "CommnetDislikeID",
                table: "DislikeComments",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CommentUserID",
                table: "DislikeComments",
                newName: "CommnetID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikeComments_CommnetDislikeID",
                table: "DislikeComments",
                newName: "IX_DislikeComments_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikeComments_CommentUserID",
                table: "DislikeComments",
                newName: "IX_DislikeComments_CommnetID");

            migrationBuilder.RenameColumn(
                name: "CommentUserID",
                table: "Comments",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CommentPostID",
                table: "Comments",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentUserID",
                table: "Comments",
                newName: "IX_Comments_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentPostID",
                table: "Comments",
                newName: "IX_Comments_PostID");

            migrationBuilder.AddColumn<int>(
                name: "CommunityID",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunityUser",
                columns: table => new
                {
                    CommunitiesID = table.Column<int>(type: "int", nullable: false),
                    UsersID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityUser", x => new { x.CommunitiesID, x.UsersID });
                    table.ForeignKey(
                        name: "FK_CommunityUser_Communities_CommunitiesID",
                        column: x => x.CommunitiesID,
                        principalTable: "Communities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityUser_Users_UsersID",
                        column: x => x.UsersID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommunityID",
                table: "Posts",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUser_UsersID",
                table: "CommunityUser",
                column: "UsersID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserID",
                table: "Comments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikeComments_Comments_CommnetID",
                table: "DislikeComments",
                column: "CommnetID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikeComments_Users_UserID",
                table: "DislikeComments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikePosts_Posts_PostID",
                table: "DislikePosts",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikePosts_Users_UserID",
                table: "DislikePosts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikeComments_Comments_CommnetID",
                table: "LikeComments",
                column: "CommnetID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikeComments_Users_UserID",
                table: "LikeComments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikePosts_Posts_PostID",
                table: "LikePosts",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikePosts_Users_UserID",
                table: "LikePosts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityID",
                table: "Posts",
                column: "CommunityID",
                principalTable: "Communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikeComments_Comments_CommnetID",
                table: "DislikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikeComments_Users_UserID",
                table: "DislikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikePosts_Posts_PostID",
                table: "DislikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DislikePosts_Users_UserID",
                table: "DislikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeComments_Comments_CommnetID",
                table: "LikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeComments_Users_UserID",
                table: "LikeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikePosts_Posts_PostID",
                table: "LikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikePosts_Users_UserID",
                table: "LikePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityID",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "CommunityUser");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CommunityID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommunityID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "LikePosts",
                newName: "LikePostUserID");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "LikePosts",
                newName: "LikePostPostID");

            migrationBuilder.RenameIndex(
                name: "IX_LikePosts_UserID",
                table: "LikePosts",
                newName: "IX_LikePosts_LikePostUserID");

            migrationBuilder.RenameIndex(
                name: "IX_LikePosts_PostID",
                table: "LikePosts",
                newName: "IX_LikePosts_LikePostPostID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "LikeComments",
                newName: "CommnetLikeID");

            migrationBuilder.RenameColumn(
                name: "CommnetID",
                table: "LikeComments",
                newName: "CommentUserID");

            migrationBuilder.RenameIndex(
                name: "IX_LikeComments_UserID",
                table: "LikeComments",
                newName: "IX_LikeComments_CommnetLikeID");

            migrationBuilder.RenameIndex(
                name: "IX_LikeComments_CommnetID",
                table: "LikeComments",
                newName: "IX_LikeComments_CommentUserID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "DislikePosts",
                newName: "DislikePostUserID");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "DislikePosts",
                newName: "DislikePostPostID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikePosts_UserID",
                table: "DislikePosts",
                newName: "IX_DislikePosts_DislikePostUserID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikePosts_PostID",
                table: "DislikePosts",
                newName: "IX_DislikePosts_DislikePostPostID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "DislikeComments",
                newName: "CommnetDislikeID");

            migrationBuilder.RenameColumn(
                name: "CommnetID",
                table: "DislikeComments",
                newName: "CommentUserID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikeComments_UserID",
                table: "DislikeComments",
                newName: "IX_DislikeComments_CommnetDislikeID");

            migrationBuilder.RenameIndex(
                name: "IX_DislikeComments_CommnetID",
                table: "DislikeComments",
                newName: "IX_DislikeComments_CommentUserID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Comments",
                newName: "CommentUserID");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "Comments",
                newName: "CommentPostID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                newName: "IX_Comments_CommentUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostID",
                table: "Comments",
                newName: "IX_Comments_CommentPostID");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Communities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_UserID",
                table: "Communities",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_CommentPostID",
                table: "Comments",
                column: "CommentPostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CommentUserID",
                table: "Comments",
                column: "CommentUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_UserID",
                table: "Communities",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikeComments_Comments_CommnetDislikeID",
                table: "DislikeComments",
                column: "CommnetDislikeID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikeComments_Users_CommentUserID",
                table: "DislikeComments",
                column: "CommentUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikePosts_Posts_DislikePostPostID",
                table: "DislikePosts",
                column: "DislikePostPostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DislikePosts_Users_DislikePostUserID",
                table: "DislikePosts",
                column: "DislikePostUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikeComments_Comments_CommnetLikeID",
                table: "LikeComments",
                column: "CommnetLikeID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikeComments_Users_CommentUserID",
                table: "LikeComments",
                column: "CommentUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikePosts_Posts_LikePostPostID",
                table: "LikePosts",
                column: "LikePostPostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikePosts_Users_LikePostUserID",
                table: "LikePosts",
                column: "LikePostUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
