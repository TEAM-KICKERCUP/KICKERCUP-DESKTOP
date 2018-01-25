using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Logic.ClientManagement.Et;
using Logic.Database;

namespace Logic.ClientManagement.Impl
{
    public class ClientIMPL
    {
        private ClientDL cdl = new ClientDL();
        private IDictionary<Guid, Client> Clients = new SortedList<Guid, Client>();

        //Benutzer hinzufügen
        public void AddClient(String username, String password, String name, String surname, String eMail,
            String gender)
        {
            String hashedPassword = PasswordHasher.Hash(password);

            Client client = new Client(username, hashedPassword, name, surname, eMail, gender);
            cdl.CreateClient(client);
        }

        //Benutzer aktualisieren
        public void UpdateClient(String username, String password, String name, String surname, String eMail,
            String gender)
        {
            //Falls das Passwort nicht geändert wurde, darf es nicht erneut gehasht werden.
            if (password == "placeholder")
            {
                Client client = new Client(username, cdl.FindClient(username).Password, name, surname, eMail, gender);
                cdl.UpdateClient(username, client);
            }
            else //Falls Passwort geändert wurde wir der komplette User aktualisiert (Neuer Hash muss generiert werden)
            {
                String hashedPassword = PasswordHasher.Hash(password);
                Client client = new Client(username, hashedPassword, name, surname, eMail, gender);
                cdl.UpdateClient(username, client);
            }
        }

        //Einen Benutzer suchen und zurückgeben
        public Client FindClient(String username)
        {
            return cdl.FindClient(username);
        }

        //Einen Benutzer anmelden
        public bool Login(String username, String password)
        {
            if (PasswordHasher.Verify(password, cdl.FindClient(username).Password) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Einen Benutzer löschen
        public void DeleteClient(String username)
        {
            cdl.DeleteClient(username);
        }
    }

    public sealed class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;

        public static string Hash(string password, int iterations)
        {
            //"Salz" erstellen
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //Hash erstellen
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //Hash und Salz kombinierne
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //Hash zu 64BaseString konvertirenren
            var base64Hash = Convert.ToBase64String(hashBytes);

            //Hash formatieren
            return string.Format("$KICKERCUP$PW${0}${1}", iterations, base64Hash);
        }




        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }


        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$KICKERCUP$PW$");
        }


        public static bool Verify(string password, string hashedPassword)
        {
            //Hash checken
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException();
            }

            //Hash zerlegen
            var splittedHashString = hashedPassword.Replace("$KICKERCUP$PW$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //Get Hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //Get Sakz
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //Hash generieren
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //Byteweise vergleichen
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}