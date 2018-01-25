using System;
using System.Windows;
using System.Windows.Controls;
using Logic.ClientManagement.Et;
using Logic.EMail;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für Feedback.xaml
    /// Feedback per E-Mail senden
    /// Zum Testen muss die eigene E-Mail angegeben werden, da sonst der Tester die Funktionalität nicht ueberpruefen kann
    /// </summary>
    public partial class Feedback : Page
    {
        public Feedback()
        {
            InitializeComponent();
            try
            {
                //Sorgt dafür, dass Tooltips nicht verschwinden
                ToolTipService.ShowDurationProperty.OverrideMetadata(
                    typeof(DependencyObject), new FrameworkPropertyMetadata(30000000));
            }
            catch
            {
                //Nichts zu tun ist Empfhleung von Stackoverflow Code Guru https://stackoverflow.com/questions/896574/forcing-a-wpf-tooltip-to-stay-on-the-screen
            }
        }

        private void b_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txt_Address.Text))
                {
                    MessageBox.Show(
                        "Das Feedback wird normalerweise automatisch an das Entwicklungsteam von KICKERCUP gesendet. Damit das Feature getestet werden kann, können Sie hier Ihre eigene E-Mail Adresse hinterlegen.",
                        "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //Prüfung ob es sich um eine E-Mail Adresse handelt
                else if (txt_Address != null || !txt_Address.Text.Contains("@"))
                {
                    //E-Mail erstellen
                    EMail mail = new EMail();
                    //E-Mail senden (Vorname + Nachname) des angemeldeten Benutzers im Betreff
                    mail.SendMail(
                        "KICKERCUP Feedback von " + ApplicationState.GetValue<Client>("LoggedOnUser").GetFullName(),
                        txt_body.Text.Trim(), txt_Address.Text.Trim());

                    // Benutzer über erfolgte Sendung informieren
                    MessageBox.Show("Vielen Dank für Ihr Feedback.", "KICKERCUP", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    //Weiterleitung auf vorherige Page
                    this.NavigationService.GoBack();
                }
            } //Fehler des E-Mail Modules werden gefangen und ausgegeben
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "KICKERCUP", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Weiterleitung auf Admin Page
        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}