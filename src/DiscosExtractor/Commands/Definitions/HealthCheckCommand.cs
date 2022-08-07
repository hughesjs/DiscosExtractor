using DiscosExtractor.Commands.Settings;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class HealthCheckCommand : DiscosCommandBase<HealthCheckCommandSettings>
{
	public override async Task<int> ExecuteAsync(CommandContext context, HealthCheckCommandSettings settings)
	{
		DiscosClient client  = GetClient(settings);
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
