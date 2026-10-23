using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDBZ.Migrations
{
    /// <inheritdoc />
    public partial class AddedTipoClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DBZ",
                table: "DBZ");

            migrationBuilder.RenameTable(
                name: "DBZ",
                newName: "Personagens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personagens",
                table: "Personagens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personagens",
                table: "Personagens");

            migrationBuilder.RenameTable(
                name: "Personagens",
                newName: "DBZ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DBZ",
                table: "DBZ",
                column: "Id");
        }
    }
}
