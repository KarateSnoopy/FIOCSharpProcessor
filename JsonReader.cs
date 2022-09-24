using Newtonsoft.Json.Linq;

class JsonReader
{
    private static void ReadProduction(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "production_KarateSnoopy.json"));
        JArray jsonArray = JArray.Parse(lines);
        foreach (var jsonNode in jsonArray)
        {
            Production n = new Production();
            n.ProductionLineId = ((string?)jsonNode["ProductionLineId"]) ?? "";
            n.SiteId = ((string?)jsonNode["SiteId"]) ?? "";
            n.PlanetId = ((string?)jsonNode["PlanetId"]) ?? "";
            n.PlanetNaturalId = ((string?)jsonNode["PlanetNaturalId"]) ?? "";
            n.PlanetName = ((string?)jsonNode["PlanetName"]) ?? "";
            n.Type = ((string?)jsonNode["Type"]) ?? "";
            n.Capacity = ((float?)jsonNode["Capacity"]) ?? 0.0f;
            n.Efficiency = ((float?)jsonNode["Efficiency"]) ?? 0.0f;
            n.Condition = ((float?)jsonNode["Condition"]) ?? 0.0f;
            n.UserNameSubmitted = ((string?)jsonNode["UserNameSubmitted"]) ?? "";
            n.Timestamp = ((string?)jsonNode["Timestamp"]) ?? "";

            var jsonArrayInt = jsonNode["Orders"];
            n.Orders = new();
            if (jsonArrayInt != null)
            {
                foreach (var jsonNodeInt in jsonArrayInt)
                {
                    ProductionOrder item = new();

                    var jsonArrayInt2 = jsonNodeInt["Inputs"];
                    item.Inputs = new();
                    if (jsonArrayInt2 != null)
                    {
                        foreach (var jsonNodeInt2 in jsonArrayInt2)
                        {
                            ProductionOrderItem item2 = new();
                            item2.ProductionLineId = ((string?)jsonNodeInt2["ProductionLineInputId"]) ?? "";
                            item2.MaterialName = ((string?)jsonNodeInt2["MaterialName"]) ?? "";
                            item2.MaterialTicker = ((string?)jsonNodeInt2["MaterialTicker"]) ?? "";
                            item2.MaterialId = ((string?)jsonNodeInt2["MaterialId"]) ?? "";
                            item2.MaterialAmount = ((int?)jsonNodeInt2["MaterialAmount"]) ?? 0;
                            item.Inputs.Add(item2);
                        }
                    }

                    jsonArrayInt2 = jsonNodeInt["Outputs"];
                    item.Outputs = new();
                    if (jsonArrayInt2 != null)
                    {
                        foreach (var jsonNodeInt2 in jsonArrayInt2)
                        {
                            ProductionOrderItem item2 = new();
                            item2.ProductionLineId = ((string?)jsonNodeInt2["ProductionLineOutputId"]) ?? "";
                            item2.MaterialName = ((string?)jsonNodeInt2["MaterialName"]) ?? "";
                            item2.MaterialTicker = ((string?)jsonNodeInt2["MaterialTicker"]) ?? "";
                            item2.MaterialId = ((string?)jsonNodeInt2["MaterialId"]) ?? "";
                            item2.MaterialAmount = ((int?)jsonNodeInt2["MaterialAmount"]) ?? 0;
                            item.Outputs.Add(item2);
                        }
                    }

                    item.ProductionLineOrderId = ((string?)jsonNodeInt["ProductionLineOrderId"]) ?? "";
                    item.CreatedEpochMs = ((long?)jsonNodeInt["CreatedEpochMs"]) ?? 0;
                    item.StartedEpochMs = ((long?)jsonNodeInt["StartedEpochMs"]) ?? 0;
                    item.CompletionEpochMs = ((long?)jsonNodeInt["CompletionEpochMs"]) ?? 0;
                    item.DurationMs = ((long?)jsonNodeInt["DurationMs"]) ?? 0;
                    item.LastUpdatedEpochMs = ((long?)jsonNodeInt["LastUpdatedEpochMs"]) ?? 0;
                    item.CompletedPercentage = ((float?)jsonNodeInt["CompletedPercentage"]) ?? 0.0f;
                    item.IsHalted = (((bool?)jsonNodeInt["IsHalted"]) ?? false);
                    item.Recurring = (((bool?)jsonNodeInt["Recurring"]) ?? false);
                    item.ProductionFee = ((float?)jsonNodeInt["ProductionFee"]) ?? 0.0f;
                    item.ProductionFeeCurrency = ((string?)jsonNodeInt["ProductionFeeCurrency"]) ?? "";
                    item.ProductionFeeCollectorId = ((string?)jsonNodeInt["ProductionFeeCollectorId"]) ?? "";
                    item.ProductionFeeCollectorName = ((string?)jsonNodeInt["ProductionFeeCollectorName"]) ?? "";
                    item.ProductionFeeCollectorCode = ((string?)jsonNodeInt["ProductionFeeCollectorCode"]) ?? "";

                    n.Orders.Add(item);
                }
            }

            gs.Productions.Add(n);
        }
    }

    private static void ReadWorkForceNeeds(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "global_workforceneeds.json"));
        JArray jsonArray = JArray.Parse(lines);
        foreach (var jsonNode in jsonArray)
        {
            WorkForceNeed n = new WorkForceNeed();
            n.WorkforceType = ((string?)jsonNode["WorkforceType"]) ?? "";

            var jsonArrayInt = jsonNode["Needs"];
            n.Needs = new();
            if (jsonArrayInt != null)
            {
                foreach (var jsonNodeInt in jsonArrayInt)
                {
                    WorkForceNeedItem item = new();
                    item.MaterialId = ((string?)jsonNodeInt["MaterialId"]) ?? "";
                    item.MaterialName = ((string?)jsonNodeInt["MaterialName"]) ?? "";
                    item.MaterialTicker = ((string?)jsonNodeInt["MaterialTicker"]) ?? "";
                    item.MaterialCategory = ((string?)jsonNodeInt["MaterialCategory"]) ?? "";
                    item.Amount = ((float?)jsonNodeInt["Amount"]) ?? 0.0f;
                    n.Needs.Add(item);
                }
            }

            gs.WorkForceNeeds.Add(n);
        }
    }

    private static void ReadMaterials(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "material_allmaterials.json"));
        JArray jsonArray = JArray.Parse(lines);
        foreach (var systemLink in jsonArray)
        {
            MaterialNode n = new();
            n.MaterialId = ((string?)systemLink["MaterialId"]) ?? "";
            n.CategoryName = ((string?)systemLink["CategoryName"]) ?? "";
            n.CategoryId = ((string?)systemLink["CategoryId"]) ?? "";
            n.Name = ((string?)systemLink["Name"]) ?? "";
            n.Ticker = ((string?)systemLink["Ticker"]) ?? "";
            n.Weight = ((float?)systemLink["Weight"]) ?? 0.0f;
            n.Volume = ((float?)systemLink["Volume"]) ?? 0.0f;
            n.UserNameSubmitted = ((string?)systemLink["UserNameSubmitted"]) ?? "";
            n.Timestamp = ((string?)systemLink["Timestamp"]) ?? "";

            gs.MaterialNodes.Add(n);
        }
    }

    private static void ReadSystemLinks(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "rain_systemlinks.json"));
        JArray jsonArray = JArray.Parse(lines);
        foreach (var systemLink in jsonArray)
        {
            SystemLinkNode n = new SystemLinkNode();
            n.left = ((string?)systemLink["Left"]) ?? "";
            n.right = ((string?)systemLink["Right"]) ?? "";
            gs.SystemLinkNodes.Add(n);
        }
    }

    private static void ReadRecipes(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "recipes_allrecipes.json"));
        JArray jsonArray = JArray.Parse(lines);
        foreach (var jsonNode in jsonArray)
        {
            ReceipeNode n = new ReceipeNode();

            var jsonArrayInt = jsonNode["Inputs"];
            n.Inputs = new List<ReceipeNodeItem>();
            if (jsonArrayInt != null)
            {
                foreach (var jsonNodeInt in jsonArrayInt)
                {
                    ReceipeNodeItem item = new ReceipeNodeItem();
                    item.Ticker = ((string?)jsonNodeInt["Ticker"]) ?? "";
                    item.Amount = ((int?)jsonNodeInt["Amount"]) ?? 0;
                    n.Inputs.Add(item);
                }
            }

            jsonArrayInt = jsonNode["Outputs"];
            n.Outputs = new List<ReceipeNodeItem>();
            if (jsonArrayInt != null)
            {
                foreach (var jsonNodeInt in jsonArrayInt)
                {
                    ReceipeNodeItem item = new ReceipeNodeItem();
                    item.Ticker = ((string?)jsonNodeInt["Ticker"]) ?? "";
                    item.Amount = ((int?)jsonNodeInt["Amount"]) ?? 0;
                    n.Outputs.Add(item);
                }
            }

            n.BuildingTicker = ((string?)jsonNode["BuildingTicker"]) ?? "";
            n.RecipeName = ((string?)jsonNode["RecipeName"]) ?? "";
            n.TimeMs = ((long?)jsonNode["TimeMs"]) ?? 0;
            gs.ReceipeNodes.Add(n);
        }
    }

    private static void ReadSystemStars(string dirPath, GameState gs)
    {
        var lines = File.ReadAllText(Path.Combine(dirPath, "systemstars.json"));
        var jsonArray = JArray.Parse(lines);
        foreach (var jsonNode in jsonArray)
        {
            ConnectionsNode n = new ConnectionsNode();

            var jsonArrayInt = jsonNode["Connections"];
            n.Connections = new List<ConnectionsNodeItem>();
            if (jsonArrayInt != null)
            {
                foreach (var jsonNodeInt in jsonArrayInt)
                {
                    ConnectionsNodeItem item = new ConnectionsNodeItem();
                    item.SystemConnectionId = ((string?)jsonNodeInt["SystemConnectionId"]) ?? "";
                    item.ConnectingId = ((string?)jsonNodeInt["ConnectingId"]) ?? "";
                    n.Connections.Add(item);
                }
            }

            n.SystemId = ((string?)jsonNode["SystemId"]) ?? "";
            n.Name = ((string?)jsonNode["Name"]) ?? "";
            n.NaturalId = ((string?)jsonNode["NaturalId"]) ?? "";
            n.Type = ((string?)jsonNode["Type"]) ?? "";
            n.PositionX = ((float?)jsonNode["PositionX"]) ?? 0;
            n.PositionY = ((float?)jsonNode["PositionY"]) ?? 0;
            n.PositionZ = ((float?)jsonNode["PositionZ"]) ?? 0;
            n.SectorId = ((string?)jsonNode["SectorId"]) ?? "";
            n.SubSectorId = ((string?)jsonNode["SubSectorId"]) ?? "";
            n.UserNameSubmitted = ((string?)jsonNode["UserNameSubmitted"]) ?? "";
            n.Timestamp = ((string?)jsonNode["Timestamp"]) ?? "";

            gs.ConnectionsNodes.Add(n);
        }
    }

    public static void ReadData(string dirPath, GameState gs)
    {
        ReadSystemLinks(dirPath, gs);
        ReadRecipes(dirPath, gs);
        ReadSystemStars(dirPath, gs);
        ReadWorkForceNeeds(dirPath, gs);
        ReadMaterials(dirPath, gs);
        ReadProduction(dirPath, gs);
    }
}
