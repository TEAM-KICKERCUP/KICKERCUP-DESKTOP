using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Logic.CompetitorManagement.Et;

namespace Logic.ClientManagement.Et
{
    public class Client : IShareable
    {
        //Attribute eines Benutzers
        [Key] public String Username { get; set; }
        [Required] public String Password { get; set; }
        [Required] public String Name { get; set; }
        [Required] public String Surname { get; set; }
        [Required] public String Gender { get; set; }

        [Required] public String EMail { get; set; }

        //Key Constraint
        public ICollection<Competitor> Competitors { get; set; }

        //Empty constructor is required by Entity Framework for some cases
        public Client()
        {
        }

        //Konstruktur
        public Client(String Username, String Password, String Name, String Surname, String EMail, String Gender)
        {
            this.Username = Username;
            this.Password = Password;
            this.Name = Name;
            this.Surname = Surname;
            this.EMail = EMail;
            this.Gender = Gender;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Client c = (Client)obj;
            return (Username == c.Username) && (Name == c.Name) && (Surname == c.Surname) && (Gender == c.Gender) && (EMail == c.EMail);
        }

        public override int GetHashCode()
        {
            return this.Username.GetHashCode()
                    ^ this.Name.GetHashCode()
                    ^ this.Surname.GetHashCode()
                    ^ this.Gender.GetHashCode()
                    ^ this.EMail.GetHashCode();
        }

        //Gibt den Vornamen + Nachname zurück
        public String GetFullName()
        {
            return Name + " " + Surname;
        }


        //Facebook API Integration
        public void ShareOnFacebook()
        {
            Process.Start(
                "https://www.facebook.com/sharer/sharer.php?u=http://lebensmomente.com/wp-content/uploads/2018/01/kickercup.png&quote=KICKERCUP für Windows ist eine super Unterstüzung für Tischkickerturniere");
        }

        //Twitter API Integration
        public void ShareOnTwitter()
        {
            Process.Start(
                "https://twitter.com/intent/tweet?via=KICKERCUP&text=KICKERCUP für Windows ist eine super Unterstüzung für Tischkickerturniere");
        }
    }
}