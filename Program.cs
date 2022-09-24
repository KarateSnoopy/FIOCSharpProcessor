using System.Reflection;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey;
        if ( args.Length == 0 )
        {
            string config = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", @"..\..\..\config.txt");
            if( File.Exists(config) )
            {
                string[] configLines = File.ReadAllLines(config);
                apiKey = configLines[0];
            }
            else
            {
                Console.WriteLine("Pass in FIO API key as param");
                Console.WriteLine("To get FIO API key, go to https://fio.fnar.net/");
                Console.WriteLine("sign in and click settings and create API key.");
                Console.WriteLine("No need to enable 'allow writes' on API key");
                return;
            }
        }
        else
        {
            apiKey = args[0];
        }

        List<string> planetsToMonitor = new List<string>()
        {
            // These planets have 5+ orders, so download them
            "TD-260c",
            "RC-040b",
            "WC-702b",
            "PD-518b",
            "XH-594a",
            "TD-203b",
            "ZV-896b",
            "OT-580c",
            "QQ-001b",
            "LS-300c",
            "OT-442b",
            "AM-783c",
            "ZV-194a",
            "XH-594b",
            "UV-351a",
            "PG-899b",
            "OT-580b",
            "ZV-759c",
            "KW-688c",
            "VH-331a",
            "UV-796b",
        };
      
        string? exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (exePath == null) return;
        string dataPath = Path.Combine(exePath, @"..\..\..\csv"); // exepath it something like "...\bin\Debug\net6.0" so path back to csv folder
        DirectoryInfo di = new DirectoryInfo(dataPath);
        dataPath = di.FullName; // collapse path

        // Download CSV & JSON data from FIO REST API
        // Downloads are cached to CSV & JSON files.
        // Some FIO data doesn't change so no need to re-downloaded.
        // Other FIO data such as market data is downloaded if its older than hour old.
        Console.WriteLine("Refreshing FIO data...");
        await Downloader.DownloadData(dataPath, planetsToMonitor, apiKey);

        // Read the CSV and JSON files
        Console.WriteLine("Parsing FIO data...");
        GameState gs = CsvReader.ReadData(dataPath, planetsToMonitor);
        JsonReader.ReadData(dataPath, gs);

        // Process the data
        Console.WriteLine("Processing FIO data...");
        string outPath = Path.Combine(dataPath, "out");
        Processor.Process(outPath, gs);

        Console.WriteLine("Done.  See csv\\out folder for CSV output");
    }
}
