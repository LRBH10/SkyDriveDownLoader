using Microsoft.Live;
using SkyDriveDownloader2.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
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

    public struct LiveConfig
    {
        public static LiveAuthClient Auth;
        public static LiveConnectClient ConnectClient;
        public static LiveLoginResult LoginResult;
    }
    /// <summary>
    /// Page affichant une collection groupée d'éléments.
    /// </summary>
    public sealed partial class GroupedItemsPage : SkyDriveDownloader2.Common.LayoutAwarePage
    {

        static bool first_run = true;

        public FileDetails Current;

        public GroupedItemsPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += DisplayPrivacyPolicy;
            if (first_run)
            {
                Initialisation();
                new Icons();
                
                first_run = false;
            }
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
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: créez un modèle de données approprié pour le domaine posant problème pour remplacer les exemples de données
            if (navigationParameter is String)
            {
                var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
                this.DefaultViewModel["Groups"] = sampleDataGroups;
            }
            else if (navigationParameter is FileDetails)
            {
                FileDetails casted = (FileDetails)navigationParameter;
                SampleDataSource.GetInstance.AllGroups.Clear();

                Current = casted;
                
                await Files.GetDirectoryView(casted);

                var sampleDataGroups = SampleDataSource.GetGroups(casted);
                this.DefaultViewModel["Groups"] = sampleDataGroups;
            }
        }

        /// <summary>
        /// Invoqué lorsqu'un utilisateur clique sur un en-tête de groupe.
        /// </summary>
        /// <param name="sender">Button utilisé en tant qu'en-tête pour le groupe sélectionné.</param>
        /// <param name="e">Données d'événement décrivant la façon dont le clic a été initié.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Déterminez le groupe représenté par l'instance Button
            //var group = (sender as FrameworkElement).DataContext;

            // Accédez à la page de destination souhaitée, puis configurez la nouvelle page
            // en transmettant les informations requises en tant que paramètre de navigation.
            //this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
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
            var itemId = (SampleDataItem)e.ClickedItem;

            if (itemId.Data.type == "folder" || itemId.Data.type == "album")
            {
                Frame.Navigate(typeof(GroupedItemsPage), itemId.Data);
            }
            else
            {
                DownloadFile_Click(itemId.Data);
                //this.Frame.Navigate(typeof(ItemDetailPage), itemId.UniqueId);
            }
        }

        private async void Initialisation()
        {
            try
            {
                LiveConfig.Auth = new LiveAuthClient();
                LiveLoginResult initializeResult = await LiveConfig.Auth.InitializeAsync();
                try
                {
                    LiveConfig.LoginResult = await LiveConfig.Auth.LoginAsync(new string[] { "wl.basic wl.skydrive" });
                    if (LiveConfig.LoginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        LiveConfig.ConnectClient = new LiveConnectClient(LiveConfig.Auth.Session);
                        LiveOperationResult operationResult = await LiveConfig.ConnectClient.GetAsync("me/skydrive");
                        dynamic result = operationResult.RawResult;
                        if (result != null)
                        {
                            FileDetails re = FileDetails.GetResponseApiFrom(result);
                            await Files.GetDirectoryView(re);
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

        private async void GoBackUser(object sender, RoutedEventArgs e)
        {
            if (Current.parent_id == null)
            {
                return;
            }

            LiveOperationResult operationResult = await LiveConfig.ConnectClient.GetAsync(Current.parent_id);
            dynamic result = operationResult.RawResult;
            if (result != null)
            {
                FileDetails re = FileDetails.GetResponseApiFrom(result);
                Frame.Navigate(typeof(GroupedItemsPage), re);
            }
        }

        private System.Threading.CancellationTokenSource ctsDownload;

        private async void DownloadFile_Click(FileDetails clicked)
        {
            MessageDialog dialog = null;
            try
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.SuggestedFileName = clicked.name;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
                picker.FileTypeChoices.Add("Tous Les Fichiers", new List<string>(new string[] { "." }));
                picker.FileTypeChoices.Add("Images", new List<string>(new string[] { ".jpg" }));
                picker.FileTypeChoices.Add("Documents", new List<string>(new string[] { ".pdf" }));
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    this.progressBar.Value = 0;
                    var progressHandler = new Progress<LiveOperationProgress>(
                        (progress) => { this.progressBar.Value = progress.ProgressPercentage; });
                    this.ctsDownload = new System.Threading.CancellationTokenSource();
                    LiveConnectClient liveClient = new LiveConnectClient(LiveConfig.Auth.Session);
                    await liveClient.BackgroundDownloadAsync(clicked.id, file, 
                        this.ctsDownload.Token, progressHandler);

                    dialog = new MessageDialog("Le téléchargement de fichier '" + clicked.name + "' est términé");
                }
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                
                dialog = new MessageDialog("Le téléchargement de dichier '" + clicked.name + "' est annulé");
                
            }
            catch (LiveConnectException exception)
            {
                this.status.Text = "Error getting file contents: " + exception.Message;
                dialog = new MessageDialog("Le téléchargement de dichier '" + clicked.name + "' est annulé : impossible de récupérer le fichier à partir de SkyDrive");
            }
            await dialog.ShowAsync();
        }

        private void btnCancelDownload_Click(object sender, RoutedEventArgs e)
        {
            if (this.ctsDownload != null)
            {
                this.ctsDownload.Cancel();
            }
        }


        private void DisplayPrivacyPolicy(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", (uiCommand) => { LaunchPrivacyPolicyUrl(); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }
        async void LaunchPrivacyPolicyUrl()
        {
            Uri privacyPolicyUrl = new Uri("http://bibouh123.blog.com/2013/10/04/skydrive-downloader-policy/");
            var result = await Windows.System.Launcher.LaunchUriAsync(privacyPolicyUrl);
        }
    }
}
