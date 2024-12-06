
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;
using Raven.Client.Documents;

namespace DirectoryService.DAL;

[ScopedDependency]
public class EmailQueueEntityRepository(DbContext db) : BaseRepository<QueuedEmail>(db), IEmailQueueEntityRepository
{
    public async Task<QueuedEmail> Create(QueuedEmail entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }


    public async Task<IEnumerable<QueuedEmail>> GetNextQueuedEmails(int limit = 1000)
    {
        using var con = DbContext.CreateConnectionAsync();
        // var emails = await con.QueryAsync<QueuedEmail>(
        //     @"SELECT * FROM emailQueue WHERE sent = FALSE
        //          AND sendOn < CURRENT_TIMESTAMP ORDER BY sendOn LIMIT @limit",
        //     new
        //     {
        //         limit
        //     });    
        var emails = await con.Query<QueuedEmail>()
            .Where(x => !x.Sent && x.SentOn < DateTime.Now)
            .Take(limit)
            .ToListAsync();
        
        return emails;
    }
    
    public async Task<QueuedEmail?> Update(QueuedEmail entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public async Task ClearSentEmails(DateTime cutoffDate)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(
        //     @"DELETE FROM emailQueue WHERE sent=TRUE AND sentOn < @cutoffDate", new
        //     {
        //         cutoffDate
        //     });
        var emails =  await con.Query<QueuedEmail>()
            .Where(x => x.Sent && x.SentOn < cutoffDate)
            .ToListAsync();
        con.Delete(emails);
        await con.SaveChangesAsync();
    }
}