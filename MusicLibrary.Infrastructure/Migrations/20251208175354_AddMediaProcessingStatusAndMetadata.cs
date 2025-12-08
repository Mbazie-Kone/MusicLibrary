using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaProcessingStatusAndMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "MediaItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Album",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bitrate",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MediaItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Album",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Artist",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Bitrate",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MediaItems");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "MediaItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
