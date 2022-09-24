using System.Diagnostics;
using System.Reflection;

struct BuildingsNode
{
    public string Ticker;
    public string Name;
    public string Area;
    public string Expertise;
}

struct BuildingcostsNode
{
    public string Key;
    public string Building;
    public string Material;
    public string Amount;
}

struct BuildingRecipesNode
{
    public string Key;
    public string Building;
    public string Duration;
}

struct BuildingWorkforcesNode
{
    public string Key;
    public string Building;
    public string Level;
    public string Capacity;
}

struct InfrastructureAllInfosNode
{
    public string PlanetName;
    public string PlanetNaturalId;
    public string Type;
    public string Ticker;
    public string Name;
    public string ProjectId;
    public string Level;
    public string ActiveLevel;
    public string CurrentLevel;
    public string UpkeepStatus;
    public string UpgradeStatus;
}

struct InfrastructureAllReportsNode
{
    public string PlanetNaturalId;
    public string PlanetName;
    public string ExplorersGraceEnabled;
    public string SimulationPeriod;
    public string TimestampMs;
    public string NextPopulationPioneer;
    public string NextPopulationSettler;
    public string NextPopulationTechnician;
    public string NextPopulationEngineer;
    public string NextPopulationScientist;
    public string PopulationDifferencePioneer;
    public string PopulationDifferenceSettler;
    public string PopulationDifferenceTechnician;
    public string PopulationDifferenceEngineer;
    public string PopulationDifferenceScientist;
    public string AverageHappinessPioneer;
    public string AverageHappinessSettler;
    public string AverageHappinessTechnician;
    public string AverageHappinessEngineer;
    public string AverageHappinessScientist;
    public string UnemploymentRatePioneer;
    public string UnemploymentRateSettler;
    public string UnemploymentRateTechnician;
    public string UnemploymentRateEngineer;
    public string UnemploymentRateScientist;
    public string OpenJobsPioneer;
    public string OpenJobsSettler;
    public string OpenJobsTechnician;
    public string OpenJobsEngineer;
    public string OpenJobsScientist;
    public string NeedFulfillmentLifeSupport;
    public string NeedFulfillmentSafety;
    public string NeedFulfillmentHealth;
    public string NeedFulfillmentComfort;
    public string NeedFulfillmentCulture;
    public string NeedFulfillmentEducation;
}

struct MaterialsNode
{
    public string Ticker;
    public string Name;
    public string Category;
    public string Weight;
    public string Volume;
}

struct PlanetDetailNode
{
    public string PlanetId;
    public string PlanetNaturalId;
    public string PlanetName;
    public string Namer;
    public string Nameable;
    public string SystemId;
    public string Gravity;
    public string MagneticField;
    public string Mass;
    public string OrbitSemiMajorAxis;
    public string OrbitEccentricity;
    public string OrbitIndex;
    public string Pressure;
    public string Radius;
    public string Sunlight;
    public string Surface;
    public string Temperature;
    public string Fertility;
    public string HasLocalMarket;
    public string HasChamberOfCommerce;
    public string HasWarehouse;
    public string HasAdministrationCenter;
    public string HasShipyard;
    public string FactionCode;
    public string FactionName;
    public string GovernorId;
    public string GovernorUserName;
    public string GovernorCorporationId;
    public string GovernorCorporationName;
    public string GovernorCorporationCode;
    public string CurrencyName;
    public string CurrencyCode;
    public string CollectorId;
    public string CollectorName;
    public string CollectorCode;
    public string BaseLocalMarketFee;
    public string WarehouseFee;
    public string PopulationId;
    public string COGCProgramStatus;
    public string PlanetTier;
    public string EpochTimestampMs;
}

struct PlanetProductionFeesNode
{
    public string PlanetNaturalId;
    public string PlanetName;
    public string Category;
    public string WorkforceLevel;
    public string FeeAmount;
    public string FeeCurrency;
}

struct PlanetResourcesNode
{
    public string Key;
    public string Planet;
    public string Ticker;
    public string Type;
    public string Factor;
}

struct PlanetsNode
{
    public string PlanetNaturalId;
    public string PlanetName;
}

struct RecipeInputOutputsNode
{
    public string Key;
    public string Material;
    public string Amount;
}

struct SystemPlanetsNode
{
    public string NaturalId;
    public string Name;
    public string Radiation;
    public string Pressure;
    public string OrbitIndex;
    public string Fertility;
    public string Sunlight;
    public string Surface;
    public string Gravity;
    public string Radius;
    public string Temperature;
    public string Mass;
    public string MagneticField;
    public string MassEarth;
}

struct SystemsNode
{
    public string NaturalId;
    public string Name;
    public string Type;
    public string PositionX;
    public string PositionY;
    public string PositionZ;
    public string SectorId;
    public string SubSectorId;
}

struct BidsNode
{
    public string MaterialTicker;
    public string ExchangeCode;
    public string CompanyId;
    public string CompanyName;
    public string CompanyCode;
    public string ItemCount;
    public string ItemCost;
}

