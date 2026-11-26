# DesignPatterns

## Factory Method

### Mục đích sử dụng

Dùng Factory Method khi nhiều chức năng có cùng luồng xử lý nhưng cần tạo các loại đối tượng khác nhau. Ví dụ, gửi thông báo qua Email và SMS dùng chung bước `Notify(message)`. Lớp con quyết định tạo đối tượng gửi Email hay SMS.

### Đặc trưng khi triển khai

1. Tạo interface `INotification` với phương thức `Send(string message)`.
2. Tạo `EmailNotification` và `SmsNotification` triển khai `INotification`. Mỗi lớp xử lý việc gửi qua kênh của mình.
3. Tạo lớp cha `NotificationService` và khai báo `protected abstract INotification CreateNotification()`. Đây là **Factory Method**. Trong `Notify(message)`, gọi `CreateNotification()` rồi gọi `Send(message)` trên đối tượng nhận được.
4. Tạo `EmailNotificationService` và `SmsNotificationService` kế thừa `NotificationService`. Mỗi lớp `override CreateNotification()` để tạo đối tượng tương ứng.

### Luồng chạy

1. Code gọi chọn `EmailNotificationService` và gọi `Notify("Đơn hàng đã được xác nhận")`.
2. `Notify()` trong lớp cha gọi `CreateNotification()`. Bản override của `EmailNotificationService` tạo `EmailNotification`.
3. `Notify()` gọi `Send(message)` qua `INotification`. Nếu bước 1 chọn `SmsNotificationService`, cùng luồng này sẽ gửi SMS.

### Kiểu trả về của Factory Method

`CreateNotification()` trả về kiểu `INotification`, nên mỗi lớp con của `NotificationService` có thể trả về đối tượng thuộc bất kỳ lớp nào triển khai interface này.

Ví dụ, `return new LineNotification();` hợp lệ vì `LineNotification` triển khai `INotification`. Nếu bỏ `: INotification` khỏi lớp `LineNotification`, dòng `return` sẽ lỗi biên dịch, kể cả khi lớp vẫn có method `Send()`.

### Ưu điểm

- `Notify()` làm việc qua `INotification`, nên luồng gửi chung dùng được với nhiều kênh.
- Ví dụ LINE cho thấy có thể thêm kênh mới mà vẫn dùng nguyên `Notify()`.

### Nhược điểm

- Có thêm lớp phải duy trì. Trong ví dụ này, mỗi kênh có một lớp gửi và một lớp tạo đối tượng gửi.

### Ứng dụng thực tế

- Gửi thông báo qua Email, SMS hoặc Push.
- Xuất cùng một báo cáo sang PDF hoặc CSV.
