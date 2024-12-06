using DirectoryService.Core.Entities;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Config;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.ServerWide.Operations;

namespace DirectoryService.Tests;

public class TestBase
{
    protected ApiWebApplicationFactory? _factory;
    protected HttpClient? _client;
    private DbContext _dbContext;

    public void TestSetup()
    {
        ServiceConfigurationContainer.Config.Db = new ServiceConfiguration.DbConfig
        {
            ConnectionUrls = ["https://localhost:8080"],
            DatabaseName = "DirectoryServiceTest"
        };

        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();

        _dbContext = _factory.Services.GetRequiredService<DbContext>();

        // dbContext.RunScript("truncateAll.sql");
        // dbContext.RunScript("testSeed.sql");

        AddUsers();
    }

    private void AddUsers()
    {
        var c = _dbContext.CreateConnection();

        c.Store(new User
        {
            IdentityProvider = IdentityProvider.Local,
            Username = "testadmin",
            Email = "admin@AnotherFoxGuy.com",
            AuthVersion = 1,
            AuthHash = "$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq",
            Activated = true,
            Role = UserRole.Admin,
            State = AccountState.Normal,
            CreatorIp = "127.0.0.1",
            ConnectionGroup = "a517237f-8457-4f87-a38b-de68c274ff24",
            FriendsGroup = "ddedda75-8dd7-477f-9b91-b12efcb25522"
        }, "6465b186-5fc9-46d2-842f-da8542ba9939");
        c.Store(new User
        {
            IdentityProvider = IdentityProvider.Local,
            Username = "user",
            Email = "user@AnotherFoxGuy.com",
            AuthVersion = 1,
            AuthHash = "$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq",
            Activated = true,
            Role = UserRole.User,
            State = AccountState.Normal,
            CreatorIp = "127.0.0.1",
            ConnectionGroup = "f2714eb7-e720-4597-b404-6c3f8250fde6",
            FriendsGroup = "ddedda75-8dd7-477f-9b91-b12efcb25522"
        }, "4115ea77-3c13-4a24-9a2e-701893ee0d61");
        c.Store(new User
        {
            IdentityProvider = IdentityProvider.Local,
            Username = "user2",
            Email = "user2@AnotherFoxGuy.com",
            AuthVersion = 1,
            AuthHash = "$2a$11$Ymgtm8ExSR1gC.CvQMIl4enpTPTBO6jpMDyDgV6tPaKg4odEiqTiq",
            Activated = true,
            Role = UserRole.Admin,
            State = AccountState.Normal,
            CreatorIp = "127.0.0.1",
            ConnectionGroup = "c8bc8937-7e5c-4899-ac4e-7705a1caa71e",
            FriendsGroup = "ef6f13f8-b47c-43aa-bff4-3a920663def0"
        }, "8b63354b-4aa0-4f8c-9b91-ccb332b8939c");
        c.SaveChanges();
    }
}