namespace DesignPatterns.Behavioral.Strategy
{
    public interface IPaymentStrategy
    {
        bool Pay(decimal amount);
    }
}
