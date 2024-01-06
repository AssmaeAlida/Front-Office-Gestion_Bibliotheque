using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionBib.Migrations
{
    /// <inheritdoc />
    public partial class TableLivre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
            name: "Livres",
        columns: table => new
        {
            Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            Titre = table.Column<string>(nullable: true),
            Autueur = table.Column<string>(nullable: true),
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Livres", x => x.Id);
        });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Livres");

        }
    }
}
