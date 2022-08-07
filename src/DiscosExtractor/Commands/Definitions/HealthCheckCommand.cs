using DiscosExtractor.Commands.Settings;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Definitions;

public class HealthCheckCommand: AsyncCommand<HealthCheckCommandSettings>
{
	public override async Task<int> ExecuteAsync(CommandContext context, HealthCheckCommandSettings settings)
	{
		HttpClient   innerClient = new() {
											 BaseAddress = new(GlobalConsts.DiscosApiUrl),
											 DefaultRequestHeaders = { Authorization = new("bearer", settings.AuthKey)}
										 };
		DiscosClient client  = new(innerClient);
		DiscosObject sputnik = await client.GetSingle<DiscosObject>("1");
		
		if (sputnik.Name.Contains("Sputnik"))
		{
			Console.WriteLine("Healthy");
			return 0;
		}
		
		Console.WriteLine("Something is wrong... I don't know what though, diagnostics haven't been implemented yet!");
		return 1;
	}
}
