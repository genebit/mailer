using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web.Mvc;

namespace Mailer.Controllers
{
    public class HomeController : Controller
    {
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
                    From = new MailAddress("webdevcorp.samp@gmail.com", "WebDev Corp."),
                    Subject = "[FEEDBACK] | Mailer.com",
                    Body = sender.Message,
                    IsBodyHtml = true
                };

                mail.To.Add(sender.Email);

                try
                {
                    SmtpClient smtp = new SmtpClient
                    {
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Host = "smtp.gmail.com",
                        Credentials = new System.Net.NetworkCredential("webdevcorp.samp@gmail.com", "qvpfuhrwlofqxpsb")
                    };
                    smtp.Send(mail);

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (SmtpException e)
                {
                    throw e;
                }
            }

            return Json(false, JsonRequestBehavior.DenyGet);
        }
    }

    public class Sender
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [AllowHtml]
        public string Message { get; set; }
    }
}