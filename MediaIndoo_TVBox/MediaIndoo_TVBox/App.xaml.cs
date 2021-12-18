using Acr.UserDialogs;
using MediaIndoo_TVBox.Repository;
using MediaIndoo_TVBox.Services;
using MediaIndoo_TVBox.ViewModels;
using MediaIndoo_TVBox.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace MediaIndoo_TVBox
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

           await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }
     
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MediaPlayerPage, MediaPlayerViewModel>();

            containerRegistry.Register<IPlayerRepository, PlayerService>();
            containerRegistry.Register<IMensagemRepository, MensagemService>();
            containerRegistry.Register<IVideosRepository, VideoService>();
            containerRegistry.Register<IPlayeReqRepository, PlayeReqService>();
            containerRegistry.Register<IVideoReqRepository, VideoReqService>();

            // Repository SqLite
            //containerRegistry.Register<Banco.Contratos.IMensagemRepositorio, Banco.Repositorios.MensagemRepository>();
            //containerRegistry.Register<Banco.Contratos.IPlayerRepositorio, Banco.Repositorios.PlayerRepository>();
            //containerRegistry.Register<Banco.Contratos.IUsuarioRepositorio, Banco.Repositorios.UsuarioRepository>();
            containerRegistry.Register<Banco.Contratos.IVideosRepositorio, Banco.Repositorios.VideosRepository>();
        }

    }
}
