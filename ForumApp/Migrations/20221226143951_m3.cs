using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumApp.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityUser");

            migrationBuilder.CreateTable(
                name: "ConnectionCommunityUser",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CommunityID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionCommunityUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConnectionCommunityUser_Communities_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Communities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConnectionCommunityUser_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionCommunityUser_CommunityID",
                table: "ConnectionCommunityUser",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionCommunityUser_UserID",
                table: "ConnectionCommunityUser",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionCommunityUser");

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
                name: "IX_CommunityUser_UsersID",
                table: "CommunityUser",
                column: "UsersID");
        }
    }
}
