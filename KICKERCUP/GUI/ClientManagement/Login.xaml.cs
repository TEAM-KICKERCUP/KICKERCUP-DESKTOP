using Logic.ClientManagement.Impl;
using System;
using System.Windows;
using System.Windows.Controls;



namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// Login des registrierten Nutzers oder Weiterleitung zu Registrierung
    /// Ggf. andere Datenbankverbindung waehlbar
    /// </summary>
    public partial class Login : Page
    {
        //Verbindung zur Dantebank um Zugangsdaten zu checken und Sessionvariable mit dem aktuell angemeldeten Benutzer zu füllen
        ClientIMPL cdl = new ClientIMPL();

        public Login()
        {
            InitializeComponent();
        }

        //Weiterleitung auf Seite zum Registrieren eines neuen Benutzers
        private void b_sign_up_Click(object sender, RoutedEventArgs e)
        {
            Create_Account SignUpPage = new Create_Account();
            this.NavigationService.Navigate(SignUpPage);
        }

        private void b_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check ob Benutzername und Passwort in der Datenbank vorkommen und zusammen gehören.
                if (cdl.Login(tb_EnterUserName.Text, tb_EnterPassword.Password))
                {
                    //Session-Variable für den aktuell angemeldeten Benutzer
                    ApplicationState.SetValue("LoggedOnUser", cdl.FindClient(tb_EnterUserName.Text));

                    //Session-Variable, dass Datenbankeinstellungen vorgenommen wurden wird aktiviert. (Nach der ersten erfolgreichen Anmeldung kann die Datenbankverbindung nicht mehr geändert werden)
                    ApplicationState.SetValue("SetDBSettings", true);

                    //Weiterleitung zur Admin_Page
                    Home_Page ct = new Home_Page();
                    this.NavigationService.Navigate(ct);
                }
                else
                {
                    MessageBox.Show("Benutzername oder Password falsch.", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            //Damit das Program nicht abstürzt, falls mit der Datenbankverbindung etwas schief läuft, wird der mögliche Fehler hier gefangen.
            catch (Exception ex)
            {
                MessageBox.Show("Der Benutzername existiert nicht");
                String s = ex.Message;
            }
        }

        private void tb_EnterPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Wasserzeichen wird bei der ersten Eingabe entfernt
            pw_overlay.Visibility = Visibility.Hidden;
        }

        //Weiterleitung zu den Datenbankeinstellungen
        private void b_dbsettings_Click(object sender, RoutedEventArgs e)
        {
            //Check ob schon mal Datenbankeinstellungen vorgenommen wurden
            if (ApplicationState.GetValue<bool>("SetDBSettings") == true)
            {
                //Benutzer informieren
                MessageBox.Show(
                    "Die Datenbankverbindung kann nur vor der ersten Anmeldung nach Programmstart geändert werden. Starten Sie KICKERCUP neu um die Datenbankverbindung zu ändern.",
                    "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                DBSettings dbs = new DBSettings();
                this.NavigationService.Navigate(dbs);
            }
        }
    }
}