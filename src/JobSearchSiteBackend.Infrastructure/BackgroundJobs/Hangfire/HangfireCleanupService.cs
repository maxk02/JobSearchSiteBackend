using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire;

public class HangfireCleanupService(IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration["HANGFIRE_DEFAULT_SQL_SERVER_CONNECTION_STRING"];

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = connection.CreateCommand();
        command.CommandText = @"
            TRUNCATE TABLE [HangFire].[AggregatedCounter];
            TRUNCATE TABLE [HangFire].[Counter];
            TRUNCATE TABLE [HangFire].[JobParameter];
            TRUNCATE TABLE [HangFire].[JobQueue];
            TRUNCATE TABLE [HangFire].[List];
            TRUNCATE TABLE [HangFire].[Set];
            TRUNCATE TABLE [HangFire].[State];
            TRUNCATE TABLE [HangFire].[Hash];
            TRUNCATE TABLE [HangFire].[Job];
        ";

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}