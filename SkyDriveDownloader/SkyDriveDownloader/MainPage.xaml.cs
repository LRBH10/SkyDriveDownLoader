using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace SkyDriveDownloader
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page. La propriété Parameter
        /// est généralement utilisée pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        LiveAuthClient auth;

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View));
            /*
            try
            {
                auth = new LiveAuthClient();
                LiveLoginResult initializeResult = await auth.InitializeAsync();
                try
                {
                    LiveLoginResult loginResult = await auth.LoginAsync(new string[] { "wl.basic wl.skydrive" });
                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        LiveConnectClient connect = new LiveConnectClient(auth.Session);
                        LiveOperationResult operationResult = await connect.GetAsync("me/skydrive/files");
                        dynamic result = operationResult.Result;
                        if (result != null)
                        {
                            this.infoTextBlock.Text = string.Join(" ", "Hello", result.data[0].id, "!");
                        }
                        else
                        {
                            this.infoTextBlock.Text = "Error getting name.";
                        }
                    }
                }
                catch (LiveAuthException exception)
                {
                    this.infoTextBlock.Text = "Error signing in: " + exception.Message;
                }
                catch (LiveConnectException exception)
                {
                    this.infoTextBlock.Text = "Error calling API: " + exception.Message;
                }
            }
            catch (LiveAuthException exception)
            {
                this.infoTextBlock.Text = "Error initializing: " + exception.Message;
            }//*/
        }


    }
}
