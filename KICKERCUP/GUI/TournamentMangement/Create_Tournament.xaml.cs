using Logic;
using Logic.ClientManagement.Et;
using Logic.TournamentManagement;
using Logic.TournamentManagement.Pers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Create_Tournament.xaml
    /// Erstellen eines Turniers (Basisdaten: Name, Spielmodus, Anzahl Saetze, die pro Match gespielt werden sollen,
    /// Anzahl Tore, die zum Sieg notwendig sind, ob Teilnehmer Skill Punkte sammeln sollen
    /// Diese Grunddaten werden in die Datenbank gespeichert
    /// Starten und Spielen des Turniers zur Laufzeit
    /// </summary>
    public partial class Create_Tournament : Page
    {
        public Create_Tournament()
        {
            InitializeComponent();

            //Ueberpruefen ob Nutzer noch eingeloggt
            if (String.IsNullOrEmpty(ApplicationState.GetValue<Client>("LoggedOnUser").Username))
            {
                Login login = new Login();
                this.NavigationService.Navigate(login);
            }
        }

        TournamentFactory tF = new TournamentFactory();

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

        #region Weiterleitung zur "Seite 2 - Auswahl Game Mode"

        //Weiterleitung zur "Seite 2 - Auswahl Game Mode"
        private void b_continue1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(tb_tournament_name.Text))
                {
                    //Ueberpruefen ob Turniername bereits vorhanden, d.h. ob die Find-Methode null zurueckgibt
                    if (TMPersistenz.FindTournament(tb_tournament_name.Text) == null)
                    {
                        tb_tournament_name.Visibility = Visibility.Hidden;
                        tbl_tournament_name.Visibility = Visibility.Hidden;
                        tbl_tournament_name_explanation.Visibility = Visibility.Hidden;
                        b_continue1.Visibility = Visibility.Hidden;
                        cb_game_mode.Visibility = Visibility.Visible;
                        b_continue2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Der Turniername ist bereits vergeben!", "KICKERCUP", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Bitte einen Turniernamen eingeben!", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler aufgetreten", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //Passende Erlaeuterung zum ausgewaehlten Game Mode anzeigen
        private void cb_game_mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_game_mode.SelectedIndex == 1)
            {
                tbl_explanation.Visibility = Visibility.Visible;
                tbl_explanation.Text =
                    "K.O.-System mit einer zweiten Chance. Erst bei der zweiten Niederlage wird das Team aus dem Turnier gekickt.";
            }

            if (cb_game_mode.SelectedIndex == 2)
            {
                tbl_explanation.Visibility = Visibility.Visible;
                tbl_explanation.Text =
                    "Ranglistenspiel bei dem zwei Spieler einzeln gegeneinander antreten. Es wird immer nur ein Satz gespielt und es sind 10 Tore nötig um zu gewinnen.";
                cb_sets.SelectedIndex = 1;
                cb_goals.SelectedIndex = 10;
                cb_ranked.SelectedIndex = 1;
            }

            if (cb_game_mode.SelectedIndex == 3)
            {
                tbl_explanation.Visibility = Visibility.Visible;
                tbl_explanation.Text =
                    "Ranglistenspiel bei dem zwei Teams einzeln gegeneinander antreten. Es wird immer nur ein Satz gespielt und es sind 10 Tore nötig um zu gewinnen.";
                cb_sets.SelectedIndex = 1;
                cb_goals.SelectedIndex = 10;
                cb_ranked.SelectedIndex = 1;
            }
        }

        #endregion

        #region Je nach Game Mode: Weiterleitung zur "Seite 3 - Anzahl Tore und Saetze" / Abspeichern Turnier

        //Weiterleitung zur "Seite 3 - Anzahl Tore und Saetze"
        private void b_continue2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cb_game_mode.SelectedIndex == 0)
                    MessageBox.Show("Bitte einen Turniermodus auswählen!", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                else if (cb_game_mode.SelectedIndex == 1)
                {
                    cb_game_mode.Visibility = Visibility.Hidden;
                    b_continue2.Visibility = Visibility.Hidden;
                    tbl_explanation.Visibility = Visibility.Hidden;
                    cb_goals.Visibility = Visibility.Visible;
                    tbl_goals.Visibility = Visibility.Visible;
                    tt_goals.Visibility = Visibility.Visible;
                    cb_sets.Visibility = Visibility.Visible;
                    tbl_sets.Visibility = Visibility.Visible;
                    tt_sets.Visibility = Visibility.Visible;
                    cb_ranked.Visibility = Visibility.Visible;
                    tbl_ranked.Visibility = Visibility.Visible;
                    tt_ranked.Visibility = Visibility.Visible;
                    b_add_competitors.Visibility = Visibility.Visible;
                }
                else
                {
                    save_tournament();
                }
            }
            catch
            {
                MessageBox.Show("Es ist ein unbekannter Fehler aufgetreten.", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion

        //Abspeichern der eingegebenen Daten
        private bool Ranked()
        {
            bool ranked = false;
            switch (cb_ranked.Text)
            {
                case "ja":
                    ranked = true;
                    break;
                case "nein":
                    ranked = false;
                    break;
                default:
                    break;
            }

            return ranked;
        }

        private void save_tournament()
        {
            try
            {
                if (cb_sets.SelectedIndex != 0 && cb_goals.SelectedIndex != 0 && cb_ranked.SelectedIndex != 0)
                {
                    TMPersistenz.AddTournament(tb_tournament_name.Text,
                        ApplicationState.GetValue<Client>("LoggedOnUser").Username, cb_game_mode.Text,
                        int.Parse(cb_sets.Text), int.Parse(cb_goals.Text), Ranked());
                    ApplicationState.SetValue("CurrentTournament",
                        TMPersistenz.FindTournament(tb_tournament_name.Text));
                    // MessageBox.Show("Das Turnier wurde erfolgreich erstellt. Jetzt Teilnehmer hinzufügen!", "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Information);

                    Tournament_Competitors tc = new Tournament_Competitors();
                    this.NavigationService.Navigate(tc);
                }
                else
                {
                    MessageBox.Show("Bitte Sätze pro Match und Tore pro Satz eingeben!", "KICKERCUP",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler aufgetreten", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void b_save_basic_tournament_Click(object sender, RoutedEventArgs e)
        {
            save_tournament();
        }

        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
        }
    }
}