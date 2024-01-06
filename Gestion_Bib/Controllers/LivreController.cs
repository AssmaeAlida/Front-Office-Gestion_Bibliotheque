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
                byte[] pdfData = GenerateReservationPDF(nom, prenom, email, telephone, duree, livre.Titre);

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
        public byte[] GenerateReservationPDF(string nom, string prenom, string email, string telephone, int duree, string titreLivre)
        {
            // Créer un nouveau document PDF
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Ajouter le logo à droite de la page
            XImage logo = XImage.FromFile("C:\\Users\\HP\\source\\repos\\Gestion_Bib\\Gestion_Bib\\wwwroot\\images\\195786.png");
            double logoWidth = 100;
            double logoHeight = 50;
            gfx.DrawImage(logo, page.Width - 40 - logoWidth, 40, logoWidth, logoHeight);

            // Titre
            XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
            gfx.DrawString("Votre réservation a été effectuée avec succès", titleFont, XBrushes.Black, new XRect(40, 100, page.Width - 40 - logoWidth, page.Height), XStringFormats.TopLeft);

            // Tableau pour les informations de réservation
            XFont tableFont = new XFont("Arial", 12);
            XBrush headerBrush = XBrushes.DarkBlue;
            XBrush rowBrush = XBrushes.Black;

            double xPoint = 40, yPoint = 200; // Modifier la valeur de yPoint pour ajuster la position verticale des informations de réservation
            double columnWidth = 150;
            double rowHeight = tableFont.GetHeight();

            string[,] reservationData = {
        { "Nom", nom },
        { "Prénom", prenom },
        { "Email", email },
        { "Téléphone", telephone },
        { "Durée de réservation", duree.ToString() + " jours" },
        { "Livre réservé", titreLivre }
    };

            for (int i = 0; i < reservationData.GetLength(0); i++)
            {
                gfx.DrawString(reservationData[i, 0], tableFont, headerBrush, xPoint, yPoint);
                gfx.DrawString(reservationData[i, 1], tableFont, rowBrush, xPoint + columnWidth, yPoint);
                yPoint += rowHeight + 10; // Ajouter de l'espace entre les lignes
            }

            // Ligne de signature
            XFont signatureFont = new XFont("Arial", 10);
            gfx.DrawString("Signature : ____________________", signatureFont, XBrushes.Black, new XRect(40, page.Height - 50, page.Width - 40 - logoWidth, page.Height), XStringFormats.BottomLeft);

            // Pied de page
            XFont footerFont = new XFont("Arial", 8);
            gfx.DrawString("© 2024 Gestion_Bib. Tous droits réservés.", footerFont, XBrushes.Gray, new XRect(40, page.Height - 20, page.Width - 40 - logoWidth, page.Height), XStringFormats.BottomLeft);

            // Sauvegarder le document en mémoire
            using (var memoryStream = new System.IO.MemoryStream())
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
