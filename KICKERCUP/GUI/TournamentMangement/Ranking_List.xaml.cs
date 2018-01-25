using Logic.ClientManagement.Et;
using Logic.TournamentManagement.Pers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Ranking_List.xaml
    /// Anzeigen der Rangliste zum auf der Vorseite ausgewaehlten Turnier
    /// </summary>
    public partial class Ranking_List : Page
    {
        public Ranking_List()
        {
            InitializeComponent();
            l_ranking.Content = "Rangliste " + ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name;
            //Ranking aus der Datenbank laden und mit Namen anreichern
            List<RankingPers> rankinglist =
                TMPersistenz.LoadRankings(ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name);
            dg_ranking_list.ItemsSource = rankinglist;
        }

        #region Header/Navigation

        //zurueck zur Startseite
        private void b_home_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
        }

        //zurueck zur Uebersichtsseite
        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            Tournament_Management tm = new Tournament_Management();
            this.NavigationService.Navigate(tm);
        }

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
    }
}