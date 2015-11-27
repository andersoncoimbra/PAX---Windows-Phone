using PAX___Windows_Phone.Common;
using PAX___Windows_Phone.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo do Aplicativo Dinâmico está documentado em http://go.microsoft.com/fwlink/?LinkID=391641

namespace PAX___Windows_Phone
{
    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Obtém o <see cref="NavigationHelper"/> associado a esta <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Obtém o modelo de exibição desta <see cref="Page"/>.
        /// Isso pode ser alterado para um modelo de exibição fortemente tipado.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Preenche a página com conteúdo transmitido durante a navegação. Qualquer estado salvo também é
        /// fornecido ao recriar uma página a partir de uma sessão anterior.
        /// </summary>
        /// <param name="sender">
        /// A origem do evento; geralmente <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Dados de evento que fornecem o parâmetro de navegação passado para
        /// <see cref="Frame.Navigate(Type, Object)"/> quando esta página foi solicitada inicialmente e
        /// um dicionário de estado preservado por esta página durante uma sessão
        /// anterior. O estado será nulo na primeira vez que uma página for visitada.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: criar um modelo de dados apropriado ao seu domínio de problema para substituir os dados de exemplo
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

        /// <summary>
        /// Preserva o estado associado a esta página no caso do aplicativo ser suspenso ou a
        /// a página é descartada do cache de navegação. Os valores devem estar de acordo com a serialização
        /// serialização <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender"> A origem do evento; geralmente <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Dados do evento que fornecem um dicionário vazio a ser preenchido com
        /// estado serializável.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Salve o estado exclusivo da página aqui.
        }

        /// <summary>
        /// Adiciona um item à lista quando o botão da barra de aplicativos é clicado.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = this.pivot.SelectedIndex == 0 ? FirstGroupName : SecondGroupName;
            var group = this.DefaultViewModel[groupName] as SampleDataGroup;
            var nextItemId = group.Items.Count + 1;
            var newItem = new SampleDataItem(
                string.Format(CultureInfo.InvariantCulture, "Group-{0}-Item-{1}", this.pivot.SelectedIndex + 1, nextItemId),
                string.Format(CultureInfo.CurrentCulture, this.resourceLoader.GetString("NewItemTitle"), nextItemId),
                string.Empty,
                string.Empty,
                this.resourceLoader.GetString("NewItemDescription"),
                string.Empty);

            group.Items.Add(newItem);

            // Rola o novo item para a visualização.
            var container = this.pivot.ContainerFromIndex(this.pivot.SelectedIndex) as ContentControl;
            var listView = container.ContentTemplateRoot as ListView;
            listView.ScrollIntoView(newItem, ScrollIntoViewAlignment.Leading);
        }

        /// <summary>
        /// Invocado ao clicar em um item dentro de uma seção.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navega até a página de destino apropriado, configurando a nova página
            // passando as informações necessárias como um parâmetro de navegação
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Carrega o conteúdo do segundo item dinâmico quando rolado para a exibição.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        #region Registro do NavigationHelper

        /// <summary>
        /// Os métodos fornecidos nesta seção são usados simplesmente para permitir
        /// NavigationHelper para responder aos métodos de navegação da página.
        /// <para>
        /// A lógica específica à página deve ser colocada em manipuladores de eventos para 
        /// <see cref="NavigationHelper.LoadState"/>
        /// e <see cref="NavigationHelper.SaveState"/>.
        /// O parâmetro de navegação está disponível no método LoadState 
        /// além do estado da página preservado durante uma sessão anterior.
        /// </para>
        /// </summary>
        /// <param name="e">Fornece dados dos métodos de navegação e manipuladores
        /// de eventos que não podem cancelar a solicitação de navegação.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
