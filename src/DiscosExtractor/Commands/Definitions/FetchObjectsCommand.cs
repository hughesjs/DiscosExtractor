using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class FetchObjectsCommand: AsyncCommand
{
	
	private readonly IBulkFetchService<DiscosObject> _bulkFetchService;
	private ProgressTaskSettings            _progressTaskSettings;

	public FetchObjectsCommand(IBulkFetchService<DiscosObject> bulkFetchService)
	{
		_progressTaskSettings = new();
		
		_bulkFetchService                       =  bulkFetchService;
	}
	
	public override async Task<int> ExecuteAsync(CommandContext context)
	{

		List<DiscosObject> objects;
		await AnsiConsole.Progress()
						 .StartAsync(async ctx =>
									 {
										 // Define tasks
										 ProgressTask objectDownload = ctx.AddTask("[green]Downloading DISCOS Objects[/]");

										 _bulkFetchService.DownloadStatusChanged += (_, status) =>
																						{
																							objectDownload.MaxValue = status.Total;
																							objectDownload.Increment(status.Downloaded - objectDownload.Value);
																						};
											 
										 objects = await _bulkFetchService.GetAll();
									 });

		return 0;
	}
}
