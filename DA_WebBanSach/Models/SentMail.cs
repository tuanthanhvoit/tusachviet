using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DA_WebBanSach.Models
{
    public class SentMail
    {
        public static void Send(String from, String to, String subject, String body)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sachviethotro@gmail.com", "Thanh.10")
            };

            var fromAddress = new MailAddress(from);
            var toAddress = new MailAddress(to);
            string bodyE = @"Chúc mừng bạn đã đăng kí thành công.
Click vào link dưới đây để kích hoạt tài khoản của bạn.
http://" + body;
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bodyE,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            smtp.Send(message);
        }

        public static void Send(String from, String to, String subject, String user, String pwToken)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sachviethotro@gmail.com", "Thanh.10")
            };

            var fromAddress = new MailAddress(from);
            var toAddress = new MailAddress(to);
            string bodyE = @"Username: " + user + "\n Click vào link để reset password: " + "\n http://" + pwToken;
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bodyE,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            smtp.Send(message);
        }

        public static void GuiDonHang(String from, String to, String subject, String body)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sachviethotro@gmail.com", "Thanh.10")
            };

            var fromAddress = new MailAddress(from);
            var toAddress = new MailAddress(to);
            string bodyE = body;
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bodyE,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            smtp.Send(message);
        }
    }
}