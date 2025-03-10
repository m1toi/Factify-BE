using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaApp.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPost_Posts_PostId",
                table: "UserSeenPost");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPost_Users_UserId",
                table: "UserSeenPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSeenPost",
                table: "UserSeenPost");

            migrationBuilder.RenameTable(
                name: "UserSeenPost",
                newName: "UserSeenPosts");

            migrationBuilder.RenameIndex(
                name: "IX_UserSeenPost_PostId",
                table: "UserSeenPosts",
                newName: "IX_UserSeenPosts_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSeenPosts",
                table: "UserSeenPosts",
                columns: new[] { "UserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPosts_Posts_PostId",
                table: "UserSeenPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPosts_Users_UserId",
                table: "UserSeenPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Posts_PostId",
                table: "UserSeenPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Users_UserId",
                table: "UserSeenPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSeenPosts",
                table: "UserSeenPosts");

            migrationBuilder.RenameTable(
                name: "UserSeenPosts",
                newName: "UserSeenPost");

            migrationBuilder.RenameIndex(
                name: "IX_UserSeenPosts_PostId",
                table: "UserSeenPost",
                newName: "IX_UserSeenPost_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSeenPost",
                table: "UserSeenPost",
                columns: new[] { "UserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPost_Posts_PostId",
                table: "UserSeenPost",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPost_Users_UserId",
                table: "UserSeenPost",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
