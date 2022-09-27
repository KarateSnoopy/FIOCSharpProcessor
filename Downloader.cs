using System.Net.Http.Headers;

class Downloader
{
    private async static Task DownloadFile(string path, string dirPath, System.Net.Http.HttpClient client, bool addDate = false, bool addcsv = true, bool overwrite = false)
    {
        string fileName = path.Replace("/", "_");
        if (fileName.Contains("?"))
        {
            fileName = path.Substring(0, path.IndexOf("?"));
        }

        if (addDate)
        {
            DateTime now = DateTime.Now;
            string dateStr = string.Format(" - {0:MM-dd-yy HH-00}", now);
            fileName += dateStr;

            dirPath += "\\history";
        }

        if (addcsv)
            fileName += ".csv";
        else
            fileName += ".json";

        string filePath = Path.Combine(dirPath, fileName);

        if (!IsFileFound(fileName, dirPath, overwrite))
        {
            string fullpath = $"/csv/{path}";
            if (!addcsv)
            {
                fullpath = $"/{path}";
            }

            var response = await client.GetAsync(fullpath);
            string msg = await response.Content.ReadAsStringAsync();
            msg = msg.Replace("\r", "");
            var lines = msg.Split("\n");

            Console.WriteLine($"Caching FIO data to {filePath}");
            File.WriteAllLines(filePath, lines);
        }
    }

    public static bool IsFileFound(string file, string folder, bool ignoreOld = false)
    {
        string orgFilePart = file.Substring(0, file.LastIndexOf('.'));
        if (orgFilePart.Contains('('))
        {
            orgFilePart = orgFilePart.Substring(0, file.LastIndexOf('('));
        }
        orgFilePart = orgFilePart.Trim();

        var files = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories);

        foreach (string currentFile in files)
        {
            FileInfo fi = new FileInfo(currentFile);
            string namePart = fi.Name;
            if (fi.Name.Contains("."))
            {
                namePart = fi.Name.Substring(0, fi.Name.LastIndexOf('.'));
            }

            if (namePart.Contains('('))
            {
                namePart = namePart.Substring(0, namePart.LastIndexOf('('));
            }
            namePart = namePart.Trim();

            if (string.Compare(orgFilePart, namePart, true) == 0)
            {
                if (ignoreOld)
                {
                    TimeSpan ts = DateTime.Now - fi.LastWriteTime;
                    if (ts.TotalMinutes > 60)
                    {
                        File.Delete(fi.FullName);
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public async static Task DownloadData(string dirPath, List<string> planetsToMonitor, string apiKey)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://rest.fnar.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/csv"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", apiKey);

            await DownloadFile("buildings", dirPath, client); // DI-Buildings
            await DownloadFile("buildingcosts", dirPath, client); // DI-BuildingCosts
            await DownloadFile("buildingrecipes", dirPath, client); // DI-BuildingRecipes
            await DownloadFile("buildingworkforces", dirPath, client); // DI-BuildingWorkforce
            await DownloadFile("materials", dirPath, client); // DI-Materials
            await DownloadFile("planetdetail", dirPath, client); // Planet-Detail, Planet-Parameters, Planet-Orbits
            await DownloadFile("planetproductionfees", dirPath, client); // Planet-Production-Fees
            await DownloadFile("planetresources", dirPath, client); // Planet-Resources
            await DownloadFile("planets", dirPath, client); // Planet-Names
            await DownloadFile("recipeinputs", dirPath, client); // DI-RecipeInputs
            await DownloadFile("recipeoutputs", dirPath, client); // DI-RecipeOutputs
            await DownloadFile("systemplanets", dirPath, client);  // Planet-Detail
            await DownloadFile("systems", dirPath, client); // System-Stars
            await DownloadFile("infrastructure/allinfos", dirPath, client); // ??
            await DownloadFile("infrastructure/allreports", dirPath, client); // ??
            await DownloadFile($"material/allmaterials", dirPath, client, false, false);
            await DownloadFile($"recipes/allrecipes", dirPath, client, false, false);
            await DownloadFile($"systemstars", dirPath, client, false, false);
            await DownloadFile($"global/workforceneeds", dirPath, client, false, false);
            await DownloadFile($"rain/systemlinks", dirPath, client, false, false); // System-Connections

            foreach (var planet in planetsToMonitor)
            {
                await DownloadFile($"localmarket/buy/{planet}", dirPath, client, false, true, true); // local market buy 
                await DownloadFile($"localmarket/sell/{planet}", dirPath, client, false, true, true); // local market sell
                await DownloadFile($"localmarket/ship/{planet}", dirPath, client, false, true, true); // // local market shipping
            }

            await DownloadFile("bids", dirPath, client, true); // CX-BidOrders
            await DownloadFile("prices", dirPath, client, true); // CX-Prices
            await DownloadFile("orders", dirPath, client, true); // CX-AskOrders
            await DownloadFile($"inventory?apikey={apiKey}&username=KarateSnoopy", dirPath, client, false, true, true);
            await DownloadFile($"burnrate?apikey={apiKey}&username=KarateSnoopy", dirPath, client, false, true, true);
            await DownloadFile($"sites?apikey={apiKey}&username=KarateSnoopy", dirPath, client, false, true, true);
            await DownloadFile($"workforce?apikey={apiKey}&username=KarateSnoopy", dirPath, client, false, true, true);
            await DownloadFile($"production/KarateSnoopy", dirPath, client, false, false, true);
        }
    }
}

