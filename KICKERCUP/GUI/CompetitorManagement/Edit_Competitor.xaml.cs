using Logic.CompetitorManagement.Et;
using Logic.CompetitorManagement.Impl;
using System;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Edit_Competitor.xaml
    /// Fenster zum Bearbeiten eines bestehenden Teilnehmers
    /// </summary>
    public partial class Edit_Competitor : Window
    {
        //fuer Verbindung zum Main Window, um das Schließen dieses Windows an das MainWindow zu kommunizieren
        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

        //Felder werden anhand des ausgewahlten Teilnehmers befuellt und koennen veraendert werden
        public Edit_Competitor()
        {
            InitializeComponent();
            l_current_comp.Content = ApplicationState.GetValue<Competitor>("SelectedCompetitor").Name + " " +
                                     ApplicationState.GetValue<Competitor>("SelectedCompetitor").Surname;
            tb_name.Text = ApplicationState.GetValue<Competitor>("SelectedCompetitor").Name;
            tb_surname.Text = ApplicationState.GetValue<Competitor>("SelectedCompetitor").Surname;

            switch (ApplicationState.GetValue<Competitor>("SelectedCompetitor").Gender)
            {
                case "männlich":
                    cb_gender.SelectedIndex = 1;
                    break;
                case "weiblich":
                    cb_gender.SelectedIndex = 2;
                    break;
                default:
                    break;
            }

            switch (ApplicationState.GetValue<Competitor>("SelectedCompetitor").Visibility)
            {
                case "global":
                    cb_visibility.SelectedIndex = 1;
                    break;
                case "lokal":
                    cb_visibility.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
        }

        //Verbindung zur DB
        CompetitorIMPL comp = new CompetitorIMPL();

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
            else
                return true;
        }

        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckFields())
                {
                    //ID und Skill Level koennen nicht durch den Benutzer bearbeitet werden
                    Guid ID = ApplicationState.GetValue<Competitor>("SelectedCompetitor").CompetitorID;
                    int skill = ApplicationState.GetValue<Competitor>("SelectedCompetitor").SkillLevel;

                    comp.UpdateCompetitor(ID, tb_name.Text, tb_surname.Text, cb_gender.Text, skill, cb_visibility.Text);
                    DataChanged?.Invoke(this, new EventArgs());
                    Close();
                }
                else
                {
                    MessageBox.Show("Bitte alle Felder ausfüllen!", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Fehler beim Speichern der Änderungen!", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}