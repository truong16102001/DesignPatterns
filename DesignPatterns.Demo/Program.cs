using DesignPatterns.Creational.FactoryMethod;

NotificationService email = new EmailNotificationService();
NotificationService sms = new SmsNotificationService();
NotificationService line = new LineNotificationService();

email.Notify("Đơn hàng đã được xác nhận");
sms.Notify("Đơn hàng đã được xác nhận");
line.Notify("Đơn hàng đã được xác nhận");
