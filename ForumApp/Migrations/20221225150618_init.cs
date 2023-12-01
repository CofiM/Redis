using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Communities_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Posts_Topics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentUserID = table.Column<int>(type: "int", nullable: true),
                    CommentPostID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_CommentPostID",
                        column: x => x.CommentPostID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_CommentUserID",
                        column: x => x.CommentUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DislikePosts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DislikePostUserID = table.Column<int>(type: "int", nullable: true),
                    DislikePostPostID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DislikePosts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DislikePosts_Posts_DislikePostPostID",
                        column: x => x.DislikePostPostID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DislikePosts_Users_DislikePostUserID",
                        column: x => x.DislikePostUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikePosts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikePostUserID = table.Column<int>(type: "int", nullable: true),
                    LikePostPostID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikePosts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LikePosts_Posts_LikePostPostID",
                        column: x => x.LikePostPostID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikePosts_Users_LikePostUserID",
                        column: x => x.LikePostUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DislikeComments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommnetDislikeID = table.Column<int>(type: "int", nullable: true),
                    CommentUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DislikeComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DislikeComments_Comments_CommnetDislikeID",
                        column: x => x.CommnetDislikeID,
                        principalTable: "Comments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DislikeComments_Users_CommentUserID",
                        column: x => x.CommentUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikeComments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommnetLikeID = table.Column<int>(type: "int", nullable: true),
                    CommentUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LikeComments_Comments_CommnetLikeID",
                        column: x => x.CommnetLikeID,
                        principalTable: "Comments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeComments_Users_CommentUserID",
                        column: x => x.CommentUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentPostID",
                table: "Comments",
                column: "CommentPostID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentUserID",
                table: "Comments",
                column: "CommentUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_UserID",
                table: "Communities",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DislikeComments_CommentUserID",
                table: "DislikeComments",
                column: "CommentUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DislikeComments_CommnetDislikeID",
                table: "DislikeComments",
                column: "CommnetDislikeID");

            migrationBuilder.CreateIndex(
                name: "IX_DislikePosts_DislikePostPostID",
                table: "DislikePosts",
                column: "DislikePostPostID");

            migrationBuilder.CreateIndex(
                name: "IX_DislikePosts_DislikePostUserID",
                table: "DislikePosts",
                column: "DislikePostUserID");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_CommentUserID",
                table: "LikeComments",
                column: "CommentUserID");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_CommnetLikeID",
                table: "LikeComments",
                column: "CommnetLikeID");

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_LikePostPostID",
                table: "LikePosts",
                column: "LikePostPostID");

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_LikePostUserID",
                table: "LikePosts",
                column: "LikePostUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopicID",
                table: "Posts",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserID",
                table: "Posts",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "DislikeComments");

            migrationBuilder.DropTable(
                name: "DislikePosts");

            migrationBuilder.DropTable(
                name: "LikeComments");

            migrationBuilder.DropTable(
                name: "LikePosts");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
