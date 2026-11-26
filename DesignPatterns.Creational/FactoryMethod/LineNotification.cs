namespace DesignPatterns.Creational.FactoryMethod
{
    public class LineNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"[LINE] {message}");
        }
    }
}
