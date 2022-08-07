using DiscosWebSdk.Clients;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Settings;

public abstract class DiscosCommandSettingsBase: CommandSettings
{
	[CommandOption("-a|--auth")]
	public string? AuthKey { get; set; }


}
