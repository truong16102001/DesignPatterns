using DesignPatterns.Behavioral.Strategy;
using DesignPatterns.Creational.Singleton;

namespace DesignPatterns.Structural.Facade
{
    public sealed class CheckoutService
    {
        private static CheckoutService? _instance;
        private static readonly object _lock = new object();

        private readonly InventoryService _inventory;
        private readonly PaymentService _payment;
        private readonly ConfirmationService _confirmation;

        private CheckoutService()
        {
            _inventory = new InventoryService(5);
            _payment = new PaymentService();
            _confirmation = new ConfirmationService();
        }

        public static CheckoutService Instance
        {
            get
            {
                // Giữ bước kiểm tra và khởi tạo trong cùng một khóa để tránh tạo hai instance.
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CheckoutService();
                    }
                    return _instance;
                }

            }
        }

        // Facade: bên gọi chỉ dùng PlaceOrder(). Method này điều phối kho, thanh toán và xác nhận.
        public bool PlaceOrder(string email, int quantity, decimal total, IPaymentStrategy payment)
        {
            if (!_inventory.HasStock(quantity))
            {
                return false;
            }

            // Strategy: gọi cùng một Pay(), còn đối tượng được truyền vào sẽ thanh toán theo cách của nó.
            if (!payment.Pay(total))
            {
                return false;
            }

            _confirmation.Send(email);
            return true;
        }
    }
}
