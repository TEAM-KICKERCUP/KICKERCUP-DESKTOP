using Logic.ClientManagement.Et;
using Logic.TournamentManagement.Pers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Tournament_Management.xaml
    /// Liste mit allen Turnieren des angemeldeten Nutzers
    /// Weiterleitung zu: Turnier fortsetzen und Rangliste des ausgewaehlten Turniers
    /// </summary>
    public partial class Tournament_Management : Page
    {
        public Tournament_Management()
        {
            InitializeComponent();
            List<TournamentPers> allTournaments =
                TMPersistenz.GetAllTournaments(ApplicationState.GetValue<Client>("LoggedOnUser").Username);
            dg_tournament_list.ItemsSource = allTournaments;
        }

        private void dg_tournament_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dg_tournament_list.SelectedItem != null)
            {
                String tournamentName = (dg_tournament_list.SelectedCells[0].Column.GetCellContent(dg_tournament_list.SelectedItem) as TextBlock).Text;
                ApplicationState.SetValue("CurrentTournament", TMPersistenz.FindTournament(tournamentName));
                l_current_tournament.Content = tournamentName;
            }

        }

        //noch nicht gespieltes Turnier fortsetzen
        private void b_continue_tournament_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ApplicationState.GetValue<TournamentPers>("CurrentTournament").IsFinished == true)
                {
                    MessageBox.Show("Das ausgewählte Turnier wurde bereits beendet!", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    Tournament_Competitors tc = new Tournament_Competitors();
                    this.NavigationService.Navigate(tc);
                }
            }
            catch
            {
                MessageBox.Show("Bitte zuerst ein Turnier auswählen!", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //Weiterleitung Seite Rangliste
        private void b_ranking_list_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ApplicationState.GetValue<TournamentPers>("CurrentTournament").IsFinished == false)
                {
                    MessageBox.Show("Das ausgewählte Turnier wurde noch nicht gespielt. Es ist somit keine Rangliste vorhanden", "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Ranking_List rl = new Ranking_List();
                    this.NavigationService.Navigate(rl);
                }
            }
            catch
            {
                MessageBox.Show("Bitte zuerst ein Turnier auswählen!", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #region Header/Navigation

        //Zurueck zur Startseite nach dem Einloggen
        private void b_home_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
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

        private void b_delete_tournament_Click(object sender, RoutedEventArgs e)
        {

            if (ApplicationState.GetValue<TournamentPers>("CurrentTournament") != null)
            {
                TMPersistenz.DeleteTournamentInDB(ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name);
                dg_tournament_list.ItemsSource = null;
                ApplicationState.SetValue("CurrentTournament", null);
                l_current_tournament.Content = "";
                List<TournamentPers> allTournaments =
                    TMPersistenz.GetAllTournaments(ApplicationState.GetValue<Client>("LoggedOnUser").Username);
                dg_tournament_list.ItemsSource = allTournaments;
            }
            else
            {
                MessageBox.Show("Bitte zuerst ein Turnier auswählen!", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}