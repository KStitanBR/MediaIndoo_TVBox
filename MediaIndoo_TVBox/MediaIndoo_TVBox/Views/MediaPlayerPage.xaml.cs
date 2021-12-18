using MediaIndoo_TVBox.Helps.DependecyServices;
using MediaIndoo_TVBox.Models;
using MediaIndoo_TVBox.Services;
using MediaIndoo_TVBox.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaIndoo_TVBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayerPage : ContentPage, IDestructible
    {
        public MediaPlayerPage()
        {
            InitializeComponent();
        }
        private void CarouselViewVideos_PositionChanged(object sender, Xamarin.Forms.PositionChangedEventArgs e)
        {

            StopVideoOutOfBounds();
            PlayVideoInOfBounds();

        }

        //private CancellationTokenSource _cancellationTokenSourceOfAnimations;

        public void PlayVideoInOfBounds()
        {
            var tasks = new List<Task>();

            //_cancellationTokenSourceOfAnimations = new CancellationTokenSource();

            if (carouselviewvideos.VisibleViews.LastOrDefault() is View view)
            {

                if (view.FindByName<MediaElement>("video") is MediaElement videoInOfBounds)
                {
                    tasks.Add(PlayVideoAsync(videoInOfBounds));
                }
            }


         
            //if (CarrouselMsg.VisibleViews.LastOrDefault() is View view1)
            //{
            //    if (view1.FindByName<MarqueeLabel>("AnimatedSongName") is MarqueeLabel songName)
            //        tasks.Add(songName.StartAnimationAsync(_cancellationTokenSourceOfAnimations.Token));
            //}
        }
        public void StopVideoOutOfBounds()
        {
            //_cancellationTokenSourceOfAnimations?.Cancel();
            //_cancellationTokenSourceOfAnimations?.Dispose();
            //_cancellationTokenSourceOfAnimations = null;

            var tasks = new List<Task>();

            for (var index = 0; index < carouselviewvideos.VisibleViews.Count - 1; index++)
            {
                if (carouselviewvideos.VisibleViews[index] is View view)
                {
                    if (view.FindByName<MediaElement>("video") is MediaElement videoOutOfBounds)
                    {
                        tasks.Add(StopVideoAsync(videoOutOfBounds));
                    }

                }
                //if (CarrouselMsg.VisibleViews.LastOrDefault() is View view1)
                //{

                //    if (view1.FindByName<MarqueeLabel>("AnimatedSongName") is MarqueeLabel songName)
                //        tasks.Add(RestoreOriginalTextAsync(songName));
                //}
            }
        }
        //private Task RestoreOriginalTextAsync(MarqueeLabel songName)
        //{
        //    songName.RestoreOriginalText();
        //    return Task.FromResult(true);
        //}

        private bool IsFirstVideoToAppear(CurrentItemChangedEventArgs args) => args.PreviousItem is null;

        private async void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs args)
        {
            {
                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(300);

                PlayVideoInOfBounds();
            }
        }


        private Task PlayVideoAsync(MediaElement video)
        {
            video.Play();
            return Task.FromResult(true);
        }
        private Task StopVideoAsync(MediaElement video)
        {
            video.Stop();
            return Task.FromResult(true);
        }
        private  void Video_MediaEnded(object sender, EventArgs e)
        {

            var _video = (MediaElement)sender;
            if (_video.Duration != null)
            {
                if (carouselviewvideos.BindingContext is MediaPlayerViewModel vm)
                {
                   /*await*/  EnviarReq(_video.BindingContext);
                    carouselviewvideos.Position = (carouselviewvideos.Position + 1) % vm.caminhovideos.Count;
                }
            }
        }

        private void EnviarReq(object obj)
        {
            var _reqV = new VideoReq
            {
                Date = DateTime.Now,
                Guid = Guid.NewGuid(),
                NomeVideo = RecortaUrl((string)obj),
                PlayerID = Preferences.Get("Play_key", 0)
            };
             new VideoReqService().PostAsync(_reqV);

        }
        private string RecortaUrl(string url)
        {
            var RootLocalPath = Xamarin.Forms.DependencyService.Get<IDeviceSdkService>().GetRootLocalPath();
            var DiretorioVideos = $"{RootLocalPath}/Videos/";
            return url.Replace(DiretorioVideos, "");
        }
        protected override bool OnBackButtonPressed()
        {
            Preferences.Remove("Play_key");
            return true;
        }
        public void Destroy()
        {
            GC.Collect();
        }
    }

}