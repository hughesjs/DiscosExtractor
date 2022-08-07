using DiscosExtractor.Commands.Settings;
using DiscosWebSdk.Clients;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Definitions;

public abstract class DiscosCommandBase<T>: AsyncCommand<T> where T:DiscosCommandSettingsBase
{
	protected DiscosClient GetClient(T settings)
	{
		HttpClient innerClient = new() {
										   BaseAddress           = new(GlobalConsts.DiscosApiUrl),
										   DefaultRequestHeaders = { Authorization = new("bearer", settings.AuthKey)}
									   };
		return new(innerClient);
	}

	public abstract override Task<int> ExecuteAsync(CommandContext context, T settings);
}
