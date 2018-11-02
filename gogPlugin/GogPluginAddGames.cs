using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using Newtonsoft.Json.Linq;
using gogPlugin.forms;

namespace gogPlugin
{
    // TODO: Harmonize interface between plugin class and forms
    // TODO: Encapsulate authentication process in own class
    // TODO: use Launchbox cefSharp binaries
    // FEATURE: save settings between imports
    // FEATURE: Implement options to download images and mauals
    // FEATURE: (De)select all on import
    // FEATURE: Sort on game import


    public class GogPluginAddGames : ISystemMenuItemPlugin
    {

        public string Caption { get { return "Add Games From gog.com"; } }
        public System.Drawing.Image IconImage {
            get
            {
                try
                {
                    return System.Drawing.Image.FromFile(@".\Plugins\gogPlugin\logo.png");
                } catch (Exception e)
                {
                    return null;
                }
            }
        }
        public bool ShowInLaunchBox { get { return true; } }
        public bool ShowInBigBox { get { return false; } }
        public bool AllowInBigBoxWhenLocked { get { return false; } }

        //public Boolean importBackground;
        //public Boolean importManual;
        //public Boolean importInfo;
        //public Boolean importBox;

        private WebClient webClient;

        public enum Mode { startWithGalaxy, linkToDownload }
        Mode mode = Mode.startWithGalaxy;
        
        public Dictionary<int, string> platforms;
        private string platform;

        private bool skipImported;
        private bool skipByTitle;

        public List<GameImport> gamesFound = new List<GameImport>();
        public List<String> skipGogId;
        public List<String> skipTitle;
        private string galaxyPath;

        public class GameImport
        {
            public string _gogId;
            public string _title;
            public string title { get { return _importTitle; } set { _importTitle = value; } }
            public string _importTitle;
            public bool _import;
            public bool import { get { return _import; } set { _import = value; } }
            public List<GameDownload> _downloads;
            public List<GameDownload> downloads { get { return _downloads; } }
            public GameDownload _selectedDownload;
            public GameDownload selectedDownload {
                get { return _selectedDownload; } set { _selectedDownload = value; } }
            public GogDataStructures.GameDetails _gameDetails;
            public GogDataStructures.MoreGameDetails _moreGameDetails;
        }

        public class GameDownload
        {
            public string _name;
            public string _language;
            public string language { get { return _language; } set { _language = value; } } 
            public string _version;
            public string version { get { return _version; } set { _version = value; } }
            public string _link;
            public string link { get { return _link; } set { _link = value; } }
            public GameDownload self { get { return this; } }

            public string display { get { return _name + " (" + _language + (_version != null ? " " + _version : "") + ")"; }}
            public override string ToString() {
                return _name + " (" + _language + (_version != null ? " " + _version : "") + ")";
            }
        }

        //##################    Authentication    ##################

        public void OnSelected()
        {
            webClient = new WebClient();
            if (authenticate())
            {
                GetGames();
            }
        }
        
