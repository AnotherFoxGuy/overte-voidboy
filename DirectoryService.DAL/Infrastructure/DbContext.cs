using System.Data;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Raven.Client.Documents;

namespace DirectoryService.DAL.Infrastructure;

[SingletonDependency]
// ReSharper disable once ClassNeverInstantiated.Global
public class DbContext
{
    private IDocumentStore _dbStore;
    public IDocumentStore Store => _dbStore;

    public DbContext()
    {

    }

    public IDocumentStore CreateConnection()
    {
       _dbStore = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "KiwiBackend_Dev",
        };
       _dbStore.Initialize();
        return _dbStore;
    }
}