using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MediaIndoo_TVBox.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        private bool isNavigating;

        protected Task SafeNavigateAsync(string name, NavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true)
        {
            if (isNavigating)
                return Task.CompletedTask;
            isNavigating = true;
            try { return NavigationService.NavigateAsync(name, parameters, useModalNavigation, animated); }
            catch { return Task.CompletedTask; }
            finally
            {
                Device.StartTimer(
         TimeSpan.FromMilliseconds(800),
         () => isNavigating = false);
            }
        }


        //public async Task<bool> MsgErrInterntOK(string title, string msg)
        //{
        //    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        //    {
        //        await PageDialogService.DisplayAlertAsync(title, msg, "OK");
        //        return true;
        //    }

        //    return false;
        //}
        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
