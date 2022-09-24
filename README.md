# FIOCSharpProcessor

A C# tool for Prosperous Universe that I made that might be useful for some folks with better C# skills than Excel skills

It downloads and caches almost all of Prosperous Universe's FIO CSV reports to disk, parses the data, and processes it in to various CSV reports.  See https://github.com/KarateSnoopy/FIOCSharpProcessor/tree/main/csv/out for various reports it currently creates.  

processed-receipes.csv is the most interesting one (it has lots of computed columns).  It updates to latest market data every hour but you can adjust as desired.

You can modify it as desired to answer various questions using C# LINQ, etc.  I'm using VS2022 to compile it but it should compile via other tools but didn't try.  It's has helped me as a noob try to come up with a plan based on market data and so on.

To use pass in your FIO API key as a param.
To get FIO API key, go to https://fio.fnar.net/
Sign in and click settings and create API key.
No need to enable 'allow writes' on the API key
