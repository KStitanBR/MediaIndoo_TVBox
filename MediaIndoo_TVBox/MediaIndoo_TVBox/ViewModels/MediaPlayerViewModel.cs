using Acr.UserDialogs;
using MediaIndoo_TVBox.Banco.Contratos;
using MediaIndoo_TVBox.Helps;
using MediaIndoo_TVBox.Helps.DependecyServices;
using MediaIndoo_TVBox.Helps.Message;
using MediaIndoo_TVBox.Models;
using MediaIndoo_TVBox.Repository;
using MediaIndoo_TVBox.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MediaIndoo_TVBox.ViewModels
{
    public class MediaPlayerViewModel : ViewModelBase
    {



        //private ObservableCollection<Videos> _Videos;
        //public ObservableCollection<Videos> Videos
        //{
        //    get { return _Videos; }
        //    set { SetProperty(ref _Videos, value); }
        //}


        private ObservableCollection<Mensagem> _Msgs;
        public ObservableCollection<Mensagem> Msgs
        {
            get { return _Msgs; }
            set { SetProperty(ref _Msgs, value); }
        }

        private ObservableCollection<string> _caminhovideos;
        public ObservableCollection<string> caminhovideos
        {
            get { return _caminhovideos; }
            set { SetProperty(ref _caminhovideos, value); }
        }

        private Player _player;
        public Player player
        {
            get { return _player; }
            set { SetProperty(ref _player, value); }
        }

        public IMensagemRepository MensagemRepository { get; }
        public IPlayeReqRepository PlayeReqRepository { get; }
        public IPageDialogService PageDialogService { get; }
        public IVideosRepositorio VideosRepositorio { get; }

        public IEventAggregator Ea;
        public IVideosRepository VideosRepository { get; }
        HubConnection hubConnection;

        public MediaPlayerViewModel(INavigationService navigationService,
            IMensagemRepository mensagemRepository,
            IPlayeReqRepository playeReqRepository,
            IPageDialogService pageDialogService,
            IEventAggregator _ea,
            IVideosRepositorio videosRepositorio,
            IVideosRepository videosRepository) : base(navigationService)
        {
            MensagemRepository = mensagemRepository;
            PlayeReqRepository = playeReqRepository;
            PageDialogService = pageDialogService;
            VideosRepository = videosRepository;
            Ea = _ea;
            VideosRepositorio = videosRepositorio;
            Msgs = new ObservableCollection<Mensagem>();
            caminhovideos = new ObservableCollection<string>();
            Ea.GetEvent<MessageSentEvent>().Subscribe(UrlReceived);
            DoRealTimeSuff();
        }
        async private void DoRealTimeSuff()
        {
            SignalRChatSetup();
            await SignalRConnect();
        }
        private void SignalRChatSetup()
        {
            //var ip = "localhost";
            hubConnection = new HubConnectionBuilder().WithUrl("http://191.252.64.6/mi/MsgHub").Build();
            //hubConnection = new HubConnectionBuilder().WithUrl("http://192.168.18.79:5000/MsgHub").Build();

            hubConnection.On<Guid>("ReceiveVideo", async (GuidVideo) =>
            {

                await VerificarVideo(GuidVideo);
                //var receivedMessage = message;
                //Msgs = new ObservableCollection<Mensagem>(video);
                //this.MessageHolder.Text += receivedMessage + "\n";
            });

        }

        private async Task VerificarVideo(Guid GuidVideo)
        {
            await Task.Run(async () =>
             {
                 var _video = VideosRepositorio.GetByGuid(GuidVideo);
                 if (_video != null)
                 {
                     RemoveVideo(_video);
                 }
                 else
                 {
                     var result = await VideosRepository.GetByGuid(GuidVideo);
                     if (result.IsSuccess)
                         AddVideo(result.Data);
                 }
             });
        }

        private void AddVideo(Videos video)
        {
            video.UriByte = null;
            VideosRepositorio.Add(video);
            var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
            var DiretorioVideos = $"{RootLocalPath}/Videos/";
            var urlvideo = CombineUrl(DiretorioVideos, video.Nome);
            caminhovideos.Add(urlvideo);
        }

        private string CombineUrl(string siretorioVideos, string nomeVideo) => Path.Combine(siretorioVideos, nomeVideo);

        private void RemoveVideo(Videos video)
        {
            VideosRepositorio.Delete(video);
            var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
            var DiretorioVideos = $"{RootLocalPath}/Videos/";
            var urlvideo = CombineUrl(DiretorioVideos, video.Nome);
            caminhovideos.Remove(urlvideo);
        }

        async Task SignalRConnect()
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                // Connection failed. Fail graciously.
                await PageDialogService.DisplayAlertAsync("Error!", ex.Message, "OK");
            }
        }

        private void UrlReceived(List<string> urls)
        {
            caminhovideos = new ObservableCollection<string>(urls);
        }

        private void EnviarStatus(int playerID)        ////Enviar Msg
        //async Task SignalRSendMessage(int codigo, Mensagem message)
        //{
        //    try
        //    {
        //        //await hubConnection.StartAsync();
        //        await hubConnection.InvokeAsync("SendMessage", codigo, message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Send failed. Fail graciously.
        //    }
        //}
        {
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
             {
                 Task.Run(async () =>
                 { 
                     await PlayeReqRepository.PostAsync(new PlayeReq
                     {
                         PlayerID = playerID,
                         PlayeReqGuid = Guid.NewGuid()
                     });
                 });
                 return true;
             });
        }
        public  override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("_MsgsKey") && parameters.ContainsKey("_PlayerKey"))
            {
                Msgs = new ObservableCollection<Mensagem>(parameters.GetValue<List<Mensagem>>("_MsgsKey"));
                var _player = parameters.GetValue<Player>("_PlayerKey");
                EnviarStatus(_player.Codigo);
            }
        }

        //public override void Destroy()
        //{
        //    base.Destroy();
        //}
    }
}
