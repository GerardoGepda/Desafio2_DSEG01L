using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BO200360_PD200491_Desafio2.Migrations
{
    /// <inheritdoc />
    public partial class candidatochange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Candidatos",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Candidatos");
        }
    }
}
