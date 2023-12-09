using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace QUIZ_IT.Models.SendGmail
{
    public class Gmail
    {

        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        public void SendGmail (string toGmail, string subject, string body)
        {
            var fromAddress = new MailAddress("anvohoang98@gmail.com", "TRẮC NGHIỆM IT - " + subject);
            var toAddress = new MailAddress(toGmail);
            const string fromPassword = "rary emst dpkw cfis";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}