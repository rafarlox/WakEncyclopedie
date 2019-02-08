using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Database;

namespace WakEncyclopedie {
    public class EncyclopediaDB {
        public const string COLUMN_ITEM_NAME = "Name";
        public const string COLUMN_ITEM_LEVEL = "Level";
        public const string COLUMN_ITEM_RARITY = "RarityName";
        public const int ITEMS_TO_LOAD = 30;
        private int NbItemsDisplayed { get; set; }
        private const int MIN_PAGE_ITEMS = 1;
        private const int INCREMENT_PAGE_ITEMS = 1;
        private const int DECREMENT_PAGE_ITEMS = 1;
        private int _actualPageItems = MIN_PAGE_ITEMS;
        private dynamic PreviousQuery { get; set; }
        private AllForWakfuDBDataContext afwDC;

        public EncyclopediaDB()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            afwDC = new AllForWakfuDBDataContext(string.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0}Database\AllForWakfuDB.mdf;Integrated Security=True;Connect Timeout=30", currentDirectory));
            NbItemsDisplayed = ITEMS_TO_LOAD;
        }

        private int ActualPageItems
        {
            get => _actualPageItems;
            set
            {
                if (value < MIN_PAGE_ITEMS)
                {
                    value = MIN_PAGE_ITEMS;
                }
                _actualPageItems = value;
            }
        }
        #region paginations_methods
        /// <summary>
        /// Use pagination for the query
        /// </summary>
        private dynamic ExecuteQueryWithPagination()
        {
            return PagingExtensions.Page(PreviousQuery, ActualPageItems, NbItemsDisplayed);
        }

        public dynamic LoadNewItems() {
            NbItemsDisplayed += ITEMS_TO_LOAD;
            return ExecuteQueryWithPagination();
        }

        private dynamic ResetItemsDisplayed() {
            NbItemsDisplayed = ITEMS_TO_LOAD;
            return ExecuteQueryWithPagination();
        }

        public dynamic GoToNextItems()
        {
            ActualPageItems += INCREMENT_PAGE_ITEMS;
            return ExecuteQueryWithPagination();
        }

        public dynamic GoToPreviousItems()
        {
            ActualPageItems -= DECREMENT_PAGE_ITEMS;
            return ExecuteQueryWithPagination();
        }
        #endregion paginations_methods

        public Item GetItemById(int idItem) {
            return (from items in afwDC.Items
                    join rarity in afwDC.Rarity
                    on items.idRarity equals rarity.Id
                    join typeItems in afwDC.Type_Items
                    on items.idType equals typeItems.Id
                    where items.Id == idItem
                    select new Item {
                        Id = items.Id,
                        Name = items.name,
                        Level = items.level,
                        Image = items.image.ToArray(),
                        Url = items.url,
                        IdType = typeItems.Id,
                        Type = typeItems.name,
                        TypeImage = typeItems.image.ToArray(),
                        IdRarity = items.idRarity,
                        RarityName = rarity.name,
                        RarityImage = rarity.image.ToArray(),
                        StatList = GetStatsOfItem(items.Id),
                    }).Single();
        }

        public dynamic GetAllItemsWithImg()
        {
            ActualPageItems = MIN_PAGE_ITEMS;
            // join the tables of foreign keys to get a value that correspond to what we want
            PreviousQuery = from items in afwDC.Items
                            join rarity in afwDC.Rarity
                            on items.idRarity equals rarity.Id
                            join typeItems in afwDC.Type_Items
                            on items.idType equals typeItems.Id
                            orderby items.Id ascending
                            select new Item {
                                Id = items.Id,
                                Name = items.name,
                                Level = items.level,
                                Image = items.image.ToArray(),
                                Url = items.url,
                                IdType = typeItems.Id,
                                Type = typeItems.name,
                                TypeImage = typeItems.image.ToArray(),
                                IdRarity = items.idRarity,
                                RarityName = rarity.name,
                                RarityImage = rarity.image.ToArray(),
                                StatList = GetStatsOfItem(items.Id),
                            };
            return ResetItemsDisplayed();
        }

        public List<Stat> GetStatsOfItem(int idItem)
        {
            return (from itemsStats in afwDC.Items_Have_Stats
                    join stats in afwDC.Stats
                    on itemsStats.IdStats equals stats.Id
                    where itemsStats.IdItem == idItem
                    select new Stat
                    {
                        Id = stats.Id,
                        Type = stats.type,
                        Value = itemsStats.value,
                    }).ToList();
        }

        public dynamic GetAllRarities()
        {
            return from rarity in afwDC.Rarity
                   orderby rarity.Id descending
                   select new Element
                   {
                       Id = rarity.Id,
                       Name = rarity.name,
                       Img = rarity.image.ToArray(),
                   };
        }

        public dynamic GetAllTypes()
        {
            return from type in afwDC.Type_Items
                   orderby type.Id ascending
                   select new Element
                   {
                       Id = type.Id,
                       Name = type.name,
                       Img = type.image.ToArray(),
                   };
        }

        public dynamic GetAllStats()
        {
            return from stats in afwDC.Stats
                   where stats.Id >= 2 && stats.Id <= 41
                   orderby stats.Id ascending
                   select new Element
                   {
                       Id = stats.Id,
                       Name = stats.type,
                   };
        }

