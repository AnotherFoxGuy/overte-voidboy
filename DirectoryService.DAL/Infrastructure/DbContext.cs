using System.Data;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace DirectoryService.DAL.Infrastructure;

[SingletonDependency]
// ReSharper disable once ClassNeverInstantiated.Global
public class DbContext
{
    private readonly string? _dbConnectionUrl;
    private readonly string? _dbName;

    public DbContext()
    {
        _dbConnectionUrl = ServiceConfigurationContainer.Config.Db!.ConnectionUrl;
        _dbName = ServiceConfigurationContainer.Config.Db!.DatabaseName;
    }

    public async Task<IAsyncDocumentSession> CreateConnectionAsync()
    {
        // var connection = new NpgsqlConnection(_dbConnectionString);
        var documentstore = new DocumentStore
        {
            Urls = new[] { _dbConnectionUrl },
            Database = _dbName,
        };
        documentstore.Initialize();
        return documentstore.OpenAsyncSession();
    }
    
    public IDocumentSession CreateConnection()
    {
        var documentstore = new DocumentStore
        {
            Urls = new[] { _dbConnectionUrl },
            Database = _dbName,
        };
        documentstore.Initialize();
        return documentstore.OpenSession();
    }

    public void RunScript(string filename)
    {
        // using var con = CreateConnection();
        // var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        // var baseDir = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "Scripts/");
        // if (!File.Exists(baseDir + "/" + filename))
        //     throw new FileNotFoundException("File not found", baseDir + "/" + filename);
        // var script = File.ReadAllText(baseDir + "/" + filename);
        // con.Execute(script);
    }

    public static void Configure()
    {

    }
}

