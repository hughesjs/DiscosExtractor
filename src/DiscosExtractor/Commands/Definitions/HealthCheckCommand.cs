using DiscosWebSdk.Clients;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class HealthCheckCommand : AsyncCommand
{
	private readonly IDiscosClient _client;
	public HealthCheckCommand(IDiscosClient client) => _client = client;

	public override async Task<int> ExecuteAsync(CommandContext context)
	{
		DiscosObject sputnik = await _client.GetSingle<DiscosObject>("1");
		
		if (sputnik.Name.Contains("Sputnik"))
		{
			Console.WriteLine("Healthy");
			return 0;
		}
		
		throw new("Something is wrong... I don't know what though, diagnostics haven't been implemented yet!");
	}
}
