using PAX___Windows_Phone.Common;
using PAX___Windows_Phone.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    /// <summary>
    /// Uma página que exibe os detalhes de um único item dentro de um grupo.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            this.InitializeComponent();

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
        /// anterior.  O estado será nulo na primeira vez que uma página for visitada.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Crie um modelo de dados apropriado ao seu domínio de problema para substituir os dados de exemplo.
            var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
            this.DefaultViewModel["Item"] = item;
        }

        /// <summary>
        /// Preserva o estado associado a esta página no caso do aplicativo ser suspenso ou a
        /// página é descartada do cache de navegação.  Valores devem estar de acordo com os requisitos de
        /// serialização <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender"> A origem do evento; geralmente <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Dados do evento que fornecem um dicionário vazio a ser preenchido com
        /// estado serializável.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Salve o estado exclusivo da página aqui.
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