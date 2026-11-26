namespace DesignPatterns.Structural.Facade
{
    public class InventoryService
    {
        private readonly int _availableStock;

        public InventoryService(int availableStock = 0)
        {
            _availableStock = availableStock;
        }

        public bool HasStock(int quantity)
        {
            return quantity > 0 && quantity <= _availableStock;
        }
    }

    public class PaymentService
    {
        public void Charge(decimal amount)
        {
            Console.WriteLine($"[Thanh toán] {amount}");
        }
    }

    public class ConfirmationService
    {
        public void Send(string email)
        {
            Console.WriteLine($"[Xác nhận] Đã gửi đến {email}");
        }
    }
}
