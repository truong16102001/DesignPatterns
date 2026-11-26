namespace DesignPatterns.Creational.FactoryMethod
{
    public class LineNotificationService : NotificationService
    {
        protected override INotification CreateNotification()
        {
            return new LineNotification();
        }
    }
}
