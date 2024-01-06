using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace GestionBib.Migrations
{
    /// <inheritdoc />
    public partial class TableReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomReservateur = table.Column<string>(nullable: true),
                    PrenomReservateur = table.Column<string>(nullable: true),
                    EmailReservateur = table.Column<string>(nullable: true),
                    NumeroTelephone = table.Column<string>(nullable: true),
                    DureeReservation = table.Column<string>(nullable: true),
                    LivreId = table.Column<int>(nullable: false) // Clé étrangère pour l'ID du livre
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Livres_LivreId",
                        column: x => x.LivreId,
                        principalTable: "Livres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    // Assurez-vous de remplacer "Livres" par le vrai nom de votre table de livres
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LivreId",
                table: "Reservations",
                column: "LivreId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

        }
    }

}
