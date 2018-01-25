using Logic;
using Logic.ClientManagement.Et;
using Logic.CompetitorManagement.Et;
using Logic.CompetitorManagement.Impl;
using Logic.TournamentManagement.Pers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Tournament_Competitors.xaml
    /// Hinzufuegen von Teilnehmern zu einem Turnier (zur Laufzeit)
    /// Ggf. Bearbeiten bestehender Teilnehmer oder anlegen neuer Teilnehmer
    /// </summary>
    public partial class Tournament_Competitors : Page
    {
        public Tournament_Competitors()
        {
            InitializeComponent();
            competitor_search();
            l_current_tournament.Content = ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name;
        }

        //Verbindung zur DB
        CompetitorIMPL comp = new CompetitorIMPL();

        //Teilnehmersuche über Suchzeile
        private void competitor_search()
        {
            try
            {
                IDictionary<Guid, Competitor> competitors = new SortedList<Guid, Competitor>();
                competitors = comp.LoadCompetitors(tb_competitor_search.Text,
                    ApplicationState.GetValue<Client>("LoggedOnUser").Username);
                dg_competitor_list.ItemsSource = null;
                dg_competitor_list.ItemsSource = competitors;
            }

            catch
            {
                MessageBox.Show("Es ist ein unbekannter Fehler aufgetreten. Nochmal versuchen", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Suche starten ueber Enter oder Klick auf den Button
        private void b_search_Click(object sender, RoutedEventArgs e)
        {
            competitor_search();
        }

        private void tb_competitor_search_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                competitor_search();
        }

        //Application State auf ausgewaehlten Teilnehmer setzen
        private void dg_competitor_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_competitor_list.SelectedItem != null)
            {
                object item = dg_competitor_list.SelectedItem;
                string ID = (dg_competitor_list.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                ApplicationState.SetValue("SelectedCompetitor", comp.FindCompetitor(Guid.Parse(ID)));
            }
        }

        //Teilnehmerliste des Turniers
        IDictionary<Guid, Competitor> tournament_competitors = new SortedList<Guid, Competitor>();

        //Teilnehmer aus kompletter Liste (linkes DataGrid) in Turnierliste (rechtes DataGrid) einfuegen
        private void b_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dg_competitor_list.SelectedCells.Count > 0)
                {
                    object item = dg_competitor_list.SelectedItem;
                    Guid compID =
                        Guid.Parse((dg_competitor_list.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    Competitor competitor = comp.FindCompetitor(compID);
                    tournament_competitors.Add(compID, competitor);
                    dg_current_competitor_list.ItemsSource = null;
                    dg_current_competitor_list.ItemsSource = tournament_competitors;
                }
                else
                {
                    MessageBox.Show("Bitte Teilnehmer, der hinzugefügt werden soll, auswählen", "KICKERCUP",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Der Teilnehmer wurde dem Turnier bereits hinzugefügt", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Teilnehmer aus Turnierliste (rechtes DataGrid) entfernen
        private void b_remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dg_current_competitor_list.SelectedCells.Count > 0)
                {
                    object item = dg_current_competitor_list.SelectedItem;
                    Guid compID =
                        Guid.Parse(
                            (dg_current_competitor_list.SelectedCells[0].Column.GetCellContent(item) as TextBlock)
                            .Text);
                    tournament_competitors.Remove(compID);
                    dg_current_competitor_list.ItemsSource = null;
                    dg_current_competitor_list.ItemsSource = tournament_competitors;
                }
                else
                {
                    MessageBox.Show("Bitte Teilnehmer, der entfernt werden soll, auswählen", "KICKERCUP",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Fehler! Der Teilnehmer konnte nicht entfernt werden", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //Speichern der Aenderungen
        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            List<ICompetitor> teilnehmerListe = new List<ICompetitor>();
            foreach (KeyValuePair<Guid, Competitor> c in tournament_competitors)
            {
                teilnehmerListe.Add(c.Value);
            }

            try
            {
                ITournament tournament =
                    TMPersistenz.GetTournamentFromDB(
                        ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name, teilnehmerListe);
                //Startseite Turnier oeffnen, Turnier ueber Konstruktor der Seite weitergegeben
                Input_Results ir = new Input_Results(tournament);
                this.NavigationService.Navigate(ir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Zur Admin_Page zurueckkehren
        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Home_Page hp = new Home_Page();
            this.NavigationService.Navigate(hp);
        }

        //Weiterleitung Seite: Teilnehmer erstellen
        private void b_new_comp_Click(object sender, RoutedEventArgs e)
        {
            Create_Competitor cc = new Create_Competitor();
            cc.DataChanged += Create_Competitor_DataChanged;
            cc.Show();
        }

        //Verbindung zum Child Window "Teilnehmer erstellen", triggert das Aktualisieren der Teilnehmerliste
        private void Create_Competitor_DataChanged(object sender, EventArgs e)
        {
            competitor_search();
        }

        //Fensteraufruf Teilnehmer bearbeiten
        private void b_edit_comp_Click(object sender, RoutedEventArgs e)
        {
            if (dg_competitor_list.SelectedCells.Count > 0)
            {
                Edit_Competitor ec = new Edit_Competitor();
                ec.DataChanged += Edit_Competitor_DataChanged;
                ec.Show();
            }
            else
            {
                MessageBox.Show("Bitte einen Teilnehmer zum Bearbeiten auswählen", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //Verbindung zum Child Window "Teilnehmer bearbeiten", triggert das Aktualisieren der Teilnehmerliste
        private void Edit_Competitor_DataChanged(object sender, EventArgs e)
        {
            competitor_search();
        }
    }
}