struct BurnRateNode
{
    public string Username;
    public string PlanetNaturalId;
    public string PlanetName;
    public string Ticker;
    public string DailyConsumption;
}

struct InventoryNode
{
    public string Username;
    public string NaturalId;
    public string Name;
    public string StorageType;
    public string Ticker;
    public string Amount;
}

struct LocalMarketBuySellNode
{
    public string ContractNaturalId;
    public string PlanetNaturalId;
    public string PlanetName;
    public string CreatorCompanyName;
    public string CreatorCompanyCode;
    public string MaterialName;
    public string MaterialTicker;
    public string MaterialCategory;
    public string MaterialWeight;
    public string MaterialVolume;
    public string MaterialAmount;
    public string Price;
    public string PriceCurrency;
    public string DeliveryTime;
    public string CreationTimeEpochMs;
    public string ExpiryTimeEpochMs;
    public string MinimumRating;
}

struct OrdersNode
{
    public string MaterialTicker;
    public string ExchangeCode;
    public string CompanyId;
    public string CompanyName;
    public string CompanyCode;
    public string ItemCount;
    public string ItemCost;
}

struct PricesNode
{
    public string Ticker;
    public string MMBuy;
    public string MMSell;

    public string AI1_Average;
    public string AI1_AskAmt;
    public string AI1_AskPrice;
    public string AI1_AskAvail;
    public string AI1_BidAmt;
    public string AI1_BidPrice;
    public string AI1_BidAvail;

    public string CI1_Average;
    public string CI1_AskAmt;
    public string CI1_AskPrice;
    public string CI1_AskAvail;
    public string CI1_BidAmt;
    public string CI1_BidPrice;
    public string CI1_BidAvail;

    public string CI2_Average;
    public string CI2_AskAmt;
    public string CI2_AskPrice;
    public string CI2_AskAvail;
    public string CI2_BidAmt;
    public string CI2_BidPrice;
    public string CI2_BidAvail;

    public string NC1_Average;
    public string NC1_AskAmt;
    public string NC1_AskPrice;
    public string NC1_AskAvail;
    public string NC1_BidAmt;
    public string NC1_BidPrice;
    public string NC1_BidAvail;

    public string NC2_Average;
    public string NC2_AskAmt;
    public string NC2_AskPrice;
    public string NC2_AskAvail;
    public string NC2_BidAmt;
    public string NC2_BidPrice;
    public string NC2_BidAvail;

    public string IC1_Average;
    public string IC1_AskAmt;
    public string IC1_AskPrice;
    public string IC1_AskAvail;
    public string IC1_BidAmt;
    public string IC1_BidPrice;
    public string IC1_BidAvail;
}

struct SitesNode
{
    public string Username;
    public string PlanetNaturalId;
    public string PlanetName;
    public string BuildingId;
    public string BuildingTicker;
    public string BuildingCondition;
    public string BuildingLastRepairEpochMs;
    public string BuildingEfficiency;
}

struct WorkforceNode
{
    public string Username;
    public string PlanetNaturalId;
    public string PlanetName;
    public string MaterialTicker;
    public string DailyAmount;
}

struct SystemLinkNode
{
    public string left;
    public string right;
}

struct ReceipeNodeItem
{
    public string Ticker;
    public int Amount;
}

class ReceipeNode
{
    public string BuildingTicker = "";
    public string RecipeName = "";
    public List<ReceipeNodeItem> Inputs = new();
    public List<ReceipeNodeItem> Outputs = new();
    public long TimeMs;

    public float CostInputToBuy;
    public bool CanMakeInput;
    public float CostInputToMake;
    public float CostOutputToSell;
    public float Profit;
    public float ProfitPerHour;
    public float MaxProfitOnMarket;
}

struct MakeCostNode
{
    public long TimeMs;
    public string Ticker;
    public int Amount;
    public int MakeReceipeIndex;
    public float CostOutputToSell;
}

struct ConnectionsNodeItem
{
    public string SystemConnectionId;
    public string ConnectingId;
}

struct ConnectionsNode
{
    public List<ConnectionsNodeItem> Connections;
    public string SystemId;
    public string Name;
    public string NaturalId;
    public string Type;
    public float PositionX;
    public float PositionY;
    public float PositionZ;
    public string SectorId;
    public string SubSectorId;
    public string UserNameSubmitted;
    public string Timestamp;
}

struct WorkForceNeedItem
{
    public string MaterialId;
    public string MaterialName;
    public string MaterialTicker;
    public string MaterialCategory;
    public float Amount;
}

struct WorkForceNeed
{
    public string WorkforceType;
    public List<WorkForceNeedItem> Needs;
}

struct MaterialNode
{
    public string MaterialId;
    public string CategoryName;
    public string CategoryId;
    public string Name;
    public string Ticker;
    public float Weight;
    public float Volume;
    public string UserNameSubmitted;
    public string Timestamp;
}

