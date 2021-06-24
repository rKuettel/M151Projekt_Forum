using Microsoft.EntityFrameworkCore.Migrations;

namespace forum.business.Migrations
{
    public partial class PictureWithoutAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_AspNetUsers_UploadedById",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Picture_UploadedById",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "UploadedById",
                table: "Picture");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
