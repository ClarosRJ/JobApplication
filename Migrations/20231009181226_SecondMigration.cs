using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AplicantId",
                table: "Experiences");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AplicantId",
                table: "Experiences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
