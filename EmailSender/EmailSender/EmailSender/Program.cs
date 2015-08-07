using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace EmailSender
{
    class Sender
    {
        static void Main()
        {

            string filePath = @"..\..\Images\publicdiscussionbanner.jpg";
            //string emailBody = "<html><body><h1>Picture</h1><br><p>This is some picture!</p><img src=\"publicdiscussionbanner.jpg\"></body></html>";
            string emailBody = @"<html>
                                <body>
                                <br>"
                                + "<img src=\"publicdiscussionbanner.jpg\">" +
                                @"<h1>Oбществено обсъждане</h1>
                                <p>Проектът се осъществява с финансовата подкрепа на Оперативна програма “Административен капацитет”, съфинансирана от Европейския съюз чрез Европейския социален фонд.!</p>
                                <div>
	                                <p>
	                                Уважаеми господин Манчев,
	                                Това е тестови имейл, чиято цел е да ви накара да цъкнете на следния линк 
                                    <br>
	                                <a href='www.dir.bg'>Цък!</a>
	                                Благодарим за вниманието!
	                                </p>
                                </div>
                                <footer>
	                                СМ Консулта 2015
                                </footer>
                                </body>
                                </html>";


            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (emailBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(filePath, MediaTypeNames.Image.Jpeg);
            inline.ContentId = Guid.NewGuid().ToString();
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage("support@smcon.com", "georgi.borisov@smcon.com");
            mail.AlternateViews.Add(avHtml);
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            mail.Body = String.Format(
                "<h3>Client: " + inline.ContentStream + " Has Sent You A Screenshot</h3>" +
                @"<img src=""cid:{0}"" />", inline.ContentId);

            Attachment att = new Attachment(filePath);
            att.ContentDisposition.Inline = true;
            
            mail.Attachments.Add(att);


            using (SmtpClient client = new SmtpClient("10.10.10.18"))
            {
                //client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("support@smcon.com", "smcsupport12");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                

                client.Send(mail);
            }
        }
    }
}
