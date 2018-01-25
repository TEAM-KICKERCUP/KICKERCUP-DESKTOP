using System;
using System.Net;
using System.Net.Mail;

namespace Logic.EMail
{
    public class EMail
    {
        public void SendMail(String txtSubject, String txtBody, String txtToAddress)
        {
            //SMTP Server von System.Net.Mail Referenz erstellen
            SmtpClient MyServer = new SmtpClient();
            MyServer.Host = "smtp.outlook.office365.com";
            MyServer.Port = 587;
            MyServer.EnableSsl = true;


            //SMTP Server Benutzerdaten
            NetworkCredential NC = new NetworkCredential();
            NC.UserName = "USERNAME";
            NC.Password = "PASSWORT!";
            //Benutzerdaten dem Server zuordnen
            MyServer.Credentials = NC;

            //Absendeadresse
            MailAddress from = new MailAddress("EMAIL", "KICKERCUP");
            //Empfängeradresse
            MailAddress to = new MailAddress(txtToAddress);

            //E-Mail aufbauen
            MailMessage Mymessage = new MailMessage(from, to);
            //Mymessage.Bcc.Add("christopher.heid@qunis.de");
            Mymessage.Subject = txtSubject;
            Mymessage.Body = txtBody;

            //E-Mail senden
            MyServer.Send(Mymessage);
        }
    }
}