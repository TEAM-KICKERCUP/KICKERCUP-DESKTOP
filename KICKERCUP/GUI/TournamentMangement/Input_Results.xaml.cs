using Logic;
using Logic.ClientManagement.Et;
using Logic.TournamentManagement;
using Logic.TournamentManagement.Pers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Input_Results.xaml
    /// Eingabe der Ergebnisse der Saetze. 
    /// Aktuelle Teams, die gegeneinander antreten, werden angezeigt + laufend aktualisierter Gewinnchancen
    /// Bei Abschluss aller Matches Weiterleitung in Gewinner-Fenster
    /// </summary>
    public partial class Input_Results : Page
    {
        ITournament tournament;
        List<Team> teams;

        public Input_Results(ITournament tournament)
        {
            InitializeComponent();
            this.tournament = tournament;
        }

        #region Turnier starten

        //wenn der Nutzer auf Turnier starten klickt, werden die Elemente fuer das Eintragen der Ergebnisse eingeblendet
        private void b_start_tournament_Click(object sender, RoutedEventArgs e)
        {
            b_start_tournament.Visibility = Visibility.Hidden;
            b_start.Visibility = Visibility.Hidden;
            b_stop.Visibility = Visibility.Hidden;
            l_title.Visibility = Visibility.Visible;
            i_pitch.Visibility = Visibility.Visible;
            tbl_team1.Visibility = Visibility.Visible;
            tbl_team2.Visibility = Visibility.Visible;
            tbl_nr_set.Visibility = Visibility.Visible;
            tb_team1.Visibility = Visibility.Visible;
            tb_team2.Visibility = Visibility.Visible;
            b_continue.Visibility = Visibility.Visible;
            b_home.Visibility = Visibility.Visible;
            tbl_chance_team1.Visibility = Visibility.Visible;
            tbl_chance_team2.Visibility = Visibility.Visible;

            try
            {
                tournament.StartTournament();
                teams = tournament.CurrentMatch.GetTeams();
                tbl_team1.Text = teams.ElementAt(0).ToString();
                tbl_team2.Text = teams.ElementAt(1).ToString();
                tbl_chance_team1.Text = "Gewinnchance: \n" +
                                        (tournament.CurrentMatch.GetWinExpectationThatTeamAWins()).ToString() + " %";
                tbl_chance_team2.Text = "Gewinnchance: \n" +
                                        (tournament.CurrentMatch.GetWinExpectationThatTeamBWins()).ToString() + " %";
            }
            catch (Exception ex)
            {
                if (ex is ArgumentOutOfRangeException)
                {
                    MessageBox.Show(
                        "Teilnehmeranzahl ist nicht korrekt für den ausgewählten Spielmodus " +
                        ApplicationState.GetValue<TournamentPers>("CurrentTournament").Gamemode + ".", "KICKERCUP",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Es ist ein schwerer Fehler aufgetreten");

                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
                else
                {
                    Tournament_Competitors tc = new Tournament_Competitors();
                    this.NavigationService.Navigate(tc);
                }
            }
        }

        #endregion

        #region Saetze Ergebnisse eintragen

        int countSet = 1;
        int countMatch = 1;

        private void b_continue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!tournament.IsFinished)
                {
                    tbl_chance_team1.Text = "Gewinnchance: \n" +
                                            (tournament.CurrentMatch.GetWinExpectationThatTeamAWins()).ToString() +
                                            " %";
                    tbl_chance_team2.Text = "Gewinnchance: \n" +
                                            (tournament.CurrentMatch.GetWinExpectationThatTeamBWins()).ToString() +
                                            " %";
                    tournament.SetGoalForCurrentSet(teams.ElementAt(0), Int32.Parse(tb_team1.Text), teams.ElementAt(1),
                        Int32.Parse(tb_team2.Text));
                    tb_team1.Clear();
                    tb_team2.Clear();

                    if (countSet == ApplicationState.GetValue<TournamentPers>("CurrentTournament").AmountSets)
                    {
                        countMatch++;
                        l_title.Content = "Spiel " + countMatch.ToString();
                        countSet = 0;
                    }

                    teams = tournament.CurrentMatch.GetTeams();

                    if (!String.IsNullOrEmpty(tbl_team1.Text) && !String.IsNullOrEmpty(tbl_team2.Text))
                    {
                        tbl_team1.Text = teams.ElementAt(0).ToString();
                        tbl_team2.Text = teams.ElementAt(1).ToString();

                        countSet++;
                        tbl_nr_set.Text = "Satz: " + countSet.ToString();
                        ApplicationState.SetValue("MatchWinner", tournament.CurrentMatch.Winner);
                    }
                    else
                    {
                        MessageBox.Show("Bitte Tore eintragen", "KICKERCUP", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }

                if (tournament.IsFinished)
                {
                    TMPersistenz.SaveFinishedTournamendToDB(tournament,
                        ApplicationState.GetValue<Client>("LoggedOnUser").Username);

                    if (tournament.IsRanked)
                    {
                        //Rankings des Turniers in der Datenbank abspeichern
                        TMPersistenz.SaveRankingsToDB(tournament);
                    }

                    Finished_Tournament ft = new Finished_Tournament();
                    ft.DataChanged += Finished_Tournament_DataChanged;
                    ft.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void Finished_Tournament_DataChanged(object sender, EventArgs e)
        {
            Tournament_Management tm = new Tournament_Management();
            this.NavigationService.Navigate(tm);
        }

        #region Header/Navigation

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

        //zurueck zur Startseite
        private void b_home_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
        }

        #endregion
    }
}