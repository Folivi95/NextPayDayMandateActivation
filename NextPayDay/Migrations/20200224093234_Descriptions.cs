using Microsoft.EntityFrameworkCore.Migrations;

namespace NextPayDay.Migrations
{
    public partial class Descriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionOne",
                table: "OTPActivations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTwo",
                table: "OTPActivations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionOne",
                table: "OTPActivations");

            migrationBuilder.DropColumn(
                name: "DescriptionTwo",
                table: "OTPActivations");
        }
    }
}
