using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Bib.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NomReservateur { get; set; }

        [Required]
        public string PrenomReservateur { get; set; }

        [Required]
        [EmailAddress]
        public string EmailReservateur { get; set; }

        [Required]
        [Phone]
        public string NumeroTelephone { get; set; }

        [Required]
        public int DureeReservation { get; set; }

        // Clé étrangère vers la table Livre
        [ForeignKey("Livre")]
        public int LivreId { get; set; }
        public Livre Livre { get; set; }
    }
}

