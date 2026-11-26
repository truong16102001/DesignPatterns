namespace DesignPatterns.Creational.FactoryMethod
{
    public class SmsNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"[Sms] {message}");
        }
    }
}
