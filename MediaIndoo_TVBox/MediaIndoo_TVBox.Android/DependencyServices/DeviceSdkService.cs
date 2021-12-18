using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediaIndoo_TVBox.Helps.DependecyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaIndoo_TVBox.Droid.DependencyServices.DeviceSdkService))]

namespace MediaIndoo_TVBox.Droid.DependencyServices
{
    public class DeviceSdkService : IDeviceSdkService
    {
        public string GetRootLocalPath() => VersionSdk11() ? $"{FileSystem.AppDataDirectory}/MediaIndooVideo" : "./sdcard/MediaIndooVideo";
        public bool VersionSdk11() => (int)Android.OS.Build.VERSION.SdkInt > 29 ? true : false;

    }
}