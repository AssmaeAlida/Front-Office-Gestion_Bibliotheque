using Gestion_Bib.Data;
using Gestion_Bib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;


namespace Gestion_Bib.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult VerifierEmail(string nom, string prenom, string livre, int duree, string email, string telephone)
        {
            var livreId = _context.Livres.FirstOrDefault(l => l.Titre == livre)?.Id;

            if (livreId != null)
            {
                var reservation = new Reservation
                {
                    NomReservateur = nom,
                    PrenomReservateur = prenom,
                    EmailReservateur = email,
                    NumeroTelephone = telephone,
                    DureeReservation = duree,
                    LivreId = (int)livreId
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                // Générer le fichier PDF après la réservation
                byte[] pdfData = GeneratePDF();

                // Envoyer l'e-mail avec le PDF en pièce jointe
                SendEmailWithAttachment(email, pdfData);

                // Redirection ou retourner une vue de confirmation
                return RedirectToAction("Confirmation");
                // Rediriger ou effectuer d'autres actions après l'enregistrement
            }
            else
            {
                // Gérer le cas où le livre n'est pas trouvé
                return RedirectToAction("Index");
            }
        }
                

public byte[] GeneratePDF()
    {
        // Créer un nouveau document PDF
        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Dessiner du contenu sur la page
        XFont font = new XFont("Arial", 12);
        gfx.DrawString("Contenu du bon de réservation", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

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
        mailMessage.Attachments.Add(new Attachment(new System.IO.MemoryStream(pdfData), "BonReservation.pdf"));

        // Envoi de l'e-mail
        smtpClient.Send(mailMessage);
    }


}
}
