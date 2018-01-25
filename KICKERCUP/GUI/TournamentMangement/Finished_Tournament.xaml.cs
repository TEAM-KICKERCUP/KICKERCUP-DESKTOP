using Logic.TournamentManagement;
using Logic.TournamentManagement.Pers;
using System;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Finished_Tournament.xaml
    /// Fenster zum Abschluss des Turniers + Verkündung Gewinner
    /// </summary>
    public partial class Finished_Tournament : Window
    {
        public Finished_Tournament()
        {
            InitializeComponent();
            l_current_tournament.Content = ApplicationState.GetValue<TournamentPers>("CurrentTournament").Name;
            l_winner_team.Content = ApplicationState.GetValue<Team>("MatchWinner").ToString();
        }

        //fuer Verbindung zum Main Window, um das Schließen dieses Windows an das MainWindow zu kommunizieren
        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

        private void b_Ok_Click(object sender, RoutedEventArgs e)
        {
            DataChanged?.Invoke(this, new EventArgs());
            Close();
        }
    }
}