using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Mailer.Controllers
{
    public class HomeController : Controller
    {
        private string serviceEmail;
        private string serviceAppPassword;

        public HomeController()
        {
            this.serviceEmail = ConfigurationManager.AppSettings["ServiceEmail"];
            this.serviceAppPassword = ConfigurationManager.AppSettings["ServiceAppPassword"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMail(Sender sender)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(this.serviceEmail, "Contact Services"),
                    Subject = "[FEEDBACK] | Mailer.com",
                    Body = $"{sender.Message}<br/><div><i>Mailed by user: <b>{sender.Name} ({sender.Email})</b></i></div>",
                    IsBodyHtml = true
                };

                mail.To.Add(this.serviceEmail);

                try
                {
                    SmtpClient smtp = new SmtpClient
                    {
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Host = "smtp.gmail.com",
                        Credentials = new NetworkCredential(this.serviceEmail, this.serviceAppPassword)
                    };
                    smtp.Send(mail);

                    return Json(new { success = true, response = "Email was sent successfully!" }, JsonRequestBehavior.AllowGet);
                }
                catch (SmtpException e)
                {
                    return Json(new { success = false, response = "Daily quota was hit!" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, response = "Invalid form request. Missing fields." }, JsonRequestBehavior.AllowGet);
        }
    }

    public class Sender
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [AllowHtml]
        public string Message { get; set; }
    }
}