using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace eProiect.Helper
{
     public class SendEmail
     { 
          public static bool SendEmailToUser(string email , string name , string subject,  string body)
          {
               try
               {
                    using (var client = new SmtpClient())
                    {
                         client.Host = "smtp.mail.ru";
                         client.Port = 587;
                         client.DeliveryMethod = SmtpDeliveryMethod.Network;
                         client.UseDefaultCredentials = false;
                         client.EnableSsl = true;
                         client.Credentials = new NetworkCredential("utmtest@mail.ru", "EBCTmsdZ6u7S54DNYCjp\r\n");
                         using (var message = new MailMessage(
                             from: new MailAddress("utmtest@mail.ru", "Vasile Berco"),
                             to: new MailAddress(email,  name)
                             ))
                         {
                              message.Subject = subject;
                              message.Body = body;

                              client.Send(message);
                         }
                    }
                    return true;
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false; // Or do something else to indicate failure
               }


          }
     }
}
