namespace DesignPatterns.Creational.FactoryMethod
{
    public class EmailNotificationService : NotificationService
    {
        protected override INotification CreateNotification()
        {
            return new EmailNotification();
        }
    }
}
