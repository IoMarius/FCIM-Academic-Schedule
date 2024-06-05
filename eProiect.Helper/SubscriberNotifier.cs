using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Helper
{
    public class SubscriberNotifier
    {
        public void NotifySubscribersAboutGroupChange(List<string> EmailList, string groupName)
        {
            try
            {
                foreach (var email in EmailList)
                {                    
                    SendEmail.SendEmailToUser(
                        email,
                        "Robul lui Dumnezeu",
                        $"Modificări orar grup {groupName}",
                        $"Au fost efectuate modificări în orarul grupei {groupName}, \n" +
                        "vizitează platforma Orar FCIM pentru a afla mai multe. \n" +
                        "Link: link....."
                    );                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NotifySubscribersAboutGroupChangeAsync(List<string>, string): Error caught: {ex.Message}.\nCall stack:{ex.StackTrace} Details: {ex.InnerException}");
            }

        }        
    }
}
