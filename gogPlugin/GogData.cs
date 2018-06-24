using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;

using FileUtils = System.IO.File;

using Dlc = gogPlugin.GogData.GameDetails;

namespace gogPlugin
{
    // TODO: exclude and implement permanent data storage in different class (settings, user data?) 

    public static class GogData
    {

        public static AuthData sessionData { get; set; } = null;
        private static string tokenPath = ".\\Plugins\\gogPlugin\\token.gog";

        // data structure for storing gog.auth.com requests
        public class AuthData
        {
                public string access_token { get; set; }
                public int expires_in { get; set; }
                public string token_type { get; set; }
                public string scope { get; set; }
                public string session_id { get; set; }
                public string refresh_token { get; set; }
                public string user_id { get; set; }

        }

        public class Error
        {
            public string error { get; set; }
            public string error_description { get; set; }
        }

        public class GamesOwned
        {
            public List<string> owned { get; set; }
        }

        public class Extra
        {
            public string manualUrl { get; set; }
            public string downloaderUrl { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int info { get; set; }
            public string size { get; set; }
        }

        public class SimpleGalaxyInstaller
        {
            public string path { get; set; }
            public string os { get; set; }
        }

        public class Download
        {
            public string manualUrl { get; set; }
            public string downloaderUrl { get; set; }
            public string name { get; set; }
            public string version { get; set; }
            public string date { get; set; }
            public string size { get; set; }
        }

        public class DownloadBySystem
        {
            public List<Download> windows;
            public List<Download> mac;
            public List<Download> linux;
        }

        //public class DownloadByLanguage
        //{
        //    public Dictionary<String, DownloadBySystem> downloads;
        //}

        public class GameDetails 
        {        
            public string title { get; set; }
            public string backgroundImage { get; set; }
            public string cdKey { get; set; }
            public string textInformation { get; set; }
            //public List<DownloadByLanguage> downloads { get; set; }
            public List<List<object>> downloads { get; set; }
            public List<List<object>> galaxyDownloads { get; set; }
            public List<Extra> extras { get; set; }
            public List<Dlc> dlcs { get; set; }
            public List<String> tags { get; set; }
            public bool isPreOrder { get; set; }
            public int releaseTimestamp { get; set; }
            public List<String> messages { get; set; }
            public string changelog { get; set; }
            public string forumLink { get; set; }
            public bool isBaseProductMissing { get; set; }
            public object missingBaseProduct { get; set; }
            public List<String> features { get; set; }
            public List<SimpleGalaxyInstaller> simpleGalaxyInstallers { get; set; }
        }        

        public class ContentSystemCompatibility
        {
            public bool windows { get; set; }
            public bool osx { get; set; }
            public bool linux { get; set; }
        }

        public class Languages
        {
            public string en { get; set; }
        }

        public class Links
        {
            public string purchase_link { get; set; }
            public string product_card { get; set; }
            public string support { get; set; }
            public string forum { get; set; }
        }

        public class InDevelopment
        {
            public bool active { get; set; }
            public object until { get; set; }
        }

        public class Images
        {
            public string background { get; set; }
            public string logo { get; set; }
            public string logo2x { get; set; }
            public string icon { get; set; }
            public string sidebarIcon { get; set; }
            public string sidebarIcon2x { get; set; }
        }

        public class File
        {
            public string id { get; set; }
            public int size { get; set; }
            public string downlink { get; set; }
        }

        public class Installer
        {
            public string id { get; set; }
            public string name { get; set; }
            public string os { get; set; }
            public string language { get; set; }
            public string language_full { get; set; }
            public object version { get; set; }
            public int total_size { get; set; }
            public List<File> files { get; set; }
        }

        public class BonusContent
        {
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int count { get; set; }
            public int total_size { get; set; }
            public List<File> files { get; set; }
        }

        public class Download2
        {
            public List<Installer> installers { get; set; }
            public List<object> patches { get; set; }
            public List<object> language_packs { get; set; }
            public List<BonusContent> bonus_content { get; set; }
        }

        public class Description
        {
            public string lead { get; set; }
            public string full { get; set; }
            public string whats_cool_about_it { get; set; }
        }

        public class FormattedImage
        {
            public string formatter_name { get; set; }
            public string image_url { get; set; }
        }

        public class Screenshot
        {
            public string image_id { get; set; }
            public string formatter_template_url { get; set; }
            public List<FormattedImage> formatted_images { get; set; }
        }

        public class MoreGameDetails
        {
            public int id { get; set; }
            public string title { get; set; }
            public string purchase_link { get; set; }
            public string slug { get; set; }
            public ContentSystemCompatibility content_system_compatibility { get; set; }
            public Languages languages { get; set; }
            public Links links { get; set; }
            public InDevelopment in_development { get; set; }
            public bool is_secret { get; set; }
            public string game_type { get; set; }
            public bool is_pre_order { get; set; }
            public DateTime release_date { get; set; }
            public Images images { get; set; }
            public List<object> dlcs { get; set; }
            public Download2 downloads { get; set; }
            public List<object> expanded_dlcs { get; set; }
            public Description description { get; set; }
            public List<Screenshot> screenshots { get; set; }
            public List<object> videos { get; set; }
            public List<object> related_products { get; set; }
            public object changelog { get; set; }
        }

        public static void save() {

            if (sessionData == null || sessionData.refresh_token == null) return;

            try
            {                
                if (FileUtils.Exists(tokenPath))
                {
                    FileUtils.Delete(tokenPath);
                }
                FileStream fs = FileUtils.Create(tokenPath);
                StreamWriter writer = new StreamWriter (fs, Encoding.UTF8);
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

        public static void load()
        {
            try
            { 
                StreamReader reader = new StreamReader(tokenPath);
                sessionData = new AuthData();
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
