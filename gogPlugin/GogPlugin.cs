using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

using FileUtils = System.IO.File;

using CefSharp;
using Unbroken.LaunchBox.Plugins;

namespace gogPlugin
{
    class GogPlugin : ISystemEventsPlugin
    {
        public static GogDataStructures.AuthData sessionData { get; set; } = null;
        private static string tokenPath = ".\\Plugins\\gogPlugin\\token.gog";

        private CefLibraryHandle libraryLoader;

        public void OnEventRaised(string eventType)
        {
            if (eventType.Equals("LaunchBoxStartupCompleted"))
            {

                LoadAuthToken();

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
                SaveAuthToken();
            }
        }

        public static void SaveAuthToken()
        {

            if (sessionData == null || sessionData.refresh_token == null) return;

            try
            {
                if (FileUtils.Exists(tokenPath))
                {
                    FileUtils.Delete(tokenPath);
                }
                FileStream fs = FileUtils.Create(tokenPath);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine(sessionData.refresh_token);
                writer.Flush();
                writer.Close();
                writer.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception e)
            {
                //file not yet created
                MessageBox.Show(e.GetType().ToString() + ": " + e.Message + "\n\n" + e.StackTrace);
            }
        }

        public static void LoadAuthToken()
        {
            try
            {
                StreamReader reader = new StreamReader(tokenPath);
                sessionData = new GogDataStructures.AuthData();
                sessionData.refresh_token = reader.ReadLine();
                reader.Close();
                reader.Dispose();
            }
            catch
            {
                //file not yet created               
            }

        }
    }
}

