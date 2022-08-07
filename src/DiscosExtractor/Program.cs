using System.Text.Json;
using DiscosExtractor.Commands.Definitions;
using DiscosWebSdk.DependencyInjection;
using DiscosWebSdk.Options;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli;
using Spectre.Cli.Extensions.DependencyInjection;

DiscosOptions options = GetOptions();

ServiceCollection                  services  = new();
services.AddDiscosServices(options.DiscosApiUrl!, options.DiscosApiKey!);

using DependencyInjectionRegistrar registrar = new(services);
CommandApp                         app       = new(registrar);

app.Configure(config =>
			  {
				  config.AddCommand<HealthCheckCommand>("healthcheck")
						.WithDescription("Check Application Health")
						.WithExample(new[] {"healthcheck"});

				  config.AddCommand<FetchObjectsCommand>("fetch")
						.WithDescription("Fetch Objects from DISCOSweb")
						.WithExample(new[] {"fetch"});

				  config.Settings.ApplicationName = "discosextractor";
			  });


app.Run(args);

const string configPath = ".discosextractorrc";
DiscosOptions GetOptions()
{
	DiscosOptions? opt = null;
	if (File.Exists(configPath))
	{
		opt = GetFromConfigFile();
	}

	if (opt is not null)
	{
		return opt;
	}

	opt = GetInteractively();

	SaveConfig(opt);

	return opt;
}

DiscosOptions? GetFromConfigFile()
{
	string         config =  File.ReadAllText(configPath);
	return JsonSerializer.Deserialize<DiscosOptions>(config);
}

DiscosOptions GetInteractively()
{
	Console.WriteLine("Please Enter The DISCOSweb URL (Leave Empty for Default)...");
	string? discosUrl = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(discosUrl)) discosUrl = "https://discosweb.esoc.esa.int/api";
	
	string? apiKey;
	do
	{
		Console.WriteLine("Please Enter Your DISCOSweb API Key...");
		apiKey = Console.ReadLine();
	}
	while (string.IsNullOrEmpty(apiKey));
		
	return new(){DiscosApiKey = apiKey, DiscosApiUrl = discosUrl};
}

void SaveConfig(DiscosOptions opt)
{
	string config = JsonSerializer.Serialize(opt);
	File.WriteAllText(configPath, config);
}
