using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Logic.ClientManagement.Et;
using Logic.ClientManagement.Impl;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// Bearbeiten des Accounts des angemeldeten Nutzers
    /// </summary>
    public partial class Edit_Account : Page
    {
        //Datenbankverbindung
        ClientIMPL cdl = new ClientIMPL();

        public Edit_Account()
        {
            InitializeComponent();

            //Username des angemeldeten Benutzers in der TextBox einfrieren (Ist Primary Key, darf in der DB sowieso nicht verändert werden)
            tb_username.Text = ApplicationState.GetValue<Client>("LoggedOnUser").Username;
            tb_username.IsEnabled = false;
            tb_username.Background = new SolidColorBrush(Colors.LightGray);

            //Alle anderen Properties des angemeldeten Benutzers aus der Session-Variable in die TextBoxen laden
            tb_lastname.Text = ApplicationState.GetValue<Client>("LoggedOnUser").Surname;
            tb_name.Text = ApplicationState.GetValue<Client>("LoggedOnUser").Name;
            tb_email.Text = ApplicationState.GetValue<Client>("LoggedOnUser").EMail;
            pw_password.Password = "placeholder";

            //Geschlecht des angemeldeten Benutzers laden
            if (ApplicationState.GetValue<Client>("LoggedOnUser").Gender == "männlich")
            {
                cb_gender.SelectedValue = m;
            }

            if (ApplicationState.GetValue<Client>("LoggedOnUser").Gender == "weiblich")
            {
                cb_gender.SelectedValue = w;
            }
        }

        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check ob die Eingabe okay ist
                if (this.CheckFields())
                {
                    //Benutzer in der Datenbank aktualisieren
                    cdl.UpdateClient(tb_username.Text, pw_password.Password, tb_name.Text, tb_lastname.Text,
                        tb_email.Text, cb_gender.Text);
                    MessageBox.Show("Ihre Benutzereinstellungen wurden erfolgreich gespeichert.", "KICKERCUP",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    //Session-Variable für den aktuell angemeldeten Benutzer aktualisieren
                    ApplicationState.SetValue("LoggedOnUser", cdl.FindClient(tb_username.Text));

                    Admin_Page ap = new Admin_Page();
                    this.NavigationService.Navigate(ap);
                }

                else
                    MessageBox.Show("Es wurden nicht alle Felder ausgefüllt", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
            //Datenbankverbindung ist "immer" mit Risiko behaftet, daher wird hier ein möglicher Fehler gefangen um Programmabsturz zu verhindern
            catch (Exception ex)
            {
                MessageBox.Show("Leider ist ein Fehler aufgetreten. (" + ex.Message + ")", "KICKERCUP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Weiterleitung auf Admin Page
        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            Admin_Page ap = new Admin_Page();
            this.NavigationService.Navigate(ap);
        }

        private void cb_gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Combobox Schwarz (von Grau) einfärben sobald eine Änderung vorgenommen wird
            cb_gender.Foreground = new SolidColorBrush(Colors.Black);
        }


        private void Pw_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Wasserzeichen nach erster Änderung deaktivieren
            pw_overlay.Visibility = Visibility.Hidden;
        }

        //Eingabe der Felder auf Null oder leere Strings prüfen
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