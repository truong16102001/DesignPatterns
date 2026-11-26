using DesignPatterns.Creational.FactoryMethod;
using DesignPatterns.Creational.Singleton;
using DesignPatterns.Structural.Adapter;
using DesignPatterns.Structural.Facade;

var first = ApplicationSettings.Instance;
var second = ApplicationSettings.Instance;

Console.WriteLine(object.ReferenceEquals(first, second)); // kì vọng là true

NotificationService email = new EmailNotificationService();
NotificationService sms = new SmsNotificationService();
NotificationService line = new LineNotificationService();

email.Notify("Đơn hàng đã được xác nhận");
sms.Notify("Đơn hàng đã được xác nhận");
line.Notify("Đơn hàng đã được xác nhận");

INotification legacySms = new LegacySmsAdapter(new LegacySmsSender());
legacySms.Send("Đơn hàng đã được xác nhận");

var checkout = CheckoutService.Instance;
var result = checkout.PlaceOrder("demo@example.com", 2, 150_000m);
if(result) Console.WriteLine("Order successfully");
