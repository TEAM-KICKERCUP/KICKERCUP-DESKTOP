using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Logic.ClientManagement.Impl;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// Registrierung eines neuen Nutzers
    /// </summary>
    public partial class Create_Account : Page
    {
        //Datenbankverbindung
        ClientIMPL cdl = new ClientIMPL();

        public Create_Account()
        {
            InitializeComponent();
        }

        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Korrekte Eingabe prüfen
                if (this.CheckFields())
                {
                    //Neuen Benutzer der Datenbank hinzufügen
                    cdl.AddClient(tb_username.Text, pw_password.Password, tb_name.Text, tb_lastname.Text, tb_email.Text,
                        cb_gender.Text);
                    //Session-Variable, dass DB Einstellungen vorgenommen wurden wird gesetzt.
                    ApplicationState.SetValue("SetDBSettings", true);
                    //Benutzer auf Login Seite umleiten
                    Login NewLoginPage = new Login();
                    this.NavigationService.Navigate(NewLoginPage);
                }

                else
                    MessageBox.Show("Es wurden nicht alle Felder ausgefüllt", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Leider ist ein Fehler aufgetreten. (" + ex.Message + ")", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Weiterleitung auf Seite fuer Login
        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Login NewLoginPage = new Login();
            this.NavigationService.Navigate(NewLoginPage);
        }

        private void cb_gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Foreground auf Schwarz (von Grau) setzen sobald sich die Selektion ändert.
            cb_gender.Foreground = new SolidColorBrush(Colors.Black);
        }

        //Passwort overlay bei erster Eingabe deaktivieren
        private void Pw_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pw_overlay.Visibility = Visibility.Hidden;
        }


        //Check der Eingabefelder auf Null oder leere Strings
        public bool CheckFields()
        {
            if (String.IsNullOrEmpty(tb_username.Text))
                return false;
            else if (String.IsNullOrEmpty(pw_password.Password))
                return false;
            else if (String.IsNullOrEmpty(tb_name.Text))
                return false;
            else if (String.IsNullOrEmpty(tb_lastname.Text))
                return false;
            else if (String.IsNullOrEmpty(tb_email.Text))
                return false;
            else if (cb_gender.Text == "Wähle dein Geschlecht")
                return false;
            else
                return true;
        }
    }
}