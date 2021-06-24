using Microsoft.EntityFrameworkCore.Migrations;

namespace forum.business.Migrations
{
    public partial class picturesInDiscussion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_Discussions_DiscussionId",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "Picture");

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "Picture",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UploadedById",
                table: "Picture",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Picture_UploadedById",
                table: "Picture",
                column: "UploadedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_AspNetUsers_UploadedById",
                table: "Picture",
                column: "UploadedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_Discussions_DiscussionId",
                table: "Picture",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_AspNetUsers_UploadedById",
                table: "Picture");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_Discussions_DiscussionId",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Picture_UploadedById",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "UploadedById",
                table: "Picture");

            migrationBuilder.AlterColumn<int>(
                name: "DiscussionId",
                table: "Picture",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedBy",
                table: "Picture",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_Discussions_DiscussionId",
                table: "Picture",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
