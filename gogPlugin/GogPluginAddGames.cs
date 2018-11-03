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

using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

using gogPlugin.forms;
using System.IO;
using System.Diagnostics;

namespace gogPlugin
{
    // TODO: Harmonize interface between plugin class and forms
    // TODO: split games found from games to import to avoid rescanning games not selected
    // FEATURE: save settings between imports
    // FEATURE: Implement options to download images and mauals
    // FEATURE: (De)select all on import
    // FEATURE: set games to ignore


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
        private ErrorForm errorForm = null;

        public enum Mode { startWithGalaxy, linkToDownload }
        public Mode mode = Mode.startWithGalaxy;
        
        public Dictionary<int, string> platforms;
        public int platformNo = 0;
        public string platform;

        public bool skipImported = false;
        public bool skipByTitle = false;

        public List<GameImport> gamesFound = new List<GameImport>();
        public List<String> skipGogId;
        public List<String> skipTitle;
        public string galaxyPath = null;

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

            // if platforms changed reset
            if (platform == null || !platforms[platformNo].Equals(platform))
            {
                platformNo = 0;
                platform = platforms[platformNo];
            }

            ImportOptions options = new ImportOptions(this);
            options.Show();
            await options.ok.Task;
            options.Close();
            this.skipByTitle = options.skipByTitle;
            this.skipImported = options.skipImported;
            this.mode = options.mode;
            this.galaxyPath = options.galaxyPath;
            this.platformNo = options.platformNo;
            this.platform = platforms[platformNo];
            options.Dispose();

            await Scan_Library();

            ProgressForm progressForm = new ProgressForm("Collecting Game Information");
            progressForm.Show();
            await progressForm.ShowProgress(GetGames_doWork);
            progressForm.Close();
            progressForm.Dispose();

            if (errorForm != null)
            {
                errorForm.Show();
                await errorForm.ok.Task;
                errorForm.Dispose();
            }

            if (gamesFound != null || gamesFound.Count > 0)            {

                gamesFound.Sort((x1, x2) => x1.title.CompareTo(x2.title));

                if (mode.Equals(Mode.startWithGalaxy))
                {
                    GameImportForm importForm = new GameImportForm(this);
                    importForm.Show();
                }
                else if (mode.Equals(Mode.linkToDownload))
                {
                    GameImportForm2 importForm = new GameImportForm2(this);
                    importForm.Show();
                }
            }

