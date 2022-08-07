using DiscosExtractor.Commands.Definitions;
using Spectre.Cli;

CommandApp app = new();
app.Configure(config =>
			  {
				  config.AddCommand<HealthCheckCommand>("healthcheck")
						.WithDescription("Check Application Health")
						.WithExample(new []{"healthcheck", "--auth", "xxx"})
						.WithExample(new []{"healthcheck", "-a", "xxx"});

				  config.Settings.ApplicationName = "discosextractor";
			  });

app.Run(args);
