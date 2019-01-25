/* Project : Wakencyclopedie
 * Description : Builder for Wakfu
 * Date : 13.12.2018
 * Author : GENGA Dario
 * © 2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using WakEncyclopedie.Database;

namespace WakEncyclopedie {
    public class EncyclopediaUpdater
    {
        private AllForWakfuDBDataContext AfwDC;
        #region url
        private const string ARMORS_URL = "https://www.wakfu.com/fr/mmorpg/encyclopedie/armures?page=1";
        private const string WEAPONS_URL = "https://www.wakfu.com/fr/mmorpg/encyclopedie/armes?page=1";
        private const string ACCESSORIES_URL = "https://www.wakfu.com/fr/mmorpg/encyclopedie/accessoires?page=1";
        private const string PETS_URL = "https://www.wakfu.com/fr/mmorpg/encyclopedie/familiers?page=1";
        private const string MOUNTS_URL = "https://www.wakfu.com/fr/mmorpg/encyclopedie/montures?page=1";
        public string[] ENCYCLOPEDIA_URL = new string[] { ARMORS_URL, WEAPONS_URL, ACCESSORIES_URL, PETS_URL, MOUNTS_URL };
        #endregion url
        #region className
        private const string TABLE_DATA_CLASS_NAME = "ak-table ak-responsivetable";
        private const string ITEM_CLASS_NAME_ODD = "ak-bg-odd";
        private const string ITEM_CLASS_NAME_EVEN = "ak-bg-even";
        private const string ITEM_RARITY_CLASS_NAME = "ak-icon-small";
        private const string ITEM_TYPE_CLASS_NAME = "item-type";
        private const string ITEM_LVL_CLASS_NAME = "item-level";
        private const string ITEM_ALL_CARACTERISTICS_CLASS_NAME = "item-caracteristics";
        private const string ITEM_CARACTERISTIC_CLASS_NAME = "ak-list-element";
        private const string ITEM_VALUE_CARACTERISTIC_CLASS_NAME = "ak-title";
        #endregion className
       
        private MainWindow Frm { get; set; }
        private DateTime Start { get; set; }

        public EncyclopediaUpdater(MainWindow frm)
        {
            AfwDC = new AllForWakfuDBDataContext();
            Start = DateTime.Now;
            Frm = frm;
        }

        /// <summary>
        /// Start the update of the databse
        /// </summary>
        public void StartUpdateDB()
        {
            DeleteAllItemsData();
            foreach (string url in ENCYCLOPEDIA_URL) {
                CreateNewBrowser(url);
            }
        }


        #region WebBrowser Methods
        /// <summary>
        /// Create a new Browser
        /// </summary>
        private void CreateNewBrowser(string url)
        {
            WebBrowser wb = new WebBrowser {
                ScriptErrorsSuppressed = true,
            };
            wb.Navigating += Browser_Navigating;
            wb.Navigate(new Uri(url));
        }

        /// <summary>
        /// Recovers data of items from the siteweb when the WebBrowser is going to navigate to a new browser document.
        /// </summary>
        private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;

            if (wb.DocumentText != "") {
                var tableItems = wb.Document.GetElementsByTagName("table");
                List<Dictionary<Items, List<Stat>>> listItem = new List<Dictionary<Items, List<Stat>>>();
                if (tableItems.Count > 0) {
                    foreach (HtmlElement table in tableItems) {
                        if (table.GetAttribute("className") == TABLE_DATA_CLASS_NAME) {
                            // <table> who contains all items
                            var trItems = table.GetElementsByTagName("tr");

                            foreach (HtmlElement tr in trItems) {
                                if (tr.GetAttribute("className") == ITEM_CLASS_NAME_ODD || tr.GetAttribute("className") == ITEM_CLASS_NAME_EVEN) {
                                    // <tr> who contains stats from an item
                                    Items actualItem = new Items();
                                    List<Stat> statsItem = new List<Stat>();
                                    Dictionary<Items, List<Stat>> itemWithStats = new Dictionary<Items, List<Stat>>();

                                    var spanTags = tr.GetElementsByTagName("span");
                                    var tdTags = tr.GetElementsByTagName("td");

                                    foreach (HtmlElement span in spanTags) {
                                        try {
                                            // first child -> <a>, "second" child -> <img>
                                            if (span.FirstChild.FirstChild.GetAttribute("alt") != "" && span.FirstChild.FirstChild != null) {
                                                // this span need to have two child (<a> & <img>) to get url, image and name of the item
                                                actualItem.name = span.FirstChild.FirstChild.GetAttribute("alt");
                                                actualItem.url = span.FirstChild.GetAttribute("href");
                                                // the id is stocked like this : .../24120-nameItem, so we ned to split it
                                                actualItem.Id = Convert.ToInt32(actualItem.url.Split('/').Last().Split('-').First());

                                                string urlImg = span.FirstChild.FirstChild.GetAttribute("src");
                                                // Create an image from the url
                                                using (WebClient wc = new WebClient()) {
                                                    actualItem.image = wc.DownloadData(urlImg);
                                                }
                                            }
                                        } catch (Exception) {
                                            // their is no "second" child, so we do nothing
                                        }

                                        if (span.GetAttribute("className").Split(' ')[0] == ITEM_RARITY_CLASS_NAME) {
                                            // the rarity is stocked like this : ak-icon-small ak-rarity-6, so we need to split it
                                            actualItem.idRarity = Convert.ToInt32(span.GetAttribute("className").Split('-').Last());
                                        }
                                    }

                                    foreach (HtmlElement td in tdTags) {
                                        if (td.GetAttribute("className") == ITEM_TYPE_CLASS_NAME) {
                                            actualItem.idType = GetIdTypeByTypeName(td.FirstChild.GetAttribute("title"));
                                        }

                                        if (td.GetAttribute("className") == ITEM_LVL_CLASS_NAME && td.InnerText != null) {
                                            // The lvl is stocked like this : Niv. 200, so we need this split it
                                            actualItem.level = Convert.ToInt32(td.InnerText.Split(' ').Last());
                                        }

                                        if (td.GetAttribute("className") == ITEM_ALL_CARACTERISTICS_CLASS_NAME) {
                                            foreach (HtmlElement div in td.GetElementsByTagName("div")) {
                                                if (div.GetAttribute("className") == ITEM_CARACTERISTIC_CLASS_NAME) {
                                                    foreach (HtmlElement divWithValue in div.GetElementsByTagName("div")) {
                                                        if (divWithValue.GetAttribute("className") == ITEM_VALUE_CARACTERISTIC_CLASS_NAME) {
                                                            // we have to remplace to delete the char '%' and then trim the text of stats for spliting it correctly
                                                            string[] stats = divWithValue.InnerText.Replace("%", "").Trim().Split(' ');

                                                            try {
                                                                int valueStat = Convert.ToInt32(stats[0]);
                                                                string typeStats = "";

                                                                for (int i = 1; i < stats.Count(); i++) {
                                                                    typeStats += stats[i] + " ";
                                                                }
                                                                statsItem.Add(new Stat(typeStats.Trim(), valueStat));
                                                            } catch (Exception) {
                                                                // An exception can occurs with special caracteristics
                                                                statsItem.Add(new Stat(divWithValue.InnerText.Trim(), -1));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    itemWithStats.Add(actualItem, statsItem);
                                    listItem.Add(itemWithStats);
                                }
                            }
                        }
                    }
                    UpdateDataFromDatabase(listItem);
                    string fullUrl = wb.Url.AbsoluteUri;
                    string url = fullUrl.Split('=')[0];
                    int nextPage = Convert.ToInt32(fullUrl.Split('=')[1]) + 1;
                    string nextUrl = url + "=" + nextPage.ToString();
                    wb.Navigate(new Uri(nextUrl));
                    CreateNewBrowser(nextUrl);
                }
                // TODO : indicate progress of upgrade
                wb.Dispose();
            }
        }
        #endregion WebBrowser Methods

        #region Database Methods
        /// <summary>
        /// Execute the queries for updating the database
        /// </summary>
        private void UpdateDataFromDatabase(List<Dictionary<Items, List<Stat>>> listItem)
        {
            foreach (Dictionary<Items, List<Stat>> dictionary in listItem) {
                foreach (KeyValuePair<Items, List<Stat>> item in dictionary) {
                    AfwDC.Items.InsertOnSubmit(item.Key);
                    foreach (Stat stat in item.Value) {
                        CreateStats(stat.Type, stat.Value, item.Key.Id);
                    }
                }
            }
            
            try {
                AfwDC.SubmitChanges();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        private void CreateStats(string typeStats, int valueStats, int idItem) {
            int idStat = VerifyIfStatsExist(typeStats);
            switch (idStat) {
                case 0:
                    Stats stat = new Stats {
                        type = typeStats
                    };
                    AfwDC.Stats.InsertOnSubmit(stat);
                    AfwDC.SubmitChanges();
                    idStat = stat.Id;
                    break;
                case -1:
                    // TODO: manage error
                    break;
                default:
                    // We already have the id of the stats
                    break;
            }
            AddStatItems(idItem, idStat, valueStats);
        }

        private void AddStatItems(int idItem, int idStats, int value)
        {
            Items_Have_Stats statsItem = new Items_Have_Stats {
                IdItem = idItem,
                IdStats = idStats,
                value = value
            };
            AfwDC.Items_Have_Stats.InsertOnSubmit(statsItem);
            try {
                AfwDC.SubmitChanges();
            } catch (Exception e) {
                Console.WriteLine(String.Format("Impossible d'ajouter la stat {0} de valeur {1} pour l'item {2}. Erreur : {3}", idStats, value, idItem, e.Message));
                AfwDC = new AllForWakfuDBDataContext();
            }
        }

        private int VerifyIfStatsExist(string typeStats)
        {
            return (from stats in AfwDC.Stats
                    where stats.type == typeStats
                    orderby stats.Id ascending
                    select stats.Id).SingleOrDefault(); //TODO: change first by single if we delete the stats before ?
        }

        /// <summary>
        /// Delete the data of each items of the database
        /// </summary>
        public void DeleteAllItemsData()
        {
            // First we have to delete the stats of each items
            List<Items_Have_Stats> items_have_stats = (from statsItems in AfwDC.Items_Have_Stats select statsItems).ToList();
            AfwDC.Items_Have_Stats.DeleteAllOnSubmit(items_have_stats);
            // Then we can delete all items
            List<Items> items = (from item in AfwDC.Items select item).ToList();
            AfwDC.Items.DeleteAllOnSubmit(items);

            AfwDC.SubmitChanges();

        }

        private int GetIdTypeByTypeName(string typeName) {
            return (from type in AfwDC.Type_Items
                    where type.name == typeName
                    select type.Id).Single();
        }

        #endregion DatabaseMethods
    }
}