struct ProductionOrderItem
{
    public string ProductionLineId;
    public string MaterialName;
    public string MaterialTicker;
    public string MaterialId;
    public int MaterialAmount;
}

struct ProductionOrder
{
    public List<ProductionOrderItem> Inputs;
    public List<ProductionOrderItem> Outputs;
    public string ProductionLineOrderId;
    public long CreatedEpochMs;
    public long StartedEpochMs;
    public long CompletionEpochMs;
    public long DurationMs;
    public long LastUpdatedEpochMs;
    public float CompletedPercentage;
    public bool IsHalted;
    public bool Recurring;
    public float ProductionFee;
    public string ProductionFeeCurrency;
    public string ProductionFeeCollectorId;
    public string ProductionFeeCollectorName;
    public string ProductionFeeCollectorCode;
}

struct Production
{
    public List<ProductionOrder> Orders;
    public string ProductionLineId;
    public string SiteId;
    public string PlanetId;
    public string PlanetNaturalId;
    public string PlanetName;
    public string Type;
    public float Capacity;
    public float Efficiency;
    public float Condition;
    public string UserNameSubmitted;
    public string Timestamp;
}

struct ProcessedDataMat
{
    public string Mat;
    public float Amount;
}

struct LocalMarketBuySellNodeProcessed
{
    public string PlanetNaturalId;
    public string CreatorCompanyName;
    public string CreatorCompanyCode;
    public string MaterialName;
    public string MaterialTicker;
    public float MaterialWeight;
    public float MaterialVolume;
    public int MaterialAmount;
    public float Price;
    public string PriceCurrency;
    public int DeliveryTime;
    public long CreationTimeEpochMs;
    public long ExpiryTimeEpochMs;
    public string MinimumRating;

    public float matMarketPrice;
    public float costToBuyFromMarket;
    public float gainedFromSellToMarket;
    public float profit;
}

class BuildingCost
{
    public string Building = "";
    public float CostToBuy = 0.0f;
    public int Pioneers = 0;
    public int Settlers = 0;
    public int Technicians = 0;
    public int Engineers = 0;
    public int Scientists = 0;
}

enum MarketIndex
{
    AI1 = 0,
    CI2,
    CI1,
    IC1,
    NC1,
    NC2
}

class ProcessedData
{
    public List<string> interestingMats = new();
    public List<ProcessedDataMat> dailyNeedMats = new();
    public float dailyCostForWorkers;
    public Dictionary<string, float>[] buyPrices = new Dictionary<string, float>[6];
    public Dictionary<string, float>[] sellPrices = new Dictionary<string, float>[6];

    public List<MakeCostNode> makeCostNodes = new();
    public List<LocalMarketBuySellNodeProcessed> localMarketBuySellNodeProcessed = new();
    public List<BuildingCost> buildingCosts = new();
}

class GameState
{
    public List<BuildingsNode> BuildingsNodes = new();
    public List<BuildingcostsNode> BuildingcostsNodes = new();
    public List<BuildingRecipesNode> BuildingRecipesNodes = new();
    public List<BuildingWorkforcesNode> BuildingWorkforcesNodes = new();
    public List<InfrastructureAllInfosNode> InfrastructureAllInfosNodes = new();
    public List<InfrastructureAllReportsNode> InfrastructureAllReportsNodes = new();
    public List<MaterialsNode> MaterialsNodes = new();
    public List<PlanetDetailNode> PlanetDetailNodes = new();
    public List<PlanetProductionFeesNode> PlanetProductionFeesNodes = new();
    public List<PlanetResourcesNode> PlanetResourcesNodes = new();
    public List<PlanetsNode> PlanetsNodes = new();
    public List<RecipeInputOutputsNode> RecipeInputsNodes = new();
    public List<RecipeInputOutputsNode> RecipeOutputsNodes = new();
    public List<SystemPlanetsNode> SystemPlanetsNodes = new();
    public List<SystemsNode> SystemsNodes = new();
    public List<BidsNode> RequestsToBuyNodesLatest = new(); // Bids, folk who want to buy
    public List<OrdersNode> OffersToSellNodesLatest = new(); // Orders, folks who want to sell
    public List<BurnRateNode> BurnRateNodes = new(); 
    public List<InventoryNode> InventoryNodes = new();
    public List<LocalMarketBuySellNode> LocalMarketBuyNodes = new();
    public List<LocalMarketBuySellNode> LocalMarketSellNodes = new();
    public List<LocalMarketBuySellNode> LocalMarketShipNodes = new();
    public List<SitesNode> SitesNodes = new();
    public List<SystemLinkNode> SystemLinkNodes = new();
    public List<ReceipeNode> ReceipeNodes = new();
    public List<ConnectionsNode> ConnectionsNodes = new();
    public List<WorkForceNeed> WorkForceNeeds = new();
    public List<MaterialNode> MaterialNodes = new();
    public List<Production> Productions = new();

    public List<PricesNode> PricesNodesLatest = new();
    public List<WorkforceNode> WorkforceNodes = new();
}

