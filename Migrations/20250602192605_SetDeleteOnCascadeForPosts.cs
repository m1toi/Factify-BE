using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaApp.Migrations
{
    /// <inheritdoc />
    public partial class SetDeleteOnCascadeForPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_PostId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_Posts_PostId",
                table: "UserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_Users_UserId",
                table: "UserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Posts_PostId",
                table: "UserSeenPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Users_UserId",
                table: "UserSeenPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_PostId",
                table: "Messages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_Posts_PostId",
                table: "UserInteractions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_Users_UserId",
                table: "UserInteractions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPosts_Posts_PostId",
                table: "UserSeenPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenPosts_Users_UserId",
                table: "UserSeenPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_PostId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_Posts_PostId",
                table: "UserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_Users_UserId",
                table: "UserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Posts_PostId",
                table: "UserSeenPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenPosts_Users_UserId",
                table: "UserSeenPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_PostId",
                table: "Messages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_Posts_PostId",
                table: "UserInteractions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_Users_UserId",
                table: "UserInteractions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

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
    }
}
