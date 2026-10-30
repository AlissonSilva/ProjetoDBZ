using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDBZ.Migrations
{
    /// <inheritdoc />
    public partial class FixPersonagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "Personagens",
                newName: "Descricao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Personagens",
                newName: "descricao");
        }
    }
}
