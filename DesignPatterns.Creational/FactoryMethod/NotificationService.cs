namespace DesignPatterns.Creational.FactoryMethod
{
    public abstract class NotificationService
    {
        // Lớp con quyết định tạo EmailNotification, LineNotification hay một lớp gửi khác.
        // Đối tượng trả về phải triển khai INotification.
        protected abstract INotification CreateNotification();

        public void Notify(string message)
        {
            // Lấy đối tượng do lớp con tạo rồi gọi Send. Đối tượng đó tự gửi qua kênh của mình.
            var notification = CreateNotification();
            notification.Send(message);
        }
    }
}
