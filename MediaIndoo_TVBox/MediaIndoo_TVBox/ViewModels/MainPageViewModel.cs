using Acr.UserDialogs;
using MediaIndoo_TVBox.Banco.Contratos;
using MediaIndoo_TVBox.Helps.DependecyServices;
using MediaIndoo_TVBox.Helps.Message;
using MediaIndoo_TVBox.Models;
using MediaIndoo_TVBox.Repository;
using MediaIndoo_TVBox.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MediaIndoo_TVBox.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        private ObservableCollection<Player> _Players;
        public ObservableCollection<Player> Players
        {
            get { return _Players; }
            set { SetProperty(ref _Players, value); }
        }

        private List<string> _Urls;
        public List<string> Urls
        {
            get { return _Urls; }
            set { SetProperty(ref _Urls, value); }
        }
        public IPlayerRepository PlayerRepositoy { get; }
        public IPageDialogService PageDialogService { get; }
        public IVideosRepository VideosRepository { get; }
        public IVideosRepositorio VideosRepositorio { get; }
        public IEventAggregator Ea { get; }
        public IMensagemRepository MensagemRepository { get; }
        public DelegateCommand<Player> PlayerCommand { get; set; }
        public MainPageViewModel(INavigationService navigationService,
            IPlayerRepository playerRepositoy,
            IPageDialogService pageDialogService,
            IVideosRepository videosRepository,
            IVideosRepositorio videosRepositorio,
            IEventAggregator _ea,
            IMensagemRepository mensagemRepository)
            : base(navigationService)
        {
            PlayerRepositoy = playerRepositoy;
            PageDialogService = pageDialogService;
            VideosRepository = videosRepository;
            VideosRepositorio = videosRepositorio;
            Ea = _ea;
            MensagemRepository = mensagemRepository;
            Urls = new List<string>();
            Players = new ObservableCollection<Player>();
            PlayerCommand = new DelegateCommand<Player>(async (Player) => await MediaPlayer(Player));
        }

        private async Task MediaPlayer(Player playerSeleciodado)
        {
            await Task.Delay(300);

            var msgs = await CarregarMsgs(playerSeleciodado.UsuarioID);

            UserDialogs.Instance.ShowLoading($"Carrregando Videos...");
            var result = await VerificarExiteVideos(playerSeleciodado.Codigo);
            //var urls = await Carregar(playerSeleciodado.Codigo);
            UserDialogs.Instance.HideLoading();

            if (result)
            {

                if (Urls != null && Urls.Count > 0 && msgs != null)
                {
                    Preferences.Set("Play_key", playerSeleciodado.Codigo);
                    await Task.Delay(500);
                    await Navegar(playerSeleciodado, msgs);
                    Ea.GetEvent<MessageSentEvent>().Publish(Urls);
                }
                else
                    await PageDialogService.DisplayAlertAsync("Alert", "Nenhum video disponivel neste player!", "OK");
            }
            else
                await PageDialogService.DisplayAlertAsync("Alert", "Nenhum video disponivel neste player!", "OK");

        }

        private async Task<bool> VerificarExiteVideos(int codigo)
        {
            try
            {

                var result = await VideosRepository.GetPartVideo(codigo);
                if (result.IsSuccess)
                {
                    foreach (var item in result.Data)
                    {
                        var _video = VideosRepositorio.GetByGuid(item.VideoGuid);
                        if (_video != null)
                        {
                            var url = PegarUrl(_video.Nome);
                            if (!string.IsNullOrEmpty(url))
                                Urls.Add(url);
                        }
                        else
                        {
                            var video = await VideosRepository.GetByGuid(item.VideoGuid);
                            await GravarVideosPasta(video.Data.Nome, video.Data.UriByte); // qualquer coisa tirar esse a task desse metodo
                            GravarBanco(item);
                            var url = PegarUrl(video.Data.Nome);
                            if (!string.IsNullOrEmpty(url))
                                Urls.Add(url);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                await PageDialogService.DisplayAlertAsync("Erro!", err, "OK");
                return false;
            }
        }

        private string PegarUrl(string nome)
        {
            var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
            var DiretorioVideos = $"{RootLocalPath}/Videos/";
            return CombineUrl(DiretorioVideos, nome);
        }

        private async Task<List<Mensagem>> CarregarMsgs(int codigo)
        {
            UserDialogs.Instance.ShowLoading($"Carrregando Mensagens...");
            var result = await MensagemRepository.GetAllMsgsById(codigo);
            return result.IsSuccess ? result.Data : null;
            UserDialogs.Instance.HideLoading();
        }

        public async Task CheckPlayUser()
        {
            if (Preferences.ContainsKey("Play_key"))
            {
                UserDialogs.Instance.ShowLoading($"Carrregando Mensagens...");
                var _msg = await MensagemRepository.GetAllMsgsById(1);
                UserDialogs.Instance.HideLoading();

                //var _players = await PlayerRepositoy.GetAllPlayersId(1);
                if (_msg.IsSuccess)
                {
                    var Codigo = Preferences.Get("Play_key", 0);
                    var _player = Players.FirstOrDefault(x => x.Codigo == Codigo);
                    if (_player != null)
                    {
                        UserDialogs.Instance.ShowLoading($"Carrregando Videos...");
                        var result = await VerificarExiteVideos(_player.Codigo);
                        //var urls = await Carregar(_player.Codigo);
                        UserDialogs.Instance.HideLoading();
                        if (result)
                        {

                            if (_player != null && Urls != null)
                            {
                                await Navegar(_player, _msg.Data);
                                Ea.GetEvent<MessageSentEvent>().Publish(Urls);
                            }
                        }
                    }
                    else
                    {
                        Preferences.Remove("Play_key");
                    }
                }
                //}
            }

        }

        private async Task Navegar(Player player, List<Mensagem> Msgs)
        {
            using (var Dialog = UserDialogs.Instance.Loading($"Carregando...", null, null, true, MaskType.Black))
            {
                var navigationParams = new NavigationParameters { { "_PlayerKey", player }, { "_MsgsKey", Msgs } };
                await SafeNavigateAsync(nameof(MediaPlayerPage), navigationParams);
            }
        }
        //private async Task<List<Mensagem>> CarregarMensagens(int Id)
        //{
        //    using (var Dialog = UserDialogs.Instance.Loading($"Carregando Mensagens...", null, null, true, MaskType.Black))
        //    {
        //        var _msg = await MensagemRepository.GetAllMsgsById(1);
        //        if (_msg.IsSuccess)
        //            return _msg.Data;
        //        return null;
        //    }

        //}

        private async Task CarregarPlayers()
        {
            var _request = await PlayerRepositoy.GetAllPlayersId(1);
            if (_request.IsSuccess)
            {
                Players = new ObservableCollection<Player>(_request.Data);
                await CheckPlayUser();
            }
            else
            {
                await Task.Delay(600);
                await PageDialogService.DisplayAlertAsync("Erro!", $"Problemas com o servidor!", "OK");
            }
        }

        //private async Task<List<string>> Carregar(int CodigoPlayer)
        //{
        //    var Urls = new List<string>();
        //    try
        //    {
        //        //var _result = Connectivity.NetworkAccess == NetworkAccess.Internet ? GravarBanco() : BuscarBanco();
        //        var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
        //        var DiretorioVideos = $"{RootLocalPath}/Videos/";

        //        var _result = await VideosRepository.GetAllById(CodigoPlayer);

        //        if (Connectivity.NetworkAccess == NetworkAccess.Internet && _result.IsSuccess)
        //        {
        //            if (_result.Data.Count > 0)
        //            {

        //                //UserDialogs.Instance.ShowLoading($"Baixarndo...");
        //                foreach (var item in _result.Data)
        //                {
        //                    await GravarVideosPasta(item.Nome, item.UriByte); // qualquer coisa tirar esse a task desse metodo
        //                    GravarBanco(item);
        //                    //var _byte = Convert.FromBase64String(item.UriBase64);

        //                    //var _pathToNewFolder = Path.Combine(DiretorioVideos, item.Nome);
        //                    //var _pathToNewFolder = Path.Combine(DiretorioVideos, item.Nome);
        //                    var urlvideo = CombineUrl(DiretorioVideos, item.Nome);
        //                    if (!string.IsNullOrWhiteSpace(urlvideo))
        //                        Urls.Add(urlvideo);
        //                    else
        //                        await UserDialogs.Instance.AlertAsync("Nehum video disponível!", "Alert", "Ok");
        //                }
        //                return Urls;
        //            }
        //            else
        //            {
        //                await UserDialogs.Instance.AlertAsync("Nehum video disponível!", "Alert", "Ok");
        //                return null;
        //                //Preferences.Remove("Play_key");
        //                //await SafeNavigateAsync(nameof(MainPage));
        //            }
        //        }
        //        else
        //        {
        //            var result = VideosRepositorio.GetByIdFather(CodigoPlayer);
        //            if (result != null)
        //            {
        //                foreach (var item in result)
        //                {
        //                    var urlvideo = CombineUrl(DiretorioVideos, item.Nome);
        //                    if (!string.IsNullOrWhiteSpace(urlvideo))
        //                        Urls.Add(urlvideo);
        //                    else
        //                        await UserDialogs.Instance.AlertAsync("Nehum video disponível!", "Alert", "Ok");
        //                }
        //                return Urls;
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await PageDialogService.DisplayAlertAsync("Erro!", ex.Message, "OK");
        //        return null;
        //    }
        //    //finally
        //    //{
        //    //    //UserDialogs.Instance.HideLoading();
        //    //}
        //    ////UserDialogs.Instance.HideLoading();
        //}

        private string CombineUrl(string PastaVideo, string nomeVideo) => Path.Combine(PastaVideo, nomeVideo);
        private async void GravarBanco(Videos item)
        {
            try
            {
                var result = VideosRepositorio.GetById(item.Codigo);
                if (result == null)
                {
                    item.UriByte = null;
                    VideosRepositorio.Add(item);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Error!", "Ok");

            }
        }

        private async Task GravarVideosPasta(string NomeVideo, Byte[] bytes)
        {
            try
            {
                var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
                var DiretorioVideos = $"{RootLocalPath}/Videos/";

                if (!File.Exists(DiretorioVideos))
                {
                    Directory.CreateDirectory(DiretorioVideos);
                }
                string localPath = Path.Combine(DiretorioVideos, NomeVideo);
                if (!File.Exists(localPath))
                    File.WriteAllBytes(localPath, bytes);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                await PageDialogService.DisplayAlertAsync("Erro!", ex.Message, "OK");
            }
            //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            //var videopath = Path.Combine(doc, filename);

        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            using (var Dialog = UserDialogs.Instance.Loading($"Carregando Players...", null, null, true, MaskType.Black))
            {
                await CarregarPlayers();
            }
        }
        //public override void Destroy()
        //{
        //    GC.Collect();
        //}
    }
}
