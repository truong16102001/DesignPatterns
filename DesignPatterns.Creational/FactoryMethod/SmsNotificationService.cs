namespace DesignPatterns.Creational.FactoryMethod
{
    public class SmsNotificationService : NotificationService
    {
        protected override INotification CreateNotification()
        {
            return new SmsNotification();
        }
    }
}