        private bool authenticate()
        {
            if (GogPlugin.sessionData == null || !getToken(null, true))
            {
                LoginBrowser loginBrowser = new LoginBrowser(this);
                loginBrowser.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void loginSuccess(object sender, FormClosedEventArgs e, string loginCode)
        {
            if (getToken(loginCode, false))
            {
                GetGames();
            }                
        }

        private bool getToken(string loginCode, bool refresh)
        {
            String json = null;
            try
            {               
                if (refresh)
                {
                    json = webClient.DownloadString("https://auth.gog.com//token?client_id=46899977096215655&client_secret=9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9&grant_type=refresh_token&refresh_token="
                       + GogPlugin.sessionData.refresh_token);
                }
                else
                {
                    json = webClient.DownloadString("https://auth.gog.com//token?client_id=46899977096215655&client_secret=9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9&grant_type=authorization_code&code="
                       + loginCode
                       + "&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient");
                }
                GogPlugin.sessionData = JsonConvert.DeserializeObject<GogDataStructures.AuthData>(json);
                webClient.Headers.Add("Authorization", "Bearer " + GogPlugin.sessionData.access_token);          
                return true;

            } catch (Exception e)
            {
                handleApiExceptions(e, json);
            }
            return false;
        }

        //##################    Scaning gog.com for games owned    ##################

        private async void GetGames()
        {
            platforms = new Dictionary<int, string>();

            int i = 0;
            foreach (IPlatform platform in PluginHelper.DataManager.GetAllPlatforms())
            {
                platforms.Add(i, platform.Name);
                i++;
            }

            ImportOptions options = new ImportOptions(this);
            options.Show();
            await options.ok.Task;
            options.Close();
            this.skipByTitle = options.skipByTitle;
            this.skipImported = options.skipImported;
            this.mode = options.mode;
            this.galaxyPath = options.galaxyPath;
            this.platform =options.platform;
            options.Dispose();

            await Scan_Library();
            MessageBox.Show("Skip " + skipGogId.Count.ToString() + " games already imported and " + skipTitle.Count.ToString() + " games by title.");

            ProgressForm progressForm = new ProgressForm();
            progressForm.Show();
            await progressForm.ShowProgress(GetGames_doWork, "Collecting Game Information");
            progressForm.Close();
            progressForm.Dispose();

            if (mode.Equals(Mode.startWithGalaxy))
            {
                GameImportForm importForm = new GameImportForm(this);
                importForm.Show();
            } else if (mode.Equals(Mode.linkToDownload))
            { 
                GameImportForm2 importForm = new GameImportForm2(this);
                importForm.Show();
            }

            webClient.Dispose();
        }

        //##################    Scaning Launchbox library for existing games    ##################

        private Task Scan_Library()
        {
            if (!(skipByTitle || skipImported)) return Task.CompletedTask;

            skipGogId = new List<string>();
            skipTitle = new List<string>();
            List<IGame> allGames = PluginHelper.DataManager.GetAllGames().ToList();
                       
            foreach (IGame game in allGames)
            {
                if (skipImported && game.GetAllCustomFields().ToList().Any(x => x.Name.Equals("gogId"))) {
                    skipGogId.Add(game.GetAllCustomFields().ToList().Find(x => x.Name.Equals("gogId")).Value);
                }
                if (skipByTitle)
                {
                    skipTitle.Add(game.Title);
                }
            }
            return Task.CompletedTask;
        }

        private void GetGames_doWork(IProgress<int> progress)
        {
            string json = null;
            GogDataStructures.GamesOwned result = null;
            try
            {
                json = webClient.DownloadString("https://embed.gog.com/user/data/games");                
                result = JsonConvert.DeserializeObject<GogDataStructures.GamesOwned>(json);
                MessageBox.Show(result.owned.Count.ToString() + " Games in account.");
            }
            catch (Exception e)
            {
                handleApiExceptions(e, json);
                return;
            }

            int skip = 0;
            int ok = 0;
            int j = 0;
            string currentGameId = "";
            int items = result.owned.Count;
            foreach (string gameId in result.owned)
            {
                try
                {
                    currentGameId = gameId;

                    GameImport gameFound = gamesFound.Find(x => x._gogId.Equals(gameId));
                    if (gameFound != null)
                    {
                        if (skipGogId.Contains(gameId) || skipTitle.Contains(gameFound.title))
                        {
                            gamesFound.Remove(gameFound);
                            skip++;
                        }
                        continue;
                    } else if (skipGogId.Contains(gameId))
                    {
                        skip++;
                        continue;
                    }


                    if (progress != null)
                    {
                        progress.Report((j * 100) / items);
                    }
                    j++;                    

                    json = webClient.DownloadString("https://embed.gog.com/account/gameDetails/"
                        + gameId.ToString() + ".json");

                    if (json.Equals("[]") || json.StartsWith("<")) continue;
                    GogDataStructures.GameDetails details = JsonConvert.DeserializeObject<GogDataStructures.GameDetails>(json);

                     if (skipTitle.Contains(details.title))
                    {
                        skip++;
                        continue;
                    }

                    // get aditional details
                    //json = webClient.DownloadString("http://api.gog.com/products/" 
                    //    + gameId.ToString() + "?expand=downloads,expanded_dlcs,description,screenshots,videos,related_products,changelog");
                    //GogDataStructures.MoreGameDetails moreDetails = JsonConvert.DeserializeObject<GogDataStructures.MoreGameDetails>(json);
                                       
                    List<GameDownload> downloads = new List<GameDownload>();

                    foreach (List<object> o in details.downloads)
                    {
                        string language = (string)o[0];
                        GogDataStructures.DownloadBySystem downloadsForLanguage =
                            JsonConvert.DeserializeObject<GogDataStructures.DownloadBySystem>(((JObject)o[1]).ToString());
                        List<GogDataStructures.Download> downloadsForWindows = downloadsForLanguage.windows;

                        foreach (GogDataStructures.Download d in downloadsForWindows)
                        {
                            downloads.Add(new GameDownload
                            {
                                _language = language,
                                _version = d.version,
                                _link = d.manualUrl,
                                _name = d.name
                            });
                        }
                    }

                    if (downloads.Count == 0) continue;

                    gamesFound.Add(new GameImport
                    {
                        _title = details.title,
                        _importTitle = details.title,
                        _import = true,
                        _gameDetails = details,
                        _downloads = downloads,
                        _selectedDownload = downloads[0],
                        _gogId = gameId
                    });
                    ok++;
                }
                catch (Exception e)
                {
                    if (e is JsonReaderException || e is JsonSerializationException)
                    {
                        MessageBox.Show("Error parsing game with id " + gameId + ".\n\n" +
                            e.GetType().ToString() + ": " + e.Message);
                    }
                    else
                    {
                        handleApiExceptions(e, null);
                    }
                    continue;
                }               
            }
            MessageBox.Show(ok.ToString() + "new games found, " + skip.ToString() + " skipped.");
        }

        //public string gameExists(GameImport gi)
        //{
        //    IGame duplicate = PluginHelper.DataManager.GetAllGames().ToList()
        //        .Find(x => x.GetAllCustomFields().ToList().Any(y => y.Name.Equals("gogId") && y.Value.Equals(gi._gogId)));
        //    if (duplicate != null)
        //    {
        //        return duplicate.ApplicationPath;
        //    } else
        //    {
        //        return null;
        //    }            
        //}

        //##################    Add games to collection    ##################

        public async void startImport()
        {
            ProgressForm progressForm = new ProgressForm();
            progressForm.Show();
            await progressForm.ShowProgress(gameImport_doWork, "Importing  Games");
            progressForm.Close();
            progressForm.Dispose();
        }

        public void gameImport_doWork(IProgress<int> progress)
        {
            int j = 0;
            int items = gamesFound.Count;
            PluginHelper.DataManager.ReloadIfNeeded();
            foreach (GameImport gi in gamesFound)
            {
                if (progress != null)
                {
                    progress.Report((j * 100) / items);
                }
                j++;
                if (!gi.import) continue;

                IGame game = PluginHelper.DataManager.AddNewGame(gi._importTitle);

                //ICustomField markedAsImported = game.AddNewCustomField();
                //markedAsImported.Name = "addedByGogPlugin";
                //markedAsImported.Value = "true";

                ICustomField gogId = game.AddNewCustomField();
                gogId.Name = "gogId";
                gogId.Value = gi._gogId;

                game.Status = "Imported from gog.com";
                game.Source = "gog.com";
                game.DateAdded = DateTime.Now;
                game.DateModified = DateTime.Now;
                game.Platform = this.platform;

                if (mode.Equals(Mode.startWithGalaxy))
                {
                    game.ApplicationPath = $@"{galaxyPath}\GalaxyClient.exe";
                    game.CommandLine= $@"/gameid={gi._gogId} /command=runGame";
                }
                else if (mode.Equals(Mode.linkToDownload))
                {
                    game.ApplicationPath = "https://embed.gog.com" + gi._selectedDownload._link;
                }
               
                game.Version = gi._selectedDownload._version + "(" + gi._selectedDownload._language.Substring(0, 2) + ")";
            }
            PluginHelper.DataManager.Save();            
        }

        //##################    Auxiliary functions    ##################

        public void BrowserClosed(object sender, FormClosedEventArgs e)
        {
            ((LoginBrowser)sender).Dispose();
        }

        public void ImportFormClosed(object sender, FormClosedEventArgs e)
        {
            if (mode.Equals(Mode.startWithGalaxy))
            {
                ((GameImportForm)sender).Dispose();
            }
            else if (mode.Equals(Mode.linkToDownload))
            {
                ((GameImportForm2)sender).Dispose();
            }
        }

        public void handleApiExceptions(Exception e, string json)
        {
            try
            {
                GogDataStructures.Error error = JsonConvert.DeserializeObject<GogDataStructures.Error>(json);
                if (error.error != null)
                {
                    MessageBox.Show(error.error + ": " + error.error_description);
                }
                else
                {
                    throw new Exception("Passing on exception");
                }
            } catch
            {
                MessageBox.Show(e.GetType().ToString() + ": " + e.Message + "\n\n" + e.StackTrace);
            }
        }
    }

}