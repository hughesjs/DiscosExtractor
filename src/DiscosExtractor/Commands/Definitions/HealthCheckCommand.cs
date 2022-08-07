using DiscosWebSdk.Clients;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class HealthCheckCommand : AsyncCommand
{
	private readonly DiscosClient _client;
	public HealthCheckCommand(DiscosClient client) => _client = client;

	public override async Task<int> ExecuteAsync(CommandContext context)
	{
		DiscosObject sputnik = await _client.GetSingle<DiscosObject>("1");

		if (sputnik.Name.Contains("Sputnik"))
		{
			Console.WriteLine("Healthy");
			return 0;
		}

		Console.WriteLine("Something is wrong... I don't know what though, diagnostics haven't been implemented yet!");
		return 1;
	}
}
