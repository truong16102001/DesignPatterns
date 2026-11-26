namespace DesignPatterns.Behavioral.Strategy
{
    public class BankTransferPayment : IPaymentStrategy
    {
        public bool Pay(decimal amount)
        {
            Console.WriteLine($"[Chuyển khoản] Thanh toán {amount}");
            return true;
        }
    }
}
