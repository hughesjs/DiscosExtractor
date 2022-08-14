using System.Collections.Concurrent;
using System.Text.Json;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiscosExtractor.Commands.Definitions;

[UsedImplicitly]
public class FetchObjectsCommand: AsyncCommand
{
	private readonly IBulkFetchService _bulkFetchService;

	public FetchObjectsCommand(IBulkFetchService bulkFetchService)
	{
		_bulkFetchService                       =  bulkFetchService;
	}
	
	public override async Task<int> ExecuteAsync(CommandContext context)
	{
		Dictionary<Type, List<DiscosModelBase>> results = new();
		await AnsiConsole.Progress()
						 .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
						 .StartAsync(async ctx =>
									 {
										 Dictionary<Type, ProgressTask> progressTasks = new();
										 foreach (Type t in typeof(DiscosModelBase).Assembly.GetTypes().Where(t => t.IsDiscosModel()))
										 {
											 progressTasks.Add(t, ctx.AddTask($"[green]Downloading {t.Name} [/]", false));
										 }

										 foreach (KeyValuePair<Type, ProgressTask> progressTask in progressTasks)
										 {
											 void CallbackFunc(object? _, DownloadStatus status)
											 {
												 progressTask.Value.MaxValue = status.Total;
												 progressTask.Value.Increment(status.Downloaded - progressTask.Value.Value);
											 }

											 _bulkFetchService.DownloadStatusChanged += CallbackFunc;
											 progressTask.Value.StartTask();
											 results.Add(progressTask.Key, await _bulkFetchService.GetAll(progressTask.Key));
											 progressTask.Value.Value = progressTask.Value.MaxValue;
											 progressTask.Value.StopTask();
											 _bulkFetchService.DownloadStatusChanged -= CallbackFunc;
										 }
									 });

		foreach (KeyValuePair<Type, List<DiscosModelBase>> result in results)
		{
			await using FileStream fStream = new($"discos-{result.Key.Name}-{DateTime.Now:yyyyMMdd}.json", FileMode.OpenOrCreate);
			await JsonSerializer.SerializeAsync(fStream, result.Value);
		}
	
		return 0;
	}
}
