using Logic.ClientManagement.Et;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Admin_Page.xaml
    /// Einstellungen
    /// Aendern Benutzerdaten, Aendern Teilnehmerdaten, Neues Turnier starten
    /// </summary>
    public partial class Admin_Page : Page
    {
        public Admin_Page()
        {
            InitializeComponent();
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

        //Weiterleitung auf Seite zur Teilnehmerverwaltung
        private void b_competitor_management_Click(object sender, RoutedEventArgs e)
        {
            Competitor_Management CompetitorSettings = new Competitor_Management();
            this.NavigationService.Navigate(CompetitorSettings);
        }

        //Weiterleitung auf Seite fuer Benutzereinstellungen. 
        private void b_usersettings_Click(object sender, RoutedEventArgs e)
        {
            Edit_Account EditAccount = new Edit_Account();
            this.NavigationService.Navigate(EditAccount);
        }

        //Neues Turnier erstellen
        private void b_new_tournament_Click(object sender, RoutedEventArgs e)
        {
            Create_Tournament ct = new Create_Tournament();
            this.NavigationService.Navigate(ct);
        }

        //Zurueck auf Startseite
        private void b_home_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
        }

        #endregion
    }
}