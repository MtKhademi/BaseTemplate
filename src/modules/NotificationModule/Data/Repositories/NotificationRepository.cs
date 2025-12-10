namespace NotificationModule.Data.Repositories;

internal class NotificationRepository(DbContext context) : 
    EFBaseRepository<int, NotificationEntity>(context), INotificationRepository
{
   
}
