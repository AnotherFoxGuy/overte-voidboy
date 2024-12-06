using System.Data;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide.Operations;

namespace DirectoryService.DAL.Infrastructure;

[SingletonDependency]
// ReSharper disable once ClassNeverInstantiated.Global
public class DbContext
{
    private readonly string[]? _dbConnectionUrls;
    private readonly string? _dbName;
    
    private DocumentStore _store;

    public DbContext()
    {
        _dbConnectionUrls = ServiceConfigurationContainer.Config.Db!.ConnectionUrls ??  ["http://localhost:8080"];
        _dbName = ServiceConfigurationContainer.Config.Db!.DatabaseName ?? "DirectoryServiceTest";
        _store = new DocumentStore
        {
            Urls = _dbConnectionUrls,
            Database = _dbName,
        };
        _store.Initialize();
    }

    public IAsyncDocumentSession CreateConnectionAsync() => _store.OpenAsyncSession();
    public IDocumentSession CreateConnection() => _store.OpenSession();

    public static void Configure()
    {

    }
}

