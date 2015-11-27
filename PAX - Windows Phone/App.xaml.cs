using PAX___Windows_Phone.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// O modelo do Aplicativo Dinâmico está documentado em http://go.microsoft.com/fwlink/?LinkID=391641

namespace PAX___Windows_Phone
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados quando o aplicativo é iniciado para abrir um arquivo específico, para exibir
        /// resultados da pesquisa e assim por diante.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa.
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página.
                rootFrame = new Frame();

                // Associe o quadro a uma chave SuspensionManager.
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: Altere este valor para um tamanho de cache apropriado para seu aplicativo.
                rootFrame.CacheSize = 1;

                // Definir o idioma padrão
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restaure o estado de sessão salvo somente quando apropriado.
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Erro ao restaurar o estado.
                        // Vamos supor que não há estado e continuar.
                    }
                }

                // Coloque o quadro na Janela atual.
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                //Remove a animação turnstile na inicialização.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                // configurando a nova página passando as informações necessárias como um parâmetro
                // parâmetro.
                if (!rootFrame.Navigate(typeof(PivotPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Verifique se a janela atual está ativa.
            Window.Current.Activate();
        }

        /// <summary>
        /// Restaura as transições de conteúdo depois que o aplicativo é iniciado.
        /// </summary>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <resumo>
        /// Chamado quando a execução do aplicativo está sendo suspensa.  O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou reiniciado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A fonte da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
