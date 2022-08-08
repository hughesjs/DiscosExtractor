using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using JetBrains.Annotations;
using Spectre.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class FetchObjectsCommand: AsyncCommand
{
	public event EventHandler<DownloadStatus>? DownloadStatusChanged;
	
	private readonly IBulkFetchService<DiscosObject> _bulkFetchService;

	public FetchObjectsCommand(IBulkFetchService<DiscosObject> bulkFetchService)
	{
		_bulkFetchService                       =  bulkFetchService;
		_bulkFetchService.DownloadStatusChanged += DownloadStatusChanged;
	}

	public override async Task<int> ExecuteAsync(CommandContext context)
	{
		Console.WriteLine("Fetching data from DISCOSweb... There may be some pauses as this is a rate-limited API!");
		await _bulkFetchService.GetAll();
		return 0;
	}
}
