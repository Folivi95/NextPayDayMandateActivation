using Microsoft.EntityFrameworkCore.Migrations;

namespace NextPayDay.Migrations
{
    public partial class DescriptionValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionOneValue",
                table: "OTPActivations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTwoValue",
                table: "OTPActivations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionOneValue",
                table: "OTPActivations");

            migrationBuilder.DropColumn(
                name: "DescriptionTwoValue",
                table: "OTPActivations");
        }
    }
}
