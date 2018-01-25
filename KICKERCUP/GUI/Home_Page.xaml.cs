using Logic.ClientManagement.Et;
using System.Windows;
using System.Windows.Controls;


namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Home_Page.xaml
    /// Startseite nach dem Login
    /// Turnierhistorie: leitet weiter zu Tournament_Management (Auflistung aller Turniere des Nutzers, Rangliste, Turnier fortsetzen)
    /// Spielen: Anlegen eines neuen Turniers
    /// Einstellungen: Nutzerdaten aendern, Teilnehmerdaten aendern, neues Turnier starten
    /// </summary>
   
    public partial class Home_Page : Page
    {
        public Home_Page()
        {
            InitializeComponent();

            //Begrüßungsnachricht "Hallo ..." auf der Admin PAge
            txt_welcome.Content = "Hallo " + ApplicationState.GetValue<Client>("LoggedOnUser").Name + "!";
        }

        #region Header

        //Logout
        private void b_logout_Click(object sender, RoutedEventArgs e)
        {
            //Session Variable abmelden
            ApplicationState.SetValue("LoggedOnUser", null);

            //Zur Login Seite navigieren
            Login l = new Login();
            this.NavigationService.Navigate(l);
        }

        //Aufruf der ShareOnFacebook Methode des jeweilig gerade angemeldeten Benutzers
        private void b_fb_share_Click(object sender, RoutedEventArgs e)
        {
            ApplicationState.GetValue<Client>("LoggedOnUser").ShareOnFacebook();
        }

        //Aufruf der ShareOnTwitter Methode des jeweilig gerade angemeldeten Benutzers
        private void b_twitter_share_Click(object sender, RoutedEventArgs e)
        {
            ApplicationState.GetValue<Client>("LoggedOnUser").ShareOnTwitter();
        }

        //Weiterleitung auf die Seite fuer Feedback
        private void b_feedback_Click(object sender, RoutedEventArgs e)
        {
            Feedback f = new Feedback();
            this.NavigationService.Navigate(f);
        }

        #endregion

        #region Navigation

        //Erstellung eines neuen Turniers
        private void b_play_Click(object sender, RoutedEventArgs e)
        {
            //zum Turnier fortsetzen in MessageBox abfragen, ob ein neues Turnier erstellt werden soll oder aktuelles fortgesetzt
            //Turnier fortsetzen außerdem möglich machen ueber Turnierhistorie, falls noch nicht abgeschlossen
            Create_Tournament ct = new Create_Tournament();
            this.NavigationService.Navigate(ct);
        }

        //Weiterleitung zu bisherigen Turnieren
        private void b_tournaments_Click(object sender, RoutedEventArgs e)
        {
            Tournament_Management tm = new Tournament_Management();
            this.NavigationService.Navigate(tm);
        }

        //Weiterleitung zu Einstellungen fuer Benutzer und Teilnehmer
        private void b_settings_Click(object sender, RoutedEventArgs e)
        {
            Admin_Page ap = new Admin_Page();
            this.NavigationService.Navigate(ap);
        }

        #endregion
    }
}