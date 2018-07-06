using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using Unbroken.LaunchBox.Plugins;

namespace gogPlugin
{
    class GogPluginEvents : ISystemEventsPlugin
    {
        private CefLibraryHandle libraryLoader;

        public void OnEventRaised(string eventType)
        {
            if (eventType.Equals("LaunchBoxStartupCompleted"))
            {

                GogData.load();

                if (!Cef.IsInitialized)
                {
                    string launchboxDir = AppDomain.CurrentDomain.BaseDirectory;

                    libraryLoader = new CefLibraryHandle($@"{launchboxDir}\CefSharp\x64\");
                    bool isValid = !libraryLoader.IsInvalid;

                    CefSettings settings = new CefSettings();
                    settings.BrowserSubprocessPath = $@"{launchboxDir}\CefSharp\x64\CefSharp.BrowserSubprocess.exe";

                    Cef.Initialize(settings);
                }

            }
            else if (eventType.Equals("LaunchBoxShutdownBeginning"))
            {
                Cef.Shutdown();
                libraryLoader.Dispose();
                GogData.save();
            }
        }
    }
}
