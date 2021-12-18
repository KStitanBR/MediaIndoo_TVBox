using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Helps.DependecyServices
{
    public interface IDeviceSdkService
    {
        bool VersionSdk11();
        string GetRootLocalPath();
    }
}
