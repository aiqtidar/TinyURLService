using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TinyURLService.Service.URLService;
using TinyURLService.Service.URLGeneratorService;
using TinyURLService.Data.Repositories;
using TinyURLService.Domain.TrieForURLs;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static IConfigurationRoot Configuration { get; set; }

    public static void Main(string[] args)
    {
        // Just a dummy read operation for now
        // TODO: Move this to a different project so that we have a common configuration for all
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // COnfigure services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var app = serviceProvider.GetService<BasicConsoleApplication>();
        app?.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        

        services.AddSingleton<ITrieNode<string, int>, TrieNode>();
        services.AddSingleton<ITrie<string, int>, Trie>();
        services.AddSingleton<IRepository<bool>, InMemoryRepository>();
        services.AddTransient<IURLGeneratorService, URLGeneratorService>();
        services.AddSingleton<IURLService, URLService>();
        services.AddTransient<BasicConsoleApplication>();
    }
}

public class BasicConsoleApplication(IURLService urlService)
{
    private readonly IURLService _urlService = urlService;

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("Welcome to a tiny TinyURL Service!");
        Console.WriteLine();

        bool continueRunning = true;

        while (continueRunning)
        {
            // Prompt the user for input
            PresentOptions();

            // Read user input
            int userInput = ReadOptions();

            if (userInput == 0) break;

            PerformAction(userInput);
        }

        Console.WriteLine("Goodbye!");
    }

    private void PerformAction(int input)
    {
        Console.Clear();

        switch (input)
        {
            case 1:
                Process_1();
                break;
            case 2:
                Process_2();
                break;
            case 3:
                Process_3();
                break;
            case 4:
                Process_4();
                break;
            case 5:
                Process_5();
                break;
            case 6:
                Process_6();
                break;
            case 7:
                Process_7();
                break;
            case 8:
                Process_8();
                break;
            default:
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Press Any Key to Continue...");
        Console.ReadKey();
    }

    private void Process_1()
    {
        try
        {
            string shortUrl = _urlService.CreateTinyUrlFromUrl(ParseUri());

            if (string.IsNullOrEmpty(shortUrl)) throw new ArgumentException("Unknown Error - Please try again...");

            Console.WriteLine();
            Console.WriteLine($"TinyURL Generated Successfully: {shortUrl}");
            Console.WriteLine();
            return;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"{ex.Message}");
            return;
        }
        
    }

    private void Process_2()
    {
        string? input;

        Uri uri = ParseUri();

        while (true)
        {
            Console.WriteLine("Please provide the custom path for your tiny URL: (The generated URL will be https://tinyUrlDomain.com/<Your custom Path>");
            input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is invalid - Please try again.");
                Console.WriteLine();
                continue;
            }
            else if (_urlService.DoesTinyUrlExist(new Uri("https://tinyUrlDomain.com/" + input)))
            {
                Console.WriteLine("This URL is taken - Please try another");
                Console.WriteLine();
                continue;
            }

            break;
        }
            
        Console.WriteLine();

        if (!_urlService.CreateTinyUrlFromUrl(uri, input)) Console.WriteLine("An unexpected error occurred...");
        else Console.WriteLine($"Url has been created successfully at https://tinyUrlDomain.com/{input}");

        return;
    }

    private void Process_3()
    {
        Uri uri = ParseUri();

        var urls = _urlService.GetTinyUrlFromUrl(uri);

        Console.WriteLine($"{urls.Count} urls found");


        foreach (var url in urls)
        {
            Console.WriteLine(url.ToString());
        };

        return;
    }

    private void Process_4()
    {
        Uri tinyUri = ParseUri();

        string url = _urlService.GetUrlFromTinyUrl(tinyUri);

        if (string.IsNullOrEmpty(url)) Console.WriteLine("No associated URL found");
        else Console.WriteLine(url);

        return;
    }

    private void Process_5()
    {
        Uri uri = ParseUri();

        string p= _urlService.GetPopularityOfTinyUrl(uri);

        if (string.IsNullOrEmpty(p)) Console.WriteLine("No associated URL found");
        else Console.WriteLine($"There have been {p} hits on this URL");

        return;
    }

    private void Process_6()
    {
        Uri uri = ParseUri();

        if (!_urlService.DeleteTinyUrl(uri)) Console.WriteLine("Delete Failed");
        else Console.WriteLine("Delete successfull!");

        return;
    }
    private void Process_7()
    {
        Uri uri = ParseUri();

        if (!_urlService.DeleteAllTinyUrlsFromUrl(uri)) Console.WriteLine("Delete Failed");
        else Console.WriteLine("Delete successfull!");

        return;
    }

    private void Process_8()
    {
        Uri uri = ParseUri();

        if (!_urlService.AddHitToTinyUrl(uri)) Console.WriteLine("Failed to add hit");
        else Console.WriteLine("Hit has been added");

        return;
    }

    private static Uri ParseUri()
    {
        Console.WriteLine("Please provide your URL: ");

        Uri uri;
        while (true)
        {
            try
            {
                uri = ParseUri(Console.ReadLine());
                return uri;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }


    private int ReadOptions()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int res) && (res >= 0 && res <= 8) ) return res;
            Console.WriteLine("Input is invalid - Please provide correct integer value between 1 and 8!");
            Console.WriteLine();
        }
    }

    private void PresentOptions()
    {
        Console.Clear();
        Console.WriteLine("Please select one of the options below by inputting its integer value: ");
        Console.WriteLine("------------------------------------------------------------------");
        Console.WriteLine("1 :: CREATE a random Tiny URL");
        Console.WriteLine("2 :: CREATE a custom Tiny URL");
        Console.WriteLine("3 :: GET Tiny URLs of an URL (from memory)");
        Console.WriteLine("4 :: GET the associated URL for a Tiny URL (from memory)");
        Console.WriteLine("5 :: GET the popularity (hits) of a Tiny URL (from memory)");
        Console.WriteLine("6 :: DELETE a Tiny URL");
        Console.WriteLine("7 :: DELETE all Tiny URLs for a URL");
        Console.WriteLine("8 :: Add a hit to a Tiny URLs (increase popularity by one)");
        Console.WriteLine("0 :: Exit");
    }

    // We both know Users are going to put in a ton of wrong inputs for the URL.
    // I added some basic validation for now.
    private static Uri ParseUri(string? uri)
    {
        if (string.IsNullOrEmpty(uri)) throw new ArgumentException("Unable to parse URL - Please validate and retype URL.");

        StringBuilder builder = new StringBuilder();

        // Set the default Scheme
        if (!uri.StartsWith("http")) builder.Append("https://");

        // Put in the rest of the URL for now
        builder.Append(uri);

        if (!Uri.TryCreate(builder.ToString(), UriKind.Absolute, out Uri? res) && res == null) throw new ArgumentException("Unable to parse URL - Please validate and retype URL.");

        return res;
    }
}