        public dynamic SearchItems(int lvlMin, int lvlMax, string itemName = "", int[] idRarities = null, int[] idTypes = null, int[] idStats = null)
        {
            // reset the pagination
            ActualPageItems = MIN_PAGE_ITEMS;

            // Get all items with the good level
            var itemsList = from items in afwDC.Items
                            where items.level >= lvlMin
                            where items.level <= lvlMax
                            select items;

            // Filter the items by name
            if (!string.IsNullOrEmpty(itemName))
            {
                itemsList = (from items in itemsList select items).Where(x => x.name.Contains(itemName));
            }

            // Filter the items by rarities
            if (idRarities.Count() > 0)
            {
                itemsList = (from items in itemsList
                             join rarity in afwDC.Rarity
                             on items.idRarity equals rarity.Id
                             select items).Where(x => idRarities.Contains(x.idRarity));
            }

            // Filter the items by type
            if (idTypes.Count() > 0)
            {
                itemsList = (from items in itemsList
                             join type in afwDC.Type_Items
                             on items.idType equals type.Id
                             select items).Where(x => idTypes.Contains(x.idType));
            }

            // Filter the items by stats
            if (idStats.Count() > 0)
            {
                // Select items that contains the id of stats wished, grouped by item id and converted into a list
                List<List<Items>> groupListItems = (from statsItem in afwDC.Items_Have_Stats
                                                    join items in itemsList
                                                    on statsItem.IdItem equals items.Id
                                                    join stats in afwDC.Stats
                                                    on statsItem.IdStats equals stats.Id
                                                    orderby statsItem.IdItem
                                                    where idStats.Contains(statsItem.IdStats) && statsItem.value > 0
                                                    select items).GroupBy(x => x.Id).Select(grp => grp.ToList()).ToList();


                // Transform the grouped list into a simple list of items
                // NOTE: factoring this code by SelectMany() methods after resolving duplicate items issues
                List<Items> listItems = new List<Items>();
                foreach (List<Items> item in groupListItems)
                {
                    listItems.Add(item[0]);
                }

                // Create a dictionary that contains the id of each items and with a list that contains the id of each stats from this item
                Dictionary<int, List<int>> itemDictionaryWithStatsList = new Dictionary<int, List<int>>();
                foreach (Items items in listItems)
                {
                    List<int> idStatList = (from statsItem in afwDC.Items_Have_Stats
                                            where statsItem.IdItem == items.Id
                                            select statsItem.IdStats).Distinct().ToList();

                    itemDictionaryWithStatsList.Add(items.Id, idStatList);
                }


                // Create an array of the dictionary that we will be used for the foreach
                var itemArrayWithStats = itemDictionaryWithStatsList.ToArray();


                foreach (int stat in idStats)
                {
                    // foreach element (item) of our array we verify if it possesses the stat searched
                    foreach (var element in itemArrayWithStats)
                    {
                        bool statOk = false;
                        // Browse each stats of each item to find the id that correspond to the search
                        foreach (int elementStat in element.Value)
                        {
                            // If the id match, then this item has the good stat and we can stop browsing it
                            if (elementStat == stat)
                            {
                                statOk = true;
                                break;
                            }
                        }
                        // If the stat is not found in the item, then we remove it from the dictionary
                        if (!statOk)
                        {
                            itemDictionaryWithStatsList.Remove(element.Key);
                        }
                    }
                }

                // Can't use a Join between a SQL source and a local source. So we need to bring the SQL data into memory before.
                // Select the items that contains an id from the dictionary
                itemsList = itemsList.Where(i => itemDictionaryWithStatsList.Keys
                                     .Select(x => x)
                                     .Contains(i.Id));

            }

            // Create the query of the items with the correct data structure
            PreviousQuery = from items in itemsList
                            join rarity in afwDC.Rarity
                            on items.idRarity equals rarity.Id
                            join typeItems in afwDC.Type_Items
                            on items.idType equals typeItems.Id
                            orderby items.Id ascending
                            select new Item
                            {
                                Id = items.Id,
                                Name = items.name,
                                Level = items.level,
                                Image = items.image.ToArray(),
                                Url = items.url,
                                IdType = typeItems.Id,
                                Type = typeItems.name,
                                TypeImage = typeItems.image.ToArray(),
                                IdRarity = items.idRarity,
                                RarityName = rarity.name,
                                RarityImage = rarity.image.ToArray(),
                                StatList = GetStatsOfItem(items.Id),
                            };

            return ResetItemsDisplayed();
        }

        public dynamic SortPreviousSearch(string column, ListSortDirection sortDirection) {
            IQueryable<Item> query = PreviousQuery;
            switch (column) {
                case COLUMN_ITEM_NAME:
                    PreviousQuery = (sortDirection == ListSortDirection.Ascending) ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                    break;
                case COLUMN_ITEM_LEVEL:
                    PreviousQuery = (sortDirection == ListSortDirection.Ascending) ? query.OrderBy(x => x.Level) : query.OrderByDescending(x => x.Level);
                    break;
                case COLUMN_ITEM_RARITY:
                    PreviousQuery = (sortDirection == ListSortDirection.Ascending) ? query.OrderBy(x => x.IdRarity) : query.OrderByDescending(x => x.IdRarity);
                    break;
                default:
                    Console.WriteLine($"Impossible to sort the column {column}");
                    break;
            }
            return ResetItemsDisplayed();
        }


        /// <summary>
        /// Get all id item and id stats that correspond to the array of stats id (grouped by item id)
        /// </summary>
        /// <param name="itemsList">Actuel list of items that we have to filter</param>
        /// <param name="idStats">The id of stats that will be used to filter the list</param>
        /// <returns></returns>
        private dynamic SearchItemsByOneOrAllStats(IQueryable<Items> itemsList, int[] idStats)
        {
            return (from statsItem in afwDC.Items_Have_Stats
                    join items in itemsList
                    on statsItem.IdItem equals items.Id
                    join stats in afwDC.Stats
                    on statsItem.IdStats equals stats.Id
                    orderby statsItem.IdItem
                    where idStats.Contains(statsItem.IdStats)
                    select statsItem).GroupBy(x => x.IdItem);
        }
    }
}
