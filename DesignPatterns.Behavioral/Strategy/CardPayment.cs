namespace DesignPatterns.Behavioral.Strategy
{
    public class CardPayment : IPaymentStrategy
    {
        public bool Pay(decimal amount)
        {
            Console.WriteLine($"[Thẻ] Thanh toán {amount}");
            return true;
        }
    }
}
