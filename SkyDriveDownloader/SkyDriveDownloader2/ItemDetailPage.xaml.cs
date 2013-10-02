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

// Pour en savoir plus sur le modèle d'élément Page Détail de l'élément, consultez la page http://go.microsoft.com/fwlink/?LinkId=234232

namespace SkyDriveDownloader2
{
    /// <summary>
    /// Page affichant les détails d'un élément au sein d'un groupe, offrant la possibilité de
    /// consulter les autres éléments qui appartiennent au même groupe.
    /// </summary>
    public sealed partial class ItemDetailPage : SkyDriveDownloader2.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
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
            // Autorise l'état de page enregistré à substituer l'élément initial à afficher
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: créez un modèle de données approprié pour le domaine posant problème pour remplacer les exemples de données
            var item = SampleDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = item.Group.Items;
            this.flipView.SelectedItem = item;
        }

        /// <summary>
        /// Conserve l'état associé à cette page en cas de suspension de l'application ou de la
        /// suppression de la page du cache de navigation. Les valeurs doivent être conformes aux
        /// exigences en matière de sérialisation de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Dictionnaire vide à remplir à l'aide de l'état sérialisable.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }
    }
}
