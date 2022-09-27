using System.Collections.Generic;
using System.Linq;

class Processor
{
    enum LogEnum
    {
        LogEnabled,
        LogDisabled
    }

    struct MarketNode
    {
        public string name;
        public int value;
    };

    private static void CalcMarketSizes(GameState gs, ProcessedData pd, string outPath)
    {
        List<string> csvLines = new();
        csvLines.Add($"MarketIndex, TotalSellPrice (in mil), TotalBuyPrice (in mil)");
        foreach (int i in Enum.GetValues(typeof(MarketIndex)))
        {
            MarketIndex mi = (MarketIndex)i;
            CalcMarketSize(mi, gs, pd, ref csvLines);
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"MarketSize.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"MarketSize.csv"), csvLines);
    }

    private static void CalcBuildingNameMap(GameState gs, string outPath)
    {
        List<string> csvBuildingTickets = new List<string>();
        csvBuildingTickets.Add("Building Name, Building Ticket");
        foreach (var x in gs.BuildingsNodes)
        {
            csvBuildingTickets.Add($"{x.Name}, {x.Ticker}");
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"buildingNameMap.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"buildingNameMap.csv"), csvBuildingTickets);
    }

    private static void CalcNumLocalMarketOrdersPerPlanet(GameState gs, string outPath)
    {
        Dictionary<string, int> numMarketOrdersOnPlanet = new Dictionary<string, int>();
        foreach (var node in gs.LocalMarketBuyNodes)
        {
            int value;
            if (!numMarketOrdersOnPlanet.TryGetValue(node.PlanetNaturalId, out value))
            {
                value = 0;
            }
            value++;
            numMarketOrdersOnPlanet[node.PlanetNaturalId] = value;
        }

        foreach (var node in gs.LocalMarketSellNodes)
        {
            int value;
            if (!numMarketOrdersOnPlanet.TryGetValue(node.PlanetNaturalId, out value))
            {
                value = 0;
            }
            value++;
            numMarketOrdersOnPlanet[node.PlanetNaturalId] = value;
        }

        List<MarketNode> nodes = new List<MarketNode>();
        foreach (var market in numMarketOrdersOnPlanet)
        {
            nodes.Add(new MarketNode()
            {
                name = market.Key,
                value = market.Value
            });
        }

        nodes.Sort((a, b) => b.value.CompareTo(a.value));

        List<string> csvLines = new();
        csvLines.Add($"MarketName, NumMarketOrders");
        foreach (var market in nodes)
        {
            csvLines.Add($"{market.name}, {market.value}");
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"LocalMarkets-NumOrders.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"LocalMarkets-NumOrders.csv"), csvLines);
    }

    private static void CalcMarketOrders(MarketIndex mi, GameState gs, ProcessedData pd, LogEnum log)
    {
        foreach (var buyNode in gs.LocalMarketBuyNodes)
        {
            LocalMarketBuySellNodeProcessed lm = new();
            lm.PlanetNaturalId = buyNode.PlanetNaturalId;
            lm.CreatorCompanyName = buyNode.CreatorCompanyName;
            lm.CreatorCompanyCode = buyNode.CreatorCompanyCode;
            lm.MaterialName = buyNode.MaterialName;
            lm.MaterialTicker = buyNode.MaterialTicker;
            lm.MaterialWeight = float.Parse(buyNode.MaterialWeight);
            lm.MaterialVolume = float.Parse(buyNode.MaterialVolume);
            lm.MaterialAmount = int.Parse(buyNode.MaterialAmount);
            lm.Price = float.Parse(buyNode.Price);
            lm.PriceCurrency = buyNode.PriceCurrency;
            lm.DeliveryTime = int.Parse(buyNode.DeliveryTime);
            lm.CreationTimeEpochMs = long.Parse(buyNode.CreationTimeEpochMs);
            lm.ExpiryTimeEpochMs = long.Parse(buyNode.ExpiryTimeEpochMs);
            lm.MinimumRating = buyNode.MinimumRating;
            float price = float.Parse(buyNode.Price);
            lm.matMarketPrice = pd.buyPrices[(int)mi][buyNode.MaterialTicker];
            lm.costToBuyFromMarket = lm.matMarketPrice * lm.MaterialAmount;
            lm.profit = price - lm.costToBuyFromMarket;

            if (lm.profit > 0)
            {
                if( log == LogEnum.LogEnabled )
                {
                    Console.WriteLine($"BUY: {buyNode.PlanetNaturalId} {buyNode.MaterialAmount} {buyNode.MaterialTicker} for {buyNode.Price} in {buyNode.DeliveryTime} days.  Profit:{lm.profit}");
                }
                pd.localMarketBuySellNodeProcessed.Add(lm);
            }
        }

        foreach (var sellNode in gs.LocalMarketSellNodes)
        {
            LocalMarketBuySellNodeProcessed lm = new();
            lm.PlanetNaturalId = sellNode.PlanetNaturalId;
            lm.CreatorCompanyName = sellNode.CreatorCompanyName;
            lm.CreatorCompanyCode = sellNode.CreatorCompanyCode;
            lm.MaterialName = sellNode.MaterialName;
            lm.MaterialTicker = sellNode.MaterialTicker;
            lm.MaterialWeight = float.Parse(sellNode.MaterialWeight);
            lm.MaterialVolume = float.Parse(sellNode.MaterialVolume);
            lm.MaterialAmount = int.Parse(sellNode.MaterialAmount);
            lm.Price = float.Parse(sellNode.Price);
            lm.PriceCurrency = sellNode.PriceCurrency;
            lm.DeliveryTime = int.Parse(sellNode.DeliveryTime);
            lm.CreationTimeEpochMs = long.Parse(sellNode.CreationTimeEpochMs);
            lm.ExpiryTimeEpochMs = long.Parse(sellNode.ExpiryTimeEpochMs);
            lm.MinimumRating = sellNode.MinimumRating;
            float price = float.Parse(sellNode.Price);
            lm.matMarketPrice = pd.sellPrices[(int)mi][sellNode.MaterialTicker];
            lm.gainedFromSellToMarket = lm.matMarketPrice * lm.MaterialAmount;
            lm.profit = lm.gainedFromSellToMarket - price;

            if (lm.profit > 0)
            {
                if (log == LogEnum.LogEnabled)
                {
                    Console.WriteLine($"SELL: {sellNode.PlanetNaturalId} {sellNode.MaterialAmount} {sellNode.MaterialTicker} for {sellNode.Price} in {sellNode.DeliveryTime} days.  Profit:{lm.profit}");
                }
                pd.localMarketBuySellNodeProcessed.Add(lm);
            }
        }
    }

    private static void LogPlanetsNear(GameState gs)
    {
        List<ConnectionsNode> nodes = GetStarsNear("YI-715", 5, gs);
        List<PlanetDetailNode> planets = GetPlanetsWithLocalMarkets(nodes, gs);
        foreach (var n in planets)
        {
            Console.WriteLine($"{n.PlanetNaturalId}");
        }
    }

    private static void CalcPlanetsNearMarkets(GameState gs, int dist, string outPath)
    {
        // Log all the planets with 5 of each of the commodity markets
        List<ConnectionsNode> nodes1 = GetStarsNear("ZV-307", 5, gs); // AI1
        List<ConnectionsNode> nodes2 = GetStarsNear("AM-783", 5, gs); // CI2
        List<ConnectionsNode> nodes3 = GetStarsNear("UV-351", 5, gs); // CI1
        List<ConnectionsNode> nodes4 = GetStarsNear("VH-331", 5, gs); // IC1
        List<ConnectionsNode> nodes5 = GetStarsNear("TD-203", 5, gs); // NC2
        List<ConnectionsNode> nodes6 = GetStarsNear("OT-580", 5, gs); // NC1
        var allStarsNearMarkets = nodes1
            .Concat(nodes2)
            .Concat(nodes3)
            .Concat(nodes4)
            .Concat(nodes5)
            .Concat(nodes6)
            .Distinct()
            .ToList();

        List<PlanetDetailNode> planets = GetPlanetsWithLocalMarkets(allStarsNearMarkets, gs);
        List<string> csvLines = new();
        foreach (var n in planets)
        {
            csvLines.Add($"{n.PlanetNaturalId}");
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"planetsNearMarkets.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"planetsNearMarkets.csv"), csvLines);
    }

    public static float ConvertMsToHours(float timeInMs)
    {
        float numSec = timeInMs / 1000.0f;
        float numMin = numSec / 60.0f;
        float numHours = numMin / 60.0f;
        return numHours;
    }

    private static void CalcDailyWorkerCost(MarketIndex mi, ProcessedData pd, LogEnum log)
    {
        pd.dailyCostForWorkers = 0.0f;
        foreach (var mat in pd.dailyNeedMats)
        {
            float price = pd.buyPrices[(int)mi][mat.Mat];
            pd.dailyCostForWorkers += price * mat.Amount;
        }

        if (log == LogEnum.LogEnabled)
        {
            Console.WriteLine($"DailyCostForWorkers: {pd.dailyCostForWorkers}");
        }
    }

    private static string GetIsBuildingStarter(string buildingTicker)
    {
        string buildingStarter = "";
        if (buildingTicker.Contains("BMP"))
        {
            buildingStarter = "Manu/Const";
        }
        else if (buildingTicker.Contains("COL"))
        {
            buildingStarter = "Manu/Fuel";
        }
        else if (buildingTicker.Contains("EXT"))
        {
            buildingStarter = "Metal/Fuel";
        }
        else if (buildingTicker.Contains("FRM"))
        {
            buildingStarter = "Vict/Carbon";
        }
        else if (buildingTicker.Contains("FP"))
        {
            buildingStarter = "Vict";
        }
        else if (buildingTicker.Contains("INC"))
        {
            buildingStarter = "Manu/Carbon";
        }
        else if (buildingTicker.Contains("PP1"))
        {
            buildingStarter = "Const";
        }
        else if (buildingTicker.Contains("REF"))
        {
            buildingStarter = "Fuel";
        }
        else if (buildingTicker.Contains("RIG"))
        {
            buildingStarter = "Vict";
        }
        else if (buildingTicker.Contains("SME"))
        {
            buildingStarter = "Metal";
        }
        return buildingStarter;
    }

    private static List<string> LogMostProfitReceipes(MarketIndex mi, List<ReceipeNode> receipeNode, ProcessedData pd, GameState gs)
    {
        List<ReceipeNode> receiptForProfit = new();
        for (int i = 0; i < receipeNode.Count; i++)
        {
            var receipe = receipeNode[i];
            if (receipe.Profit > 0 && receipe.TotalCostOutputToSell > 0 && receipe.CostInputToMake >= 0)
            {
                receiptForProfit.Add(receipe);
            }
        }
        receiptForProfit.Sort((a, b) => b.ProfitPerHour.CompareTo(a.ProfitPerHour));

        List<string> csvReceipeLines = new List<string>();
        for (int i = 0; i < receiptForProfit.Count; i++)
        {
            var receipe = receiptForProfit[i];

            var prod = gs.Productions.FirstOrDefault(r =>
            {
                var buildingNode = gs.BuildingsNodes.First(x => x.Name == r.Type);
                return buildingNode.Ticker == receipe.BuildingTicker;
            });
            var hasBuilding = !string.IsNullOrEmpty(prod.PlanetId);
            string buildingStarter = GetIsBuildingStarter(receipe.BuildingTicker);

            var building = pd.buildingCosts.First(r => r.Building == receipe.BuildingTicker);
            float hoursToMake = ConvertMsToHours(receipe.TimeMs);
            csvReceipeLines.Add(
                $"{mi}, {receipe.RecipeName}, {receipe.CostInputToMake:0}, {receipe.CostInputToBuy:0}, {receipe.TotalCostOutputToSell:0}, {receipe.Profit:0}, {receipe.ProfitPerHour:0}, {building.Building}, " +
                $"{building.Pioneers}, {building.Settlers}, {building.Technicians}, {building.Engineers}, {building.Scientists}, {building.CostToBuy}, {buildingStarter}, {hoursToMake}, {receipe.MaxProfitOnMarket:0}");
        }

        return csvReceipeLines;
    }

    private static float CalcMaxProfitOnMarket(MarketIndex mi, GameState gs, ProcessedData pd, ReceipeNode receipe)
    {
        float totalProfit = 0.0f;
        string miStr = mi.ToString();
        foreach (var outputItem in receipe.Outputs)
        {
            foreach (var order in gs.RequestsToBuyNodesLatest)
            {
                if (order.ExchangeCode != miStr)
                {
                    continue;
                }

                if (order.MaterialTicker == outputItem.Ticker)
                {
                    float unitCostToMake = receipe.CostInputToMake / outputItem.Amount;                                        
                    float itemCost = float.Parse(order.ItemCost);
                    float profitPerItem = itemCost - unitCostToMake;
                    if (profitPerItem > 0)
                    {
                        float itemCount = float.Parse(order.ItemCount);
                        float profitOnOrder = profitPerItem * itemCount;
                        totalProfit += profitOnOrder;
                    }
                }
            }
        }

        return totalProfit;
    }

    private static void CalcRecipeCosts(MarketIndex mi, GameState gs, List<ReceipeNode> receipeNodes, ProcessedData pd)
    {
        pd.makeCostNodes = new List<MakeCostNode>();
        for (int i = 0; i < receipeNodes.Count; i++)
        {
            var receipe = receipeNodes[i];
            foreach (var item in receipe.Outputs)
            {
                float price = pd.sellPrices[(int)mi][item.Ticker];
                float priceAmt = price * item.Amount;

                pd.makeCostNodes.Add(new MakeCostNode()
                {
                    TimeMs = receipe.TimeMs,
                    Ticker = item.Ticker,
                    Amount = item.Amount,
                    MakeReceipeIndex = i,
                    CostOutputToSell = priceAmt,
                });
            }
        }

        for (int i = 0; i < receipeNodes.Count; i++)
        {
            var receipe = receipeNodes[i];
            receipe.CostInputToBuy = 0.0f;

            receipe.CanMakeInput = false;
            receipe.CostInputToMake = 0.0f;

            foreach (var item in receipe.Inputs)
            {
                float price = pd.buyPrices[(int)mi][item.Ticker];
                float priceAmt = price * item.Amount;
                receipe.CostInputToBuy += priceAmt;

                MakeCostNode makeNode = pd.makeCostNodes.FirstOrDefault(r => r.Ticker == item.Ticker);
                if (makeNode.Ticker == item.Ticker)
                {
                    receipe.CanMakeInput = true;
                }
            }

            receipe.TotalCostOutputToSell = 0.0f;
            foreach (var item in receipe.Outputs)
            {
                float price = pd.sellPrices[(int)mi][item.Ticker];
                float priceAmt = price * item.Amount;
                receipe.TotalCostOutputToSell += priceAmt;
            }
        }

        for (int i = 0; i < receipeNodes.Count; i++)
        {
            var receipe = receipeNodes[i];
            UpdateCostToMakeReceipe(mi, receipeNodes, pd, receipe);
            receipe.Profit = receipe.TotalCostOutputToSell - receipe.CostInputToMake;
            float numHours = ConvertMsToHours(receipe.TimeMs);
            receipe.ProfitPerHour = receipe.Profit / numHours;
            receipe.MaxProfitOnMarket = CalcMaxProfitOnMarket(mi, gs, pd, receipe);
        }
    }

    private static void UpdateCostToMakeReceipe(MarketIndex mi, List<ReceipeNode> receipeNodeOwned, ProcessedData pd, ReceipeNode receipe)
    {
        if (receipe.CanMakeInput)
        {
            if(receipe.CostInputToMake > 0)
            {
                return;
            }

            receipe.CostInputToMake = 0.0f;
            foreach (var item in receipe.Inputs)
            {
                MakeCostNode makeNode = pd.makeCostNodes.FirstOrDefault(r => r.Ticker == item.Ticker);
                if (makeNode.Ticker == item.Ticker)
                {
                    var receipe2 = receipeNodeOwned[makeNode.MakeReceipeIndex];
                    UpdateCostToMakeReceipe(mi, receipeNodeOwned, pd, receipe2);
                    receipe.CostInputToMake += receipe2.CostInputToMake;
                }
                else
                {
                    float price = pd.buyPrices[(int)mi][item.Ticker];
                    float priceAmt = price * item.Amount;
                    receipe.CostInputToMake += priceAmt;
                }
            }
        }
        else
        {
            receipe.CostInputToMake = receipe.CostInputToBuy;
        }
    }

    private static void GetDailyNeedMats(GameState gs, ProcessedData pd)
    {
        foreach (var node in gs.WorkforceNodes)
        {
            if (node.MaterialTicker == "COF" ||
                node.MaterialTicker == "PWO")
            {
                continue;
            }

            pd.dailyNeedMats.Add(new ProcessedDataMat()
            {
                Mat = node.MaterialTicker,
                Amount = float.Parse(node.DailyAmount)
            });
        }
    }

    private static void UpdateStockPrices(GameState gs, ProcessedData pd)
    {
        for (int i = 0; i < 6; i++)
        {
            pd.buyPrices[i] = new Dictionary<string, float>();
            pd.sellPrices[i] = new Dictionary<string, float>();
        }

        foreach (var node in gs.PricesNodesLatest)
        {
            float price;

            // AI1
            if (!float.TryParse(node.AI1_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.AI1].Add(node.Ticker, price);
            if (!float.TryParse(node.AI1_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.AI1].Add(node.Ticker, price);

            // CI1
            if (!float.TryParse(node.CI1_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.CI1].Add(node.Ticker, price);
            if (!float.TryParse(node.CI1_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.CI1].Add(node.Ticker, price);

            // CI2
            if (!float.TryParse(node.CI2_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.CI2].Add(node.Ticker, price);
            if (!float.TryParse(node.CI2_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.CI2].Add(node.Ticker, price);

            // NC1
            if (!float.TryParse(node.NC1_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.NC1].Add(node.Ticker, price);

            if (!float.TryParse(node.NC1_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.NC1].Add(node.Ticker, price);

            // NC2
            if (!float.TryParse(node.NC2_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.NC2].Add(node.Ticker, price);
            if (!float.TryParse(node.NC2_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.NC2].Add(node.Ticker, price);

            // IC1
            if (!float.TryParse(node.IC1_AskPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.buyPrices[(int)MarketIndex.IC1].Add(node.Ticker, price);
            if (!float.TryParse(node.IC1_BidPrice, out price))
            {
                price = -1000000.0f;
            }
            pd.sellPrices[(int)MarketIndex.IC1].Add(node.Ticker, price);
        }
    }

    private static void LogBaseArea(GameState gs)
    {
        float area = 0;
        foreach (var prod in gs.SitesNodes)
        {
            var buildingNode = gs.BuildingsNodes.First(r => r.Ticker == prod.BuildingTicker);
            area += float.Parse(buildingNode.Area);
        }
        Console.WriteLine($"Base Area: {area}");
    }

    private static List<ReceipeNode> GetOwnedReceipes(GameState gs)
    {
        List<ReceipeNode> receipeNodeOwned = new();
        foreach (var prod in gs.Productions)
        {
            var buildingNode = gs.BuildingsNodes.First(r => r.Name == prod.Type);
            if (buildingNode.Ticker == "RIG")
            {
                var rec = from r in gs.ReceipeNodes where r.BuildingTicker == buildingNode.Ticker select r;
                if (rec != null)
                {
                    ReceipeNode n = rec.First();
                    if (n.Outputs.Count == 0)
                    {
                        n.Outputs.Add(new ReceipeNodeItem()
                        {
                            Ticker = "H2O",
                            Amount = 8
                        });
                        n.TimeMs = (6 * 60 + 20) * 60 * 1000;
                        n.RecipeName = "n/a=>8xH2O";
                    }
                    receipeNodeOwned.Add(n);
                }
            }
            else
            {
                var rec = from r in gs.ReceipeNodes where r.BuildingTicker == buildingNode.Ticker select r;
                if (rec != null)
                {
                    receipeNodeOwned.AddRange(rec);
                }
            }
        }

        return receipeNodeOwned;
    }

    private static string GetFactionCode(string planetId, GameState gs)
    {
        PlanetDetailNode node = gs.PlanetDetailNodes.First(r => r.PlanetNaturalId == planetId);
        return node.FactionCode;
    }

    private static void GetConnectedStarsNear(string connectionId, int maxDist, GameState gs, ref List<string> visited, string? filterToFactionCode)
    {
        ConnectionsNode star = gs.ConnectionsNodes.First(r => r.SystemId == connectionId);

        if( !string.IsNullOrEmpty(filterToFactionCode) )
        {
            string factionId = GetFactionCode(star.NaturalId + "a", gs);
            if( factionId != "" && factionId != filterToFactionCode )
            {
                return;
            }
        }

        visited.Add(star.SystemId);
        if (maxDist > 0)
        {
            foreach (var connect in star.Connections)
            {
                if (!visited.Contains(connect.ConnectingId))
                {
                    GetConnectedStarsNear(connect.ConnectingId, maxDist - 1, gs, ref visited, filterToFactionCode);
                }
            }
        }
    }

    private static List<ConnectionsNode> GetStarsNear(string starName, int maxDist, GameState gs)
    {
        List<string> visited = new List<string>();
        ConnectionsNode star = gs.ConnectionsNodes.First(r => r.NaturalId == starName);
        visited.Add(star.SystemId);
        foreach ( var connect in star.Connections )
        {
            GetConnectedStarsNear(connect.ConnectingId, maxDist - 1, gs, ref visited, null);
        }

        List<ConnectionsNode> nodes = new List<ConnectionsNode>();
        foreach ( var visit in visited )
        {
            ConnectionsNode visitedStar = gs.ConnectionsNodes.First(r => r.SystemId == visit);
            nodes.Add(visitedStar);
        }

        return nodes;
    }

    private static List<PlanetDetailNode> GetPlanetsWithLocalMarkets(List<ConnectionsNode> nodes, GameState gs)
    {
        List<PlanetDetailNode> planets = new();
        foreach (var starNode in nodes)
        {
            var foundPlanets = gs.PlanetDetailNodes.FindAll(r => r.PlanetNaturalId.Contains(starNode.NaturalId) && r.HasLocalMarket == "True");
            planets.AddRange(foundPlanets);
        }

        return planets;
    }


    private static void CalcBuildingCosts(MarketIndex mi, GameState gs, ProcessedData pd, LogEnum log)
    {
        Dictionary<string, float> buildingCosts = new Dictionary<string, float>();
        foreach (var building in gs.BuildingcostsNodes)
        {
            float totalValue = 0.0f;
            if( !buildingCosts.TryGetValue(building.Building, out totalValue) )
            {
                totalValue = 0.0f;
            }

            float matMarketPrice = pd.buyPrices[(int)mi][building.Material];
            int amount = int.Parse(building.Amount);
            float amountPrice = matMarketPrice * amount;
            totalValue += amountPrice;

            buildingCosts[building.Building] = totalValue;
        }

        foreach( var cost in buildingCosts )
        {
            if( cost.Value > 0 )
            {
                pd.buildingCosts.Add(new BuildingCost()
                {
                    Building = cost.Key,
                    CostToBuy = cost.Value
                });
            }
        }

        for (int i=0; i<pd.buildingCosts.Count; i++)
        {
            var building = pd.buildingCosts[i];

            var workForcesForBuilding = gs.BuildingWorkforcesNodes.FindAll(r => r.Building == building.Building);
            foreach (var workforces in workForcesForBuilding)
            {
                int cap = int.Parse(workforces.Capacity);
                if (workforces.Level.Contains("PIONEER")) building.Pioneers += cap;
                if (workforces.Level.Contains("SETTLER")) building.Settlers += cap;
                if (workforces.Level.Contains("TECHNICIAN")) building.Technicians += cap;
                if (workforces.Level.Contains("ENGINEER")) building.Engineers += cap;
                if (workforces.Level.Contains("SCIENTIST")) building.Scientists += cap;
            }
        }

        pd.buildingCosts.Sort((a, b) => b.CostToBuy.CompareTo(a.CostToBuy));

        if (log == LogEnum.LogEnabled)
        {
            foreach (var cost in pd.buildingCosts)
            {
                Console.WriteLine($"{cost.Building}: {cost.CostToBuy}");
            }
        }
    }

    private static void CalcMarketSize(MarketIndex mi, GameState gs, ProcessedData pd, ref List<string> csvLines)
    {
        float totalBuyPrice = 0.0f;
        string miStr = mi.ToString();
        foreach( var order in gs.RequestsToBuyNodesLatest)
        {
            if( order.ExchangeCode != miStr )
            {
                continue;
            }

            float itemCost = float.Parse(order.ItemCost);
            float itemCount = float.Parse(order.ItemCount);
            totalBuyPrice += itemCost * itemCount;
        }

        float totalSellPrice = 0.0f;
        foreach (var order in gs.OffersToSellNodesLatest)
        {
            if (order.ExchangeCode != miStr)
            {
                continue;
            }

            float itemCost = float.Parse(order.ItemCost);
            float itemCount = float.Parse(order.ItemCount);
            totalSellPrice += itemCost * itemCount;
        }

        csvLines.Add($"{mi}, {totalSellPrice / 1000.0f / 1000.0f:0.000}, {totalBuyPrice / 1000.0f / 1000.0f:0.000}");
    }

    private static void UpdateSubReceipesHelper(
        ReceipeNodeItem outputItem,
        float numHoursPerOutput,
        float hoursPerOutputUnit,
        ReceipeNode parentReceipe,
        ReceipeNode inputReceipe,
        int desiredInputAmount,
        GameState gs,
        List<ReceipeNode> receipeNodes,
        ProcessedData pd)
    {
        string receipeName = parentReceipe.RecipeName;
        receipeName = receipeName.Replace(inputReceipe.Outputs[0].Ticker, $"[{inputReceipe.RecipeName}]");
        if (inputReceipe.Inputs.Count == 0)
        {
            float inputReceipeNumHours = ConvertMsToHours(inputReceipe.TimeMs);
            float inputReceipeHoursPerInputUnit = inputReceipeNumHours / inputReceipe.Outputs[0].Amount;

            pd.makeCostNodes2.Add(new MakeCostNode2()
            {
                RecipeName = receipeName,
                OutputTicker = outputItem.Ticker,
                OutputAmount = outputItem.Amount,
                OutputSellPrice = outputItem.OutputSellPrice,
                InputItemTicker = inputReceipe.Outputs[0].Ticker,
                InputItemAmount = inputReceipe.Outputs[0].Amount,
                CostToBuyInput = inputReceipe.CostInputToBuy,
                HoursToMake = Math.Max(
                    inputReceipeHoursPerInputUnit * desiredInputAmount,
                    numHoursPerOutput
                    )
            });
        }
    }











    //    for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
    //    {
    //        var inputItem = receipe.Inputs[iInput];
    //        foreach (var inputReceipe in inputItem.inputReceipes)
    //        {
    //            float inputReceipeNumHours = ConvertMsToHours(inputReceipe.TimeMs);
    //            float inputReceipeHoursPerInputUnit = numHoursPerOutput / inputReceipe.Outputs[0].Amount;
    //            string receipeName = receipe.RecipeName;
    //            receipeName = receipeName.Replace(inputItem.Ticker, $"[{inputReceipe.RecipeName}]");
    //            if (inputReceipe.Inputs.Count == 0)
    //            {
    //                pd.makeCostNodes2.Add(new MakeCostNode2()
    //                {
    //                    RecipeName = receipeName,
    //                    OutputTicker = outputItem.Ticker,
    //                    OutputAmount = outputItem.Amount,
    //                    OutputSellPrice = outputItem.OutputSellPrice,
    //                    InputItemTicker = inputItem.Ticker,
    //                    InputItemAmount = inputItem.Amount,
    //                    CostToBuyInput = inputReceipe.CostInputToBuy,
    //                    HoursToMake = Math.Max(
    //                        inputReceipeHoursPerInputUnit * inputItem.Amount,
    //                        numHoursPerOutput
    //                        )
    //                });
    //            }
    //            else
    //            {
    //                foreach (var inputInput in inputReceipe.Inputs)
    //                {
    //                    foreach (var inputInputReceipe in inputInput.inputReceipes)
    //                    {
    //                        UpdateSubReceipesHelper(outputItem, numHoursPerOutput, hoursPerOutputUnit, inputInputReceipe, gs, receipeNodes, pd);
    //                        //float inputInputReceipeNumHours = ConvertMsToHours(inputInputReceipe.TimeMs);
    //                        //float inputInputReceipeHoursPerInputUnit = numHoursPerOutput / inputInputReceipe.Outputs[0].Amount;
    //                        //receipeName = receipeName.Replace(inputItem.Ticker, $"[{inputReceipe.RecipeName}]");
    //                        //if (inputReceipe.Inputs.Count == 0)
    //                        //{
    //                        //    pd.makeCostNodes2.Add(new MakeCostNode2()
    //                        //    {
    //                        //        RecipeName = receipeName,
    //                        //        OutputTicker = outputItem.Ticker,
    //                        //        OutputAmount = outputItem.Amount,
    //                        //        OutputSellPrice = outputItem.OutputSellPrice,
    //                        //        InputItemTicker = inputItem.Ticker,
    //                        //        InputItemAmount = inputItem.Amount,
    //                        //        CostToBuyInput = inputReceipe.CostInputToBuy,
    //                        //        HoursToMake = Math.Max(
    //                        //            inputReceipeHoursPerInputUnit * inputItem.Amount,
    //                        //            numHoursPerOutput
    //                        //            )
    //                        //    });
    //                        //}
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private static void UpdateSubReceipes(ReceipeNode receipe, GameState gs, List<ReceipeNode> receipeNodes, ProcessedData pd)
    {
        float numHoursPerOutput = ConvertMsToHours(receipe.TimeMs);
        for (int iOutput = 0; iOutput < receipe.Outputs.Count; iOutput++)
        {
            var outputItem = receipe.Outputs[iOutput];
            float hoursPerOutputUnit = numHoursPerOutput / outputItem.Amount;
            for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
            {
                var inputItem = receipe.Inputs[iInput];
                foreach (var inputReceipe in inputItem.inputReceipes)
                {
                    UpdateSubReceipesHelper(
                        outputItem,
                        numHoursPerOutput,
                        hoursPerOutputUnit,
                        receipe,
                        inputReceipe,
                        inputItem.Amount,
                        gs,
                        receipeNodes,
                        pd);
                }
            }
        }
    }

    class CostTimeNode
    {
        public float cost;
        public float timeMs;
        public List<string> buildingsRequiredToMake = new();
    }

    private static CostTimeNode GetExpandedMakeCostsAndTime(ReceipeNode receipe, GameState gs, List<ReceipeNode> receipeNodes, ProcessedData pd)
    {
        CostTimeNode bestCostTimeNode = new CostTimeNode();
        float numHoursPerOutput = ConvertMsToHours(receipe.TimeMs);
        for (int iOutput = 0; iOutput < receipe.Outputs.Count; iOutput++)
        {
            var outputItem = receipe.Outputs[iOutput];
            float hoursPerOutputUnit = numHoursPerOutput / outputItem.Amount;
            if(receipe.Inputs.Count == 0)
            {
                bestCostTimeNode.cost = 1;
                bestCostTimeNode.timeMs = 1;
                bestCostTimeNode.buildingsRequiredToMake.Add("TEST");
            }
            else
            {
                for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
                {
                    var inputItem = receipe.Inputs[iInput];

                    foreach (var inputReceipe in inputItem.inputReceipes)
                    {
                        var costTime = GetExpandedMakeCostsAndTime(inputReceipe, gs, receipeNodes, pd);
                        if (costTime.cost < bestCostTimeNode.cost)
                        {
                            bestCostTimeNode = costTime;
                        }
                    }

                    inputItem.BestCostToMakeInput = bestCostTimeNode.cost;
                    inputItem.BestTimeToMakeInputMs = bestCostTimeNode.timeMs;
                    inputItem.BuildingsRequiredToMake = bestCostTimeNode.buildingsRequiredToMake;

                    receipe.CostInputToMake += inputItem.BestCostToMakeInput;
                }
            }
        }

        return bestCostTimeNode;
    }

    private static List<ReceipeNode> GetExpandedReceipes(ReceipeNode receipe, GameState gs, ProcessedData pd)
    {
        List<ReceipeNode> expandedList = new();
        expandedList.Add(receipe);
        //Console.WriteLine($"Expanding {receipe.RecipeName}");

        for (int iOutput = 0; iOutput < receipe.Outputs.Count; iOutput++)
        {
            var outputItem = receipe.Outputs[iOutput];
            if (receipe.Inputs.Count == 0)
            {
                expandedList.Add(receipe);
            }
            else
            {
                for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
                {
                    var inputItem = receipe.Inputs[iInput];
                    foreach (var inputReceipe in inputItem.inputReceipes)
                    {
                        List<ReceipeNode> expandedListForInput = GetExpandedReceipes(inputReceipe, gs, pd);
                        expandedList.AddRange(expandedListForInput);
                    }
                }
            }
        }

        expandedList = expandedList.Distinct().ToList();
        return expandedList;
    }

    private static void CalcRecipeCosts2(MarketIndex mi, GameState gs, ProcessedData pd, ref List<string> csvLines)
    {      
        for (int i = 0; i < gs.ReceipeNodes.Count; i++)
        {
            var receipe = gs.ReceipeNodes[i];
            receipe.InputsString = "";
            foreach ( var x in receipe.Inputs )
            {
                receipe.InputsString += $"[{x.Ticker}]";
            }
            receipe.OutputsString = "";
            foreach (var x in receipe.Outputs)
            {
                receipe.OutputsString += $"[{x.Ticker}]";
            }
        }

        for (int i = 0; i < gs.ReceipeNodes.Count; i++)
        {
            var receipe = gs.ReceipeNodes[i];
            receipe.TotalCostOutputToSell = 0.0f;
            for (int iOutput = 0; iOutput < receipe.Outputs.Count; iOutput++)
            {
                var outputItem = receipe.Outputs[iOutput];
                float outputPrice = pd.sellPrices[(int)mi][outputItem.Ticker];
                if (outputPrice > 0)
                {
                    float priceAmt = outputPrice * outputItem.Amount;
                    outputItem.OutputSellPrice = priceAmt;
                    receipe.TotalCostOutputToSell += priceAmt;
                }
                else
                {
                    outputItem.OutputSellPrice = 0.0f;
                }
            }

            for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
            {
                var inputItem = receipe.Inputs[iInput];
                float inputPrice = pd.buyPrices[(int)mi][inputItem.Ticker];
                if (inputPrice > 0)
                {
                    float priceAmt = inputPrice * inputItem.Amount;
                    inputItem.CostToBuyInput = priceAmt;
                    inputItem.CanBuyInput = true;
                }
                else
                {
                    inputItem.CostToBuyInput = 0.0f;
                    inputItem.CanBuyInput = false;
                }

                inputItem.inputReceipes = gs.ReceipeNodes.FindAll(r => r.OutputsString.Contains($"[{inputItem.Ticker}]"));
            }
        }

        List<ReceipeNode> receipeNodeOwned = GetOwnedReceipes(gs);
        for (int i = 0; i < receipeNodeOwned.Count; i++)
        {
            var receipe = receipeNodeOwned[i];
            List<ReceipeNode> expandedList = GetExpandedReceipes(receipe, gs, pd);
            string str = "";
            foreach( var e in expandedList )
            {
                str += $", {e.RecipeName}";
            }
            Console.WriteLine($"{receipe.RecipeName}: {str}");
        }



        //for (int i = 0; i < receipeNodes.Count; i++)
        //{
        //    var receipe = receipeNodes[i];
        //    for (int iOutput = 0; iOutput < receipe.Outputs.Count; iOutput++)
        //    {
        //        var outputItem = receipe.Outputs[iOutput];
        //        for (int iInput = 0; iInput < receipe.Inputs.Count; iInput++)
        //        {
        //            var inputItem = receipe.Inputs[iInput];
        //            float bestTimeToMakeInputHours = ConvertMsToHours(inputItem.BestTimeToMakeInputMs);
        //            var building = pd.buildingCosts.First(r => r.Building == receipe.BuildingTicker);

        //            if( !inputItem.BuildingsRequiredToMake.Contains(receipe.BuildingTicker) )
        //            {
        //                inputItem.BuildingsRequiredToMake.Add(receipe.BuildingTicker);
        //            }
        //            string buildings = "";
        //            inputItem.BuildingsRequiredToMake.ForEach(r => buildings += $" {r}");
        //            buildings = buildings.Trim();
        //            string buildingStarter = GetIsBuildingStarter(receipe.BuildingTicker);

        //            csvLines.Add(
        //                $"{mi}, {receipe.RecipeName}, {outputItem.Ticker}, {outputItem.Amount}, {outputItem.OutputSellPrice}, " + 
        //                $"{bestTimeToMakeInputHours}, {inputItem.Ticker}, {inputItem.Amount}, {inputItem.CostToBuyInput}, " +
        //                $"{buildings}, {building.Pioneers}, {building.Settlers}, {building.Technicians}, {building.Engineers}, {building.Scientists}, {building.CostToBuy}, {buildingStarter}, "
        //                );
        //            // Profit, ProfitPerHour, MaxProfitOnMarket");

        //            //receipe.Profit = receipe.CostOutputToSell - receipe.CostInputToMake;
        //            //receipe.ProfitPerHour = receipe.Profit / numHours;
        //            //receipe.MaxProfitOnMarket = CalcMaxProfitOnMarket(mi, gs, pd, receipe);
        //        }

        //        if (receipe.Inputs.Count == 0)
        //        {
        //            float numHours = ConvertMsToHours(receipe.TimeMs);
        //            var building = pd.buildingCosts.First(r => r.Building == receipe.BuildingTicker);
        //            string buildingStarter = GetIsBuildingStarter(receipe.BuildingTicker);

        //            csvLines.Add(
        //                $"{mi}, {receipe.RecipeName}, {outputItem.Ticker}, {outputItem.Amount}, {outputItem.OutputSellPrice}, {numHours}, " +
        //                $"n/a, n/a, n/a, " +
        //                $"{building.Building}, {building.Pioneers}, {building.Settlers}, {building.Technicians}, {building.Engineers}, {building.Scientists}, {building.CostToBuy}, {buildingStarter}, "
        //                );
        //        }
        //    }
        //}
    }

    private static List<string> LogMostProfitReceipes2(MarketIndex mi, List<ReceipeNode> receipeNode, ProcessedData pd, GameState gs)
    {
        List<ReceipeNode> receiptForProfit = new();
        for (int i = 0; i < receipeNode.Count; i++)
        {
            var receipe = receipeNode[i];
            if (receipe.Profit > 0 && receipe.TotalCostOutputToSell > 0 && receipe.CostInputToMake >= 0)
            {
                receiptForProfit.Add(receipe);
            }
        }
        receiptForProfit.Sort((a, b) => b.ProfitPerHour.CompareTo(a.ProfitPerHour));

        List<string> csvReceipeLines = new List<string>();
        for (int i = 0; i < receiptForProfit.Count; i++)
        {
            var receipe = receiptForProfit[i];

            var prod = gs.Productions.FirstOrDefault(r =>
            {
                var buildingNode = gs.BuildingsNodes.First(x => x.Name == r.Type);
                return buildingNode.Ticker == receipe.BuildingTicker;
            });
            var hasBuilding = !string.IsNullOrEmpty(prod.PlanetId);
            string buildingStarter = GetIsBuildingStarter(receipe.BuildingTicker);

            var building = pd.buildingCosts.First(r => r.Building == receipe.BuildingTicker);
            float hoursToMake = ConvertMsToHours(receipe.TimeMs);
            csvReceipeLines.Add(
                $"{mi}, {receipe.RecipeName}, {receipe.CostInputToMake:0}, {receipe.CostInputToBuy:0}, {receipe.TotalCostOutputToSell:0}, {receipe.Profit:0}, {receipe.ProfitPerHour:0}, {building.Building}, " +
                $"{building.Pioneers}, {building.Settlers}, {building.Technicians}, {building.Engineers}, {building.Scientists}, {building.CostToBuy}, {buildingStarter}, {hoursToMake}, {receipe.MaxProfitOnMarket:0}");
        }

        return csvReceipeLines;
    }

    public static void Process(string outPath, GameState gs)
    {
        ProcessedData pd = new();
        UpdateStockPrices(gs, pd);
        GetDailyNeedMats(gs, pd);

        //LogBaseArea(gs);
        CalcPlanetsNearMarkets(gs, 5, outPath);
        CalcNumLocalMarketOrdersPerPlanet(gs, outPath);
        CalcBuildingNameMap(gs, outPath);
        CalcMarketSizes(gs, pd, outPath);

        List<string> csvReceipeLines = new List<string>();
        csvReceipeLines.Add("MarketIndex, RecipeName, CostInputToMake, CostInputToBuy, CostOutputToSell, Profit, ProfitPerHour, Building, Pioneers, Settlers, Technicians, Engineers, Scientists, CostToBuyBuilding, BuildingStarter, HoursToMake, MaxProfitOnMarket");
        foreach (int i in Enum.GetValues(typeof(MarketIndex)))
        {
            MarketIndex mi = (MarketIndex)i;

            // Enable logging as desired
            CalcDailyWorkerCost(mi, pd, LogEnum.LogDisabled);
            CalcBuildingCosts(mi, gs, pd, LogEnum.LogDisabled);

            List<ReceipeNode> receipeNodeOwned = GetOwnedReceipes(gs);
            CalcRecipeCosts(mi, gs, receipeNodeOwned, pd);
            //LogMostProfitReceipes(receipeNodeOwned, pd, gs);
            CalcMarketOrders(mi, gs, pd, LogEnum.LogDisabled);
            CalcRecipeCosts(mi, gs, gs.ReceipeNodes, pd);
            csvReceipeLines.AddRange(LogMostProfitReceipes(mi, gs.ReceipeNodes, pd, gs));
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"processed-receipes.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"processed-receipes.csv"), csvReceipeLines);

        List<string> csvReceipeLines2 = new List<string>();
        csvReceipeLines2.Add("MarketIndex, RecipeName, OutputItem, OutputItemAmount, OutputSellPrice, HoursToMake, " +
                        "InputItem, InputItemAmount, CostToBuyInput, " +
                        "Building, Pioneers, Settlers, Technicians, Engineers, Scientists, CostToBuyBuilding, BuildingProfession, "
                        );
        foreach (int i in Enum.GetValues(typeof(MarketIndex)))
        {
            MarketIndex mi = (MarketIndex)i;
            CalcRecipeCosts2(mi, gs, pd, ref csvReceipeLines2);
            //csvReceipeLines.AddRange(LogMostProfitReceipes2(mi, gs.ReceipeNodes, pd, gs));
        }
        Console.WriteLine($"Writing {Path.Combine(outPath, $"processed-receipes2.csv")}");
        File.WriteAllLines(Path.Combine(outPath, $"processed-receipes2.csv"), csvReceipeLines2);
    }
}
