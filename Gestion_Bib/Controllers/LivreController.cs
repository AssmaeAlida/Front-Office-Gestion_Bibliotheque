using Gestion_Bib.Data;
using Gestion_Bib.Models;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace Gestion_Bib.Controllers
{
    public class LivreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivreController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Votre action pour afficher la liste des livres
        [HttpGet("livre/listeLivre")]
        public IActionResult ListeLivres()
        {
            var livre = _context.Livres.ToList();
            return View(livre); // Renvoie la vue avec la liste de livres
        }

        // Action pour gérer la réservation

        [HttpPost]
        public IActionResult VerifierEmail2(string nom, string prenom, int livreId, int duree, string email, string telephone)
        {
            var livre = _context.Livres.FirstOrDefault(l => l.Id == livreId); // Récupérer le livre par son ID

            if (livre != null)
            {
                var reservation = new Reservation
                {
                    NomReservateur = nom,
                    PrenomReservateur = prenom,
                    EmailReservateur = email,
                    NumeroTelephone = telephone,
                    DureeReservation = duree,
                    LivreId = livre.Id // Enregistrer l'ID du livre dans la réservation
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                // Générer le PDF avec les informations de réservation
                byte[] pdfData = GenerateReservationPDF();

                // Envoyer l'email avec le PDF en pièce jointe
                SendEmailWithAttachment(email, pdfData);

                // Redirection ou retourner une vue de confirmation
                return RedirectToAction("Confirmation");
            }
            else
            {
                // Gérer le cas où le livre n'est pas trouvé
                return RedirectToAction("ListeLivres");
            }
        }

        public IActionResult Confirmation()
        {
            return View("Confirmation2");
        }
        public byte[] GenerateReservationPDF()
        {
            // Créer un nouveau document PDF
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Dessiner du contenu sur la page
            XFont font = new XFont("Arial", 12);
            gfx.DrawString("Contenu du bon de réservation", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            // Sauvegarder le document en mémoire
            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void SendEmailWithAttachment(string recipientEmail, byte[] pdfData)
        {
            // Configuration des informations de messagerie
            string senderEmail = "alidaassmae@gmail.com";
            string senderPassword = "zdlgnhcjwwhbyrek";
            string subject = "Bon de réservation";
            string body = "Merci pour votre réservation! Veuillez trouver ci-joint votre bon de réservation.";

            // Configuration du client SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            // Création du message
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);

            // Ajout du fichier PDF en pièce jointe
            mailMessage.Attachments.Add(new Attachment(new MemoryStream(pdfData), "BonReservation.pdf"));

            // Envoi de l'e-mail
            smtpClient.Send(mailMessage);
        }


    }
}
