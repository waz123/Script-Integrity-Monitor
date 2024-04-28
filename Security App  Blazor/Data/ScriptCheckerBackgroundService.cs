﻿namespace Security_App__Blazor.Data;

public class ScriptCheckerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public ScriptCheckerBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
                {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scriptCheckerService = scope.ServiceProvider.GetRequiredService<ScriptCheckerService>();
                var fileService = scope.ServiceProvider.GetRequiredService<FileService>();

                // Get the URL file content
                var urlFileContent = await fileService.GetUrlFile("urls.txt");

                // Send the file content to API
                var checkedScripts = await scriptCheckerService.UploadFileAsync(urlFileContent);

            }

            // Delay before the next iteration
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
