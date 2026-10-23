using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDBZ.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Personagens");

            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "Personagens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_TipoId",
                table: "Personagens",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personagens_Tipos_TipoId",
                table: "Personagens",
                column: "TipoId",
                principalTable: "Tipos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personagens_Tipos_TipoId",
                table: "Personagens");

            migrationBuilder.DropIndex(
                name: "IX_Personagens_TipoId",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "Personagens");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Personagens",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
