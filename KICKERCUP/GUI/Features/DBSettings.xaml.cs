using Logic.Database;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace GUI
{
    /// <summary>
    /// Aendern der Datenbank, die geladen werden soll
    /// Ggf. Anlegen einer eigenen DB-Verbindung
    /// </summary>
    public partial class DBSettings : Page
    {
        public DBSettings()
        {
            InitializeComponent();

            //Textfeld und speichern button deaktivieren und auf Grau schalten
            tb_db_constr.IsEnabled = false;
            b_save.IsEnabled = false;

            tb_db_constr.Background = new SolidColorBrush(Colors.LightGray);
            b_save.Background = new SolidColorBrush(Colors.LightGray);
        }

        //Falls der User eine eigene Connection angeben möchte werden die entsprechenden Kontrollelemente freigeschalten
        private void b_db_use_constr_Click(object sender, RoutedEventArgs e)
        {
            tb_db_constr.IsEnabled = true;
            b_save.IsEnabled = true;
            tb_db_constr.Background = new SolidColorBrush(Colors.White);
            b_save.Background = new SolidColorBrush(Color.FromRgb(9, 131, 63));

            //Session-Variable, dass DB Einstellungen vorgenommen wurden, wird gesetzt.
            ApplicationState.SetValue("SetDBSettings", true);
        }


        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            //Connection String aus TextBox wird in Session-Variable gespeichert
            ConnectionTool.SetValue("DBConnectionString", tb_db_constr.Text);
            //Benutzer wird über Erfolg informier
            MessageBox.Show("Die Datenbankeinstellungen wurden erfolgreich gespeichert", "KICKERCUP",
                MessageBoxButton.OK, MessageBoxImage.Information);

            //Weiterleitung auf Seite fuer Login
            Login l = new Login();
            this.NavigationService.Navigate(l);
        }

        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.NavigationService.Navigate(l);
        }
    }
}