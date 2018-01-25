using System;
using Logic.ClientManagement.Et;

namespace Logic.Database
{
    public class ClientDL
    {
        public ClientDL()
        {
        }

        //Benutzer erstellen
        public void CreateClient(Client newClient)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Clients.Add(newClient);
            cdl.SaveChanges();
        }

        //Benutzer aktualisieren
        public void UpdateClient(String username, Client newClient)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            Client oldClient = cdl.Clients.Find(username);
            oldClient.Name = newClient.Name;
            oldClient.Surname = newClient.Surname;
            oldClient.Gender = newClient.Gender;
            oldClient.EMail = newClient.EMail;
            oldClient.Password = newClient.Password;
            cdl.SaveChanges();
        }

        //Einen Benutzer finden und zurückgeben
        public Client FindClient(String username)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            return cdl.Clients.Find(username);
        }

        //Einen Benutzer löschen
        public void DeleteClient(String username)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Clients.Remove(cdl.Clients.Find(username));
            cdl.SaveChanges();
        }
    }
}