            webClient.Dispose();
        }

        //##################    Scaning Launchbox library for existing games    ##################

        private Task Scan_Library()
        {
            skipGogId = new List<string>();
            skipTitle = new List<string>();

            if (!(skipByTitle || skipImported)) return Task.CompletedTask;

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

        private void GetGames_doWork(IProgress<int> progress, IProgress<String> msg)
        {
            string json = null;

            // get ids of agmes owned
            GogDataStructures.GamesOwned userGames = null;
            try
            {
                json = webClient.DownloadString("https://embed.gog.com/user/data/games");
                userGames = JsonConvert.DeserializeObject<GogDataStructures.GamesOwned>(json);
            }
            catch (Exception e)
            {
                handleApiExceptions(e, json);
                return;
            }

            if (userGames == null || userGames.owned == null)
            {
                MessageBox.Show("Could not retrieve owned games.");
                return;
            }
            if (userGames.owned.Count == 0)
            {
                MessageBox.Show("No games found.");
                return;
            }

            // Debug Code: reduce number of games for faster testing
#if DEBUG
                reduceResults(userGames, 10);
#endif

            // get game info
            int noGamesOwned = userGames.owned.Count;
            int noGamesToScan = noGamesOwned - gamesFound.Count;
            int counter = 0;
            int skipped = 0;
            int failure = 0;

            string currentGameId = "";
            string currentGame = "Unknown";

            Dictionary<string, string> errorMessages = new Dictionary<string, string>();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Error = (sender, e) => handleParsingError(sender, e, errorMessages, currentGame),
                
            };

            foreach (string gameId in userGames.owned)
            {
                // report progress
                if (progress != null)
                {
                    progress.Report((counter * 100) / noGamesToScan);
                }

                currentGameId = gameId;

                // don't scan games already found
                GameImport gameFound = gamesFound.Find(x => x._gogId.Equals(gameId));
                if (gameFound != null)
                {
                    // if set to skip, also remove game from games already found
                    if (skipGogId.Contains(gameId) || skipTitle.Contains(gameFound.title))
                    {
                        gamesFound.Remove(gameFound);
                    }
                    continue;
                }
                counter++;

                // skip games by id
                if (skipGogId.Contains(gameId))
                {
                    skipped++;
                    continue;
                }

                GogDataStructures.GameDetails details = null;

                try
                {
                    json = webClient.DownloadString("https://embed.gog.com/account/gameDetails/"
                        + gameId.ToString() + ".json");

                    if (json == null || json.Equals("[]"))
                    {
                        appendError(errorMessages, "Game with Id " + gameId.ToString(), "No game information found (skip game).");
                        failure++;
                        continue;
                    }

                    // try to get game title
                    JsonTextReader reader = new JsonTextReader(new StringReader(json));
                    bool titleFound = false;
                    while (!titleFound)
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value != null && reader.Value.Equals("title"))
                        {
                            titleFound = true;
                            break;
                        }
                    }
                    reader.Read();
                    if (reader.TokenType != JsonToken.String || reader.Value == null || reader.Value.Equals(""))
                        titleFound = false;
                    if (!titleFound)
                    {
                        appendError(errorMessages, "Game with Id " + gameId.ToString(), "Invalid game information: game has no title (skip game).");
                        failure++;
                        continue;
                    }
                    else
                    {
                        currentGame = reader.Value.ToString();
                    }

                    // report curent game
                    string messageText = "Game " + (counter + 1).ToString() + " of " + noGamesOwned.ToString() + ": " + currentGame;
                    if (msg != null)
                    {
                        msg.Report(messageText);
                    }

                    // skip duplicate games by title
                    if (skipTitle.Contains(currentGame))
                    {
                        skipped++;
                        continue;
                    }

                    // parse game details
                    details = JsonConvert.DeserializeObject<GogDataStructures.GameDetails>(json, settings);
                }
                catch (Exception e)
                {
                    if (e is JsonReaderException || e is JsonSerializationException)
                    {
                        appendError(errorMessages, currentGame, "Error parsing game with id " + gameId + " (skip game): " + e.ToString());
                        failure++;
                        continue;
                    }
                    else if (e is NullReferenceException)
                    {
                        appendError(errorMessages, currentGame, "Error parsing object " + e.Source + " (skip game).");
                        failure++;
                        continue;
                    }
                }

                if (details == null)
                {
                    appendError(errorMessages, currentGame, "Could not retrieve game details (skip game).");
                    failure++;
                    continue;
                }

                Debug.Assert(details.title.Equals(currentGame));

                // get aditional details
                //json = webClient.DownloadString("http://api.gog.com/products/" 
                //    + gameId.ToString() + "?expand=downloads,expanded_dlcs,description,screenshots,videos,related_products,changelog");
                //GogData.MoreGameDetails moreDetails = JsonConvert.DeserializeObject<GogData.MoreGameDetails>(json);

                // get Downloads
                List<GameDownload> downloads = null;
                if (mode.Equals(Mode.linkToDownload))
                {
                    downloads = getDownloads(details);
                    if (downloads == null || downloads.Count == 0)
                    {
                        appendError(errorMessages, currentGame, "No Downloads found (skip game).");
                        failure++;
                        continue;
                    }
                }

                // add game to list of found games (stored during launchbox session)
                gamesFound.Add(new GameImport
                {
                    _title = currentGame,
                    _importTitle = details.title,
                    _import = true,
                    _gameDetails = details,
                    _downloads = downloads,
                    _selectedDownload = downloads[0] ?? null,
                    _gogId = gameId
                });
            }

            // show errors
            if (errorMessages.Count > 0)
            {
                this.errorForm = new ErrorForm(userGames.owned.Count.ToString() + " games found, " + gamesFound.Count.ToString() + " ready for import, " + skipped.ToString() + " skipped, "
                    + failure.ToString() + " failed to scan, " + (errorMessages.Count - failure).ToString() + " scanned with errors.", errorMessages);
            }
            else
            {
                MessageBox.Show(gamesFound.Count.ToString() + " games found, " + skipped.ToString() + " skipped.");
            }
        }

            
        private static void reduceResults(GogDataStructures.GamesOwned result, int denominator)
        {
            List<String> idsSelected = result.owned.FindAll(x => result.owned.IndexOf(x) % denominator == 0);
            result.owned = idsSelected;
        }

        //##################    Add games to collection    ##################

        private List<GameDownload> getDownloads(GogDataStructures.GameDetails details)
        {
            if (details.downloads == null) return null;

            List<GameDownload> downloads = new List<GameDownload>();

            foreach (List<object> downloadObject in details.downloads)
            {
                string language = (string)downloadObject[0];
                GogDataStructures.DownloadBySystem downloadsForLanguage =
                    JsonConvert.DeserializeObject<GogDataStructures.DownloadBySystem>(((JObject)downloadObject[1]).ToString());

                if (downloadsForLanguage == null) return null;                
                List<GogDataStructures.Download> downloadsForWindows = downloadsForLanguage.windows;
                if (downloadsForWindows == null) return null;

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
            return downloads;
        }


        public async void startImport()
        {
            ProgressForm progressForm = new ProgressForm("Importing  Games");
            progressForm.Show();
            await progressForm.ShowProgress(gameImport_doWork);
            progressForm.Close();
            progressForm.Dispose();
        }

        private void gameImport_doWork(IProgress<int> progress, IProgress<String> msg)
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

                if (gi.title != null)
                {
                    msg.Report(gi.title);
                }

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
                game.Platform = this.platforms[platformNo];

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

        private void handleApiExceptions(Exception e, string json)
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
                    throw e;
                }
            } catch
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void handleParsingError(object sender, ErrorEventArgs args, Dictionary<string,string> errorMessages, string currentGame)
        {
            // only handle error once
            if (args.CurrentObject != args.ErrorContext.OriginalObject) return;

            string errorMessage = "Error trying to deserialize " + args.CurrentObject.ToString() + ": " + args.ErrorContext.Error.Message;
            appendError(errorMessages, currentGame, errorMessage);
            args.ErrorContext.Handled = true;
        }

        private static void appendError(Dictionary<string, string> errorMessages, string currentGame, string errorMessage)
        {
            if (errorMessages.Keys.Contains(currentGame) && errorMessages[currentGame] != null)
            {
                errorMessages[currentGame] = errorMessages[currentGame] + "\n" + errorMessage;
            }
            else
            {
                errorMessages.Add(currentGame, errorMessage);
            }
        }
    }

}