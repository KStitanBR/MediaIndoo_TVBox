using MediaIndoo_TVBox.Models;
using MediaIndoo_TVBox.Services;
using MediaIndoo_TVBox.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediaIndoo_TVBox.Behaviors
{
    public class MarqueeBehavior : Behavior<StackLayout>
    {
        #region Properties

        private StackLayout stack { get; set; }
        private StackLayout stackLayout { get; set; }
        private bool isStartTimer;

        public double PageWidth
        {
            get { return (double)GetValue(PageWidthProperty); }
            set { SetValue(PageWidthProperty, value); }
        }

        // Using a BindableProperty as the backing store for PageWidth. This enables animation, styling, binding, etc.
        public static readonly BindableProperty PageWidthProperty =
            BindableProperty.Create("PageWidth", typeof(double), typeof(MarqueeBehavior));

        #endregion

        #region Methods

        protected override void OnAttachedTo(StackLayout stackLayout)
        {
            base.OnAttachedTo(stackLayout);
            this.stack = stackLayout;
            isStartTimer = false;
            AddMarquee();
        }

        /// <summary>
        /// This event is invoked when stacklayout binding context is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StackLayout_BindingContextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            stackLayout = sender as StackLayout;
            AddMarquee();
        }

        private async void AddMarquee()
        {
            //if (stackLayout.BindingContext != null
            //     && stackLayout.BindingContext
            //     is MediaPlayerViewModel ViewModel
            //     && ViewModel != null)
            //{
            await AtualizarLista();
            isStartTimer = true;
            await StartAnimation();
            //if (msgs != null && msgs.Count > 0)
            //{
            //    //stackLayout.Children.Clear();
            //    //ViewModel.Msgs.CollectionChanged += Msgs_CollectionChanged;
            //    foreach (var msg in msgs)
            //    {
            //        stackLayout.Children.Add(GetNewLabel(msg.Msg));
            //    }


            //}
        }
        //HubConnection hubConnection;
        //async private void DoRealTimeSuff()
        //{
        //    //SignalRChatSetup();
        //    await SignalRConnect();
        //}
        //private async void SignalRChatSetup()
        //{
        //    hubConnection = new HubConnectionBuilder().WithUrl("http://192.168.18.79:5000/MsgHub").Build();

        //    //hubConnection.On<int, Mensagem>("ReceiveMessage", async (codigo, message) =>
        //    //{
        //    //    //AddMarquee(message);
        //    //    await VerificarMarquee(message);
        //    //});

        //}

        //private async Task VerificarMarquee(StackLayout stack, Mensagem message)
        //{
        //    await Task.Run(async () =>
        //     {
        //         if (stack.BindingContext
        //         is MediaPlayerViewModel ViewModel
        //         && ViewModel != null)
        //         {
        //             var list = ViewModel.Msgs.FirstOrDefault(x => x.MensagemGuid.Equals(message.MensagemGuid));
        //             if (list != null)
        //                 RemoveView(stack, message.Msg);
        //             else
        //                 AddView(stack, message.Msg);
        //         }

        //     });
        //}

        //private void RemoveView(StackLayout stack, string msg) => stack.Children.Remove(GetNewLabel(msg));
        //private void AddView(StackLayout stack, string msg) => stack.Children.Add(GetNewLabel(msg));

        //async Task SignalRConnect()
        //{
        //    try
        //    {
        //        await hubConnection.StartAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Connection failed. Fail graciously.
        //    }
        //}


        /// <summary>
        /// This method is used for starting the marquee scrolling animation.
        /// </summary>
        private async Task StartAnimation()
        {
            await Task.Run(() =>
             {
                 Device.StartTimer(TimeSpan.FromMilliseconds(70), () =>
                 {
                     stack.TranslationX -= 5f;
                     if (Math.Abs(stack.TranslationX) > stack.Width)
                     {
                         stack.TranslationX = PageWidth;
                         this.OnAttachedTo(stack);
                     }
                     return isStartTimer;
                 });
             });
        }
        private async Task AtualizarLista()
        {
            try
            {
                var _List = await new MensagemService().GetAllMsgsById(1);
                if (_List.IsSuccess)
                {
                    stack.Children.Clear();
                    foreach (var item in _List.Data)
                    {
                        stack.Children.Add(GetNewLabel(item.Msg));
                    }
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
        }


        private Label GetNewLabel(string content)
        {
            var Label = new Label
            {
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                Text = content,
                HeightRequest = 45,
                Margin = new Thickness(0, 0, 13, 0),
                Padding = new Thickness(12, -3, 18, 0),
                TextColor = System.Drawing.Color.White,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = System.Drawing.Color.Transparent
                //BackgroundColor = GetPriorityColor(priorityId)
            };

            //var button = new SfButton()
            //{
            //    FontSize = 16,
            //    Text = content,
            //    HeightRequest = 45,
            //    HasShadow = false,
            //    Margin = new Thickness(0, 0, 8, 0),
            //    Padding = new Thickness(12, 0, 18, 0),
            //    ImageSource = imageName,
            //    ShowIcon = true,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    TextColor = (Color)Application.Current.Resources["ContentTextColor"],
            //    BackgroundColor = GetPriorityColor(priorityId),
            //    VerticalOptions = LayoutOptions.Center,

            //};

            //button.Clicked += (sender, args) =>
            //{
            //    //Perform marquee selected item.
            //};

            return Label;
        }

        /// <summary>
        /// This method is used to return the color based on priority level.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //private Color GetPriorityColor(int id)
        //{
        //    if (id == 1)
        //        return (Color)Application.Current.Resources["HighPriorityBackgroundColor"];
        //    else if (id == 2)
        //        return (Color)Application.Current.Resources["NormalPriorityBackgroundColor"];
        //    else
        //        return (Color)Application.Current.Resources["LowPriorityBackgroundColor"];
        //}

        protected override void OnDetachingFrom(StackLayout stackLayout)
        {
            //stackLayout.BindingContextChanged -= StackLayout_BindingContextChanged;
            isStartTimer = false;
            base.OnDetachingFrom(stackLayout);
        }

        #endregion
    }
}
