namespace Sql.ScriptRunner;

using System.Data;
using System.Data.SqlClient;

public class Worker : BackgroundService
{
    readonly ILogger<Worker> logger;
    string connectionString;
    static bool scriptsAlreadyRun;

    public Worker(ILogger<Worker> logger, string connectionString)
    {
        this.logger = logger;
        this.connectionString = connectionString;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (scriptsAlreadyRun)
        {
            await Task.Delay(10000, stoppingToken);
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            scriptsAlreadyRun = true;
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            CreateDatabase(connection);
            RunScripts(connection);
        }
    }

    void CreateDatabase(SqlConnection connection)
    {
        const string sql = @"
            IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'microservices')
            CREATE DATABASE [microservices]";

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.ExecuteNonQuery();
    }

    void RunScripts(SqlConnection connection)
    {
        connection.ChangeDatabase("microservices");
        var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Scripts", "", SearchOption.AllDirectories);

        foreach (var file in files.Where(x => x.EndsWith("schema.txt")))
        {
            logger.LogInformation(file);
            var script = File.ReadAllText(file);
            logger.LogInformation(script);
            using var command = connection.CreateCommand();
            command.CommandText = script;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
        }

        foreach (var file in files.Where(x => !x.EndsWith("schema.txt")))
        {
            logger.LogInformation(file);
            var script = File.ReadAllText(file);
            logger.LogInformation(script);
            using var command = connection.CreateCommand();
            command.CommandText = script;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
        }
    }
}
