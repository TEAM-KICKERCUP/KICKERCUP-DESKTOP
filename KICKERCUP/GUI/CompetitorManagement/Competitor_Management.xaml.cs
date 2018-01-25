using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Logic.CompetitorManagement.Impl;
using Logic.CompetitorManagement.Et;
using Logic.ClientManagement.Et;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Competitor_Management.xaml
    /// Anlegen neuer Teilnehmer fuer Turniere oder Bearbeiten der Daten eines bestehenden Teilnehmers
    /// </summary>
    public partial class Competitor_Management : Page
    {
        CompetitorIMPL comp = new CompetitorIMPL();

        public Competitor_Management()
        {
            InitializeComponent();
            competitor_search();
        }

        //Ueberpruefen ob alle Felder ausgefuellt wurden
        public bool CheckFields()
        {
            if (String.IsNullOrEmpty(tb_name.Text))
                return false;
            else if (String.IsNullOrEmpty(tb_surname.Text))
                return false;
            else if (cb_gender.SelectedIndex == 0)
                return false;
            else if (cb_visibility.SelectedIndex == 0)
                return false;
            else if (String.IsNullOrEmpty(ApplicationState.GetValue<Client>("LoggedOnUser").Username))
                return false;
            else
                return true;
        }

        // setzt alle Felder in den urspruenglichen Zustand zurueck
        private void EmptyAll()
        {
            tb_name.Clear();
            tb_surname.Clear();
            cb_gender.SelectedIndex = 0;
            cb_visibility.SelectedIndex = 0;
            l_compID.Content = "";
            tb_competitor_search.Clear();
            dg_competitor_list.ItemsSource = null;
        }

        //Aktion abbrechen und zur Admin-Seite zurueckkehren
        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Admin_Page Administration = new Admin_Page();
            this.NavigationService.Navigate(Administration);
        }

        //Teilnehmersuche über Suchzeile
        private void competitor_search()
        {
            IDictionary<Guid, Competitor> competitors = new SortedList<Guid, Competitor>();
            try
            {
                competitors = comp.LoadCompetitors(tb_competitor_search.Text,
                    ApplicationState.GetValue<Client>("LoggedOnUser").Username);
                dg_competitor_list.ItemsSource = competitors;
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler bei der Suche in der Datenbank aufgetreten", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void b_search_Click(object sender, RoutedEventArgs e)
        {
            competitor_search();
        }

        private void tb_competitor_search_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                competitor_search();
        }

        //Anzeigen des ausgewaehlten Teilnehmers in den Feldern auf der rechten Seite
        private void dg_competitor_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dg_competitor_list.SelectedItem != null)
                {
                    object item = dg_competitor_list.SelectedItem;
                    string ID = (dg_competitor_list.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                    Guid compID = Guid.Parse(ID);
                    Competitor competitor = comp.FindCompetitor(compID);

                    l_compID.Content = ID;
                    tb_name.Text = competitor.Name;
                    tb_surname.Text = competitor.Surname;

                    string gender = competitor.Gender;
                    if (gender == "männlich")
                        cb_gender.SelectedIndex = 1;
                    if (gender == "weiblich")
                        cb_gender.SelectedIndex = 2;

                    string visibility = competitor.Visibility;
                    if (visibility == "global")
                        cb_visibility.SelectedIndex = 1;
                    if (visibility == "lokal")
                        cb_visibility.SelectedIndex = 2;
                }
            }
            catch
            {
                MessageBox.Show(
                    "Es ist ein Fehler aufgetreten. Sie können diesen Teilnehmer im Moment nicht bearbeiten",
                    "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Ausgewaehlten Teilnehmer aus der Datenbank loeschen
        private void b_delete_one_comp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (l_compID.Content.ToString() != "")
                {
                    Guid compID = Guid.Parse(l_compID.Content.ToString());
                    comp.DeleteCompetitor(compID);
                }

                MessageBox.Show("Der Teilnehmer wurde erfolgreich gelöscht", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                EmptyAll();
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler aufgetreten", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            competitor_search();
        }

        //Alle Teilnehmer aus der Datenbank loeschen
        private void b_delete_all_comp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                comp.DeleteAllCompetitors();
                MessageBox.Show("Alle Teilnehmer wurden erfolgreich gelöscht", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                EmptyAll();
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler aufgetreten. Die Teilnehmer wurden nicht gelöscht", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Nach Eingabe aller notwendigen Daten, speichern eines neuen Teilnehmers
        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.CheckFields())
                {
                    //falls Teilnehmer noch keine ID hat, also nicht über die Suche aufgerufen wurde, neuen anlegen
                    if (l_compID.Content.ToString() == "")
                    {
                        comp.AddCompetitor(tb_name.Text, tb_surname.Text, cb_gender.Text, 1500, cb_visibility.Text,
                            ApplicationState.GetValue<Client>("LoggedOnUser").Username);
                    }

                    //ansonsten Teilnehmer anhand der ID updaten
                    else
                    {
                        Guid compID = Guid.Parse(l_compID.Content.ToString());
                        comp.UpdateCompetitor(compID, tb_name.Text, tb_surname.Text, cb_gender.Text,
                            comp.FindCompetitor(compID).SkillLevel, cb_visibility.Text);
                    }

                    EmptyAll();
                }

                else
                {
                    MessageBox.Show("Es wurden nicht alle Felder ausgefüllt", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler aufgetreten", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            competitor_search();
        }

        //Zuruecksetzen der Eingabefelder zum Anlegen eines neuen Teilnehmers
        private void b_new_Click(object sender, RoutedEventArgs e)
        {
            EmptyAll();
        }
    }
}