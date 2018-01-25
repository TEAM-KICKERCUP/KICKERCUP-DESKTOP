using Logic.ClientManagement.Et;
using Logic.CompetitorManagement.Impl;
using System;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Create_Competitor.xaml
    /// Fenster zum Anlegen eines neuen Teilnehmers
    /// </summary>
    public partial class Create_Competitor : Window
    {
        //fuer Verbindung zum Main Window, um das Schließen dieses Windows an das MainWindow zu kommunizieren
        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

        public Create_Competitor()
        {
            InitializeComponent();
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

        //Abspeichern Teilnehmer
        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                int skilllevel = 1500; //Startlevel festgesetzt
                comp.AddCompetitor(tb_name.Text, tb_surname.Text, cb_gender.Text, skilllevel, cb_visibility.Text,
                    ApplicationState.GetValue<Client>("LoggedOnUser").Username);
                DataChanged?.Invoke(this, new EventArgs());
                Close();
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!", "KICKERCUP", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}