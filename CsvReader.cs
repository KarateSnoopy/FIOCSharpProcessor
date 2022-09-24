using System.Text.RegularExpressions;

class CsvReader
{
    private static List<string[]> ReadCsv(string dirPath, string fileName)
    {
        var lines = File.ReadAllLines(Path.Combine(dirPath, fileName));
        List<string[]> data = new List<string[]>();
        bool first = true;
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"); // handle quotes in CSV
        foreach ( string line in lines )
        {
            var split = CSVParser.Split(line);
            if (!first && split.Length > 1)
            {
                data.Add(split);
            }
            first = false;
        }
        return data;
    }

    private static string GetLastCreated(string dirPath, string filePrefix)
    {
        var files = Directory.EnumerateFiles(dirPath, filePrefix + "*", SearchOption.AllDirectories);

        DateTime lastDateTime = DateTime.MinValue;
        FileInfo? lastFi = null;
        foreach (string currentFile in files)
        {
            FileInfo fi = new FileInfo(currentFile);
            if (fi.CreationTime > lastDateTime)
            {
                lastDateTime = fi.CreationTime;
                lastFi = fi;
            }
        }

        if (lastFi != null)
        {
            return lastFi.Name;
        }
        return "";
    }

    public static GameState ReadData(string dirPath, List<string> planetsToMonitor)
    {
        GameState gs = new GameState();
        List<string[]> data;

        data = ReadCsv(dirPath, "buildings.csv");
        foreach( var lineData in data )
        {
            int i = 0;
            gs.BuildingsNodes.Add(new BuildingsNode()
            {
                Ticker = lineData[i++],
                Name = lineData[i++],
                Area = lineData[i++],
                Expertise = lineData[i++]
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "buildingcosts.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.BuildingcostsNodes.Add(new BuildingcostsNode()
            {
                Key = lineData[i++],
                Building = lineData[i++],
                Material = lineData[i++],
                Amount = lineData[i++]
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "buildingrecipes.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.BuildingRecipesNodes.Add(new BuildingRecipesNode()
            {
                Key = lineData[i++],
                Building = lineData[i++],
                Duration = lineData[i++]
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "buildingworkforces.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.BuildingWorkforcesNodes.Add(new BuildingWorkforcesNode()
            {
                Key = lineData[i++],
                Building = lineData[i++],
                Level = lineData[i++],
                Capacity = lineData[i++]
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "infrastructure_allinfos.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.InfrastructureAllInfosNodes.Add(new InfrastructureAllInfosNode()
            {
                PlanetName = lineData[i++],
                PlanetNaturalId = lineData[i++],
                Type = lineData[i++],
                Ticker = lineData[i++],
                Name = lineData[i++],
                ProjectId = lineData[i++],
                Level = lineData[i++],
                ActiveLevel = lineData[i++],
                CurrentLevel = lineData[i++],
                UpkeepStatus = lineData[i++],
                UpgradeStatus = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "infrastructure_allreports.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.InfrastructureAllReportsNodes.Add(new InfrastructureAllReportsNode()
            {
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                ExplorersGraceEnabled = lineData[i++],
                SimulationPeriod = lineData[i++],
                TimestampMs = lineData[i++],
                NextPopulationPioneer = lineData[i++],
                NextPopulationSettler = lineData[i++],
                NextPopulationTechnician = lineData[i++],
                NextPopulationEngineer = lineData[i++],
                NextPopulationScientist = lineData[i++],
                PopulationDifferencePioneer = lineData[i++],
                PopulationDifferenceSettler = lineData[i++],
                PopulationDifferenceTechnician = lineData[i++],
                PopulationDifferenceEngineer = lineData[i++],
                PopulationDifferenceScientist = lineData[i++],
                AverageHappinessPioneer = lineData[i++],
                AverageHappinessSettler = lineData[i++],
                AverageHappinessTechnician = lineData[i++],
                AverageHappinessEngineer = lineData[i++],
                AverageHappinessScientist = lineData[i++],
                UnemploymentRatePioneer = lineData[i++],
                UnemploymentRateSettler = lineData[i++],
                UnemploymentRateTechnician = lineData[i++],
                UnemploymentRateEngineer = lineData[i++],
                UnemploymentRateScientist = lineData[i++],
                OpenJobsPioneer = lineData[i++],
                OpenJobsSettler = lineData[i++],
                OpenJobsTechnician = lineData[i++],
                OpenJobsEngineer = lineData[i++],
                OpenJobsScientist = lineData[i++],
                NeedFulfillmentLifeSupport = lineData[i++],
                NeedFulfillmentSafety = lineData[i++],
                NeedFulfillmentHealth = lineData[i++],
                NeedFulfillmentComfort = lineData[i++],
                NeedFulfillmentCulture = lineData[i++],
                NeedFulfillmentEducation = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "materials.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.MaterialsNodes.Add(new MaterialsNode()
            {
                Ticker = lineData[i++],
                Name = lineData[i++],
                Category = lineData[i++],
                Weight = lineData[i++],
                Volume = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "planetdetail.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.PlanetDetailNodes.Add(new PlanetDetailNode()
            {
                PlanetId = lineData[i++],
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                Namer = lineData[i++],
                Nameable = lineData[i++],
                SystemId = lineData[i++],
                Gravity = lineData[i++],
                MagneticField = lineData[i++],
                Mass = lineData[i++],
                OrbitSemiMajorAxis = lineData[i++],
                OrbitEccentricity = lineData[i++],
                OrbitIndex = lineData[i++],
                Pressure = lineData[i++],
                Radius = lineData[i++],
                Sunlight = lineData[i++],
                Surface = lineData[i++],
                Temperature = lineData[i++],
                Fertility = lineData[i++],
                HasLocalMarket = lineData[i++],
                HasChamberOfCommerce = lineData[i++],
                HasWarehouse = lineData[i++],
                HasAdministrationCenter = lineData[i++],
                HasShipyard = lineData[i++],
                FactionCode = lineData[i++],
                FactionName = lineData[i++],
                GovernorId = lineData[i++],
                GovernorUserName = lineData[i++],
                GovernorCorporationId = lineData[i++],
                GovernorCorporationName = lineData[i++],
                GovernorCorporationCode = lineData[i++],
                CurrencyName = lineData[i++],
                CurrencyCode = lineData[i++],
                CollectorId = lineData[i++],
                CollectorName = lineData[i++],
                CollectorCode = lineData[i++],
                BaseLocalMarketFee = lineData[i++],
                WarehouseFee = lineData[i++],
                PopulationId = lineData[i++],
                COGCProgramStatus = lineData[i++],
                PlanetTier = lineData[i++],
                EpochTimestampMs = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "planetproductionfees.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.PlanetProductionFeesNodes.Add(new PlanetProductionFeesNode()
            {
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                Category = lineData[i++],
                WorkforceLevel = lineData[i++],
                FeeAmount = lineData[i++],
                FeeCurrency = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "planetresources.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.PlanetResourcesNodes.Add(new PlanetResourcesNode()
            {
                Key = lineData[i++],
                Planet = lineData[i++],
                Ticker = lineData[i++],
                Type = lineData[i++],
                Factor = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "planets.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.PlanetsNodes.Add(new PlanetsNode()
            {
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "recipeinputs.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.RecipeInputsNodes.Add(new RecipeInputOutputsNode()
            {
                Key = lineData[i++],
                Material = lineData[i++],
                Amount = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "recipeoutputs.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.RecipeOutputsNodes.Add(new RecipeInputOutputsNode()
            {
                Key = lineData[i++],
                Material = lineData[i++],
                Amount = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "systemplanets.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.SystemPlanetsNodes.Add(new SystemPlanetsNode()
            {
                NaturalId = lineData[i++],
                Name = lineData[i++],
                Radiation = lineData[i++],
                Pressure = lineData[i++],
                OrbitIndex = lineData[i++],
                Fertility = lineData[i++],
                Sunlight = lineData[i++],
                Surface = lineData[i++],
                Gravity = lineData[i++],
                Radius = lineData[i++],
                Temperature = lineData[i++],
                Mass = lineData[i++],
                MagneticField = lineData[i++],
                MassEarth = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "systems.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.SystemsNodes.Add(new SystemsNode()
            {
                NaturalId = lineData[i++],
                Name = lineData[i++],
                Type = lineData[i++],
                PositionX = lineData[i++],
                PositionY = lineData[i++],
                PositionZ = lineData[i++],
                SectorId = lineData[i++],
                SubSectorId = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        string fileName = GetLastCreated(Path.Combine(dirPath, "history"), "bids");
        data = ReadCsv(dirPath, Path.Combine("history", fileName));
        foreach (var lineData in data)
        {
            int i = 0;
            gs.RequestsToBuyNodesLatest.Add(new BidsNode()
            {
                MaterialTicker = lineData[i++],
                ExchangeCode = lineData[i++],
                CompanyId = lineData[i++],
                CompanyName = lineData[i++],
                CompanyCode = lineData[i++],
                ItemCount = lineData[i++],
                ItemCost = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "burnrate.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.BurnRateNodes.Add(new BurnRateNode()
            {
                Username = lineData[i++],
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                Ticker = lineData[i++],
                DailyConsumption = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "inventory.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.InventoryNodes.Add(new InventoryNode()
            {
                Username = lineData[i++],
                NaturalId = lineData[i++],
                Name = lineData[i++],
                StorageType = lineData[i++],
                Ticker = lineData[i++],
                Amount = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        foreach( var planetName in planetsToMonitor )
        {
            data = ReadCsv(dirPath, $"localmarket_buy_{planetName}.csv");
            foreach (var lineData in data)
            {
                int i = 0;
                gs.LocalMarketBuyNodes.Add(new LocalMarketBuySellNode()
                {
                    ContractNaturalId = lineData[i++],
                    PlanetNaturalId = lineData[i++],
                    PlanetName = lineData[i++],
                    CreatorCompanyName = lineData[i++],
                    CreatorCompanyCode = lineData[i++],
                    MaterialName = lineData[i++],
                    MaterialTicker = lineData[i++],
                    MaterialCategory = lineData[i++],
                    MaterialWeight = lineData[i++],
                    MaterialVolume = lineData[i++],
                    MaterialAmount = lineData[i++],
                    Price = lineData[i++],
                    PriceCurrency = lineData[i++],
                    DeliveryTime = lineData[i++],
                    CreationTimeEpochMs = lineData[i++],
                    ExpiryTimeEpochMs = lineData[i++],
                    MinimumRating = lineData[i++],
                });

                if (i != lineData.Length) throw new Exception("Invalid CSV");
            }

            data = ReadCsv(dirPath, $"localmarket_sell_{planetName}.csv");
            foreach (var lineData in data)
            {
                int i = 0;
                gs.LocalMarketSellNodes.Add(new LocalMarketBuySellNode()
                {
                    ContractNaturalId = lineData[i++],
                    PlanetNaturalId = lineData[i++],
                    PlanetName = lineData[i++],
                    CreatorCompanyName = lineData[i++],
                    CreatorCompanyCode = lineData[i++],
                    MaterialName = lineData[i++],
                    MaterialTicker = lineData[i++],
                    MaterialCategory = lineData[i++],
                    MaterialWeight = lineData[i++],
                    MaterialVolume = lineData[i++],
                    MaterialAmount = lineData[i++],
                    Price = lineData[i++],
                    PriceCurrency = lineData[i++],
                    DeliveryTime = lineData[i++],
                    CreationTimeEpochMs = lineData[i++],
                    ExpiryTimeEpochMs = lineData[i++],
                    MinimumRating = lineData[i++],
                });

                if (i != lineData.Length) throw new Exception("Invalid CSV");
            }

            data = ReadCsv(dirPath, $"localmarket_ship_{planetName}.csv");
            foreach (var lineData in data)
            {
                int i = 0;
                gs.LocalMarketShipNodes.Add(new LocalMarketBuySellNode()
                {
                    ContractNaturalId = lineData[i++],
                    PlanetNaturalId = lineData[i++],
                    PlanetName = lineData[i++],
                    CreatorCompanyName = lineData[i++],
                    CreatorCompanyCode = lineData[i++],
                    MaterialName = lineData[i++],
                    MaterialTicker = lineData[i++],
                    MaterialCategory = lineData[i++],
                    MaterialWeight = lineData[i++],
                    MaterialVolume = lineData[i++],
                    MaterialAmount = lineData[i++],
                    Price = lineData[i++],
                    PriceCurrency = lineData[i++],
                    DeliveryTime = lineData[i++],
                    CreationTimeEpochMs = lineData[i++],
                    ExpiryTimeEpochMs = lineData[i++],
                    MinimumRating = lineData[i++],
                });

                if (i != lineData.Length) throw new Exception("Invalid CSV");
            }
        }


        fileName = GetLastCreated(Path.Combine(dirPath, "history"), "orders");
        data = ReadCsv(dirPath, Path.Combine("history", fileName));
        foreach (var lineData in data)
        {
            int i = 0;
            gs.OffersToSellNodesLatest.Add(new OrdersNode()
            {
                MaterialTicker = lineData[i++],
                ExchangeCode = lineData[i++],
                CompanyId = lineData[i++],
                CompanyName = lineData[i++],
                CompanyCode = lineData[i++],
                ItemCount = lineData[i++],
                ItemCost = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        fileName = GetLastCreated(Path.Combine(dirPath, "history"), "prices");
        data = ReadCsv(dirPath, Path.Combine("history", fileName));
        foreach (var lineData in data)
        {
            int i = 0;
            gs.PricesNodesLatest.Add(new PricesNode()
            {
                Ticker = lineData[i++],
                MMBuy = lineData[i++],
                MMSell = lineData[i++],

                AI1_Average = lineData[i++],
                AI1_AskAmt = lineData[i++],
                AI1_AskPrice = lineData[i++],
                AI1_AskAvail = lineData[i++],
                AI1_BidAmt = lineData[i++],
                AI1_BidPrice = lineData[i++],
                AI1_BidAvail = lineData[i++],

                CI1_Average = lineData[i++],
                CI1_AskAmt = lineData[i++],
                CI1_AskPrice = lineData[i++],
                CI1_AskAvail = lineData[i++],
                CI1_BidAmt = lineData[i++],
                CI1_BidPrice = lineData[i++],
                CI1_BidAvail = lineData[i++],

                CI2_Average = lineData[i++],
                CI2_AskAmt = lineData[i++],
                CI2_AskPrice = lineData[i++],
                CI2_AskAvail = lineData[i++],
                CI2_BidAmt = lineData[i++],
                CI2_BidPrice = lineData[i++],
                CI2_BidAvail = lineData[i++],

                NC1_Average = lineData[i++],
                NC1_AskAmt = lineData[i++],
                NC1_AskPrice = lineData[i++],
                NC1_AskAvail = lineData[i++],
                NC1_BidAmt = lineData[i++],
                NC1_BidPrice = lineData[i++],
                NC1_BidAvail = lineData[i++],

                NC2_Average = lineData[i++],
                NC2_AskAmt = lineData[i++],
                NC2_AskPrice = lineData[i++],
                NC2_AskAvail = lineData[i++],
                NC2_BidAmt = lineData[i++],
                NC2_BidPrice = lineData[i++],
                NC2_BidAvail = lineData[i++],

                IC1_Average = lineData[i++],
                IC1_AskAmt = lineData[i++],
                IC1_AskPrice = lineData[i++],
                IC1_AskAvail = lineData[i++],
                IC1_BidAmt = lineData[i++],
                IC1_BidPrice = lineData[i++],
                IC1_BidAvail = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "sites.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.SitesNodes.Add(new SitesNode()
            {
                Username = lineData[i++],
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                BuildingId = lineData[i++],
                BuildingTicker = lineData[i++],
                BuildingCondition = lineData[i++],
                BuildingLastRepairEpochMs = lineData[i++],
                BuildingEfficiency = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        data = ReadCsv(dirPath, "workforce.csv");
        foreach (var lineData in data)
        {
            int i = 0;
            gs.WorkforceNodes.Add(new WorkforceNode()
            {
                Username = lineData[i++],
                PlanetNaturalId = lineData[i++],
                PlanetName = lineData[i++],
                MaterialTicker = lineData[i++],
                DailyAmount = lineData[i++],
            });

            if (i != lineData.Length) throw new Exception("Invalid CSV");
        }

        return gs;
    }
}

