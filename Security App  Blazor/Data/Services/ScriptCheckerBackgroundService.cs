using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime.Internal.Util;

namespace Security_App__Blazor.Data.Services;

public class ScriptCheckerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScriptCheckerBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);


    public ScriptCheckerBackgroundService(IServiceProvider serviceProvider, ILogger<ScriptCheckerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var scriptCheckerService = scope.ServiceProvider.GetRequiredService<ScriptCheckerService>();
                    var fileService = scope.ServiceProvider.GetRequiredService<FileService>();
                    var sharedResultService = scope.ServiceProvider.GetRequiredService<SharedResultService>();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                    // Get the URL file content
                    var urlFileContent = await fileService.GetUrlFile("urls.txt");

                    // Send the file content to API
                    var checkedScripts = await scriptCheckerService.UploadFileAsync(urlFileContent);

                    // Update shared results
                    sharedResultService.UpdateScripts(checkedScripts);

                    // Check for unauthorized scripts and send email
                    if (checkedScripts.Any(s => (bool)!s.Allowed))
                    {
                        var unauthSubdomains = checkedScripts.Where(s => (bool)!s.Allowed).Select(s => s.Subdomain).Distinct();
                        string subject = "Unauthorized Scripts Detected";
                        string message = $"Unauthorized scripts found on the following subdomains: {string.Join(", ", unauthSubdomains)}";
                        await emailService.SendEmailAsync("tester@example.com", subject, message, message);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background service execution.");
                }
            }


            }

            // Delay before the next iteration
            await Task.Delay(_interval, stoppingToken);
        }
    }

}
