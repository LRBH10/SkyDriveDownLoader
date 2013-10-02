using Microsoft.Live;
using SkyDriveDownloader2.Data;
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

// Pour en savoir plus sur le modèle d'élément Page Éléments groupés, consultez la page http://go.microsoft.com/fwlink/?LinkId=234231

namespace SkyDriveDownloader2
{
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class GroupedItemsPage : SkyDriveDownloader2.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
            Initialisation();
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="navigationParameter">Valeur de paramètre passée à
        /// <see cref="Frame.Navigate(Type, Object)"/> lors de la requête initiale de cette page.
        /// </param>
        /// <param name="pageState">Dictionnaire d'état conservé par cette page durant une session
        /// antérieure. Null lors de la première visite de la page.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: créez un modèle de données approprié pour le domaine posant problème pour remplacer les exemples de données
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Invoqué lorsqu'un utilisateur clique sur un en-tête de groupe.
        /// </summary>
        /// <param name="sender">Button utilisé en tant qu'en-tête pour le groupe sélectionné.</param>
        /// <param name="e">Données d'événement décrivant la façon dont le clic a été initié.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Déterminez le groupe représenté par l'instance Button
            var group = (sender as FrameworkElement).DataContext;

            // Accédez à la page de destination souhaitée, puis configurez la nouvelle page
            // en transmettant les informations requises en tant que paramètre de navigation.
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoqué lorsqu'un utilisateur clique sur un élément appartenant à un groupe.
        /// </summary>
        /// <param name="sender">GridView (ou ListView lorsque l'état d'affichage de l'application est Snapped)
        /// affichant l'élément sur lequel l'utilisateur a cliqué.</param>
        /// <param name="e">Données d'événement décrivant l'élément sur lequel l'utilisateur a cliqué.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Accédez à la page de destination souhaitée, puis configurez la nouvelle page
            // en transmettant les informations requises en tant que paramètre de navigation.
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }


        LiveAuthClient auth;

        private async void Initialisation()
        {
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
                            this.pageTitle.Text = string.Join(" ", "Hello", result.data[0].id, "!");
                        }
                        else
                        {
                            this.pageTitle.Text = "Error getting name.";
                        }
                    }
                }
                catch (LiveAuthException exception)
                {
                    this.pageTitle.Text = "Error signing in: " + exception.Message;
                }
                catch (LiveConnectException exception)
                {
                    this.pageTitle.Text = "Error calling API: " + exception.Message;
                }
            }
            catch (LiveAuthException exception)
            {
                this.pageTitle.Text = "Error initializing: " + exception.Message;
            }//*/
        }


    }
}
