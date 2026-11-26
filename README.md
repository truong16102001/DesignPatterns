# DesignPatterns

## Singleton

### Mục đích sử dụng

Dùng Singleton khi một lớp cần đúng một instance để các phần của ứng dụng cùng sử dụng, ví dụ cấu hình chỉ đọc [`ApplicationSettings`](DesignPatterns.Creational/Singleton/ApplicationSettings.cs).

Ví dụ, màn hình chính và màn hình Giới thiệu đều có thể đọc `ApplicationSettings.Instance.ApplicationName` để hiển thị tên ứng dụng từ cùng một đối tượng cấu hình.

### Đặc trưng khi triển khai

1. Đặt constructor là `private` để code bên ngoài lớp không thể gọi `new ApplicationSettings()`.
2. Khai báo field `private static ApplicationSettings? _instance` để lưu đối tượng dùng chung. Field bắt đầu là `null` và được gán khi `Instance` được đọc lần đầu.
3. Cung cấp property `public static ApplicationSettings Instance` để code bên ngoài lấy đối tượng đã lưu.
4. Tạo một đối tượng khóa dùng chung bằng `private static readonly object _lock = new object()`. Đặt cả bước kiểm tra `_instance == null` và bước tạo đối tượng bên trong `lock (_lock)` để hai luồng không thể cùng thấy `_instance` là `null` rồi cùng tạo đối tượng.

### Luồng chạy

1. Luồng A đọc `Instance`, lấy khóa `_lock`, thấy `_instance` là `null` và tạo `ApplicationSettings`.
2. Nếu luồng B đọc `Instance` trong lúc A đang giữ khóa, B chờ ở `lock (_lock)`.
3. A trả về đối tượng và tự nhả khóa khi rời khối `lock`. B lấy khóa, thấy `_instance` đã được gán và trả về cùng đối tượng.

### Ưu điểm

- Kiểm soát việc tạo đối tượng. Constructor `private` và `lock` bảo đảm các lần gọi `Instance` nhận cùng một đối tượng.
- Có điểm truy cập chung qua `ApplicationSettings.Instance`. Ví dụ, màn hình chính và màn hình Giới thiệu dùng cùng một cấu hình.
- Bản dùng `lock` trong demo chỉ tạo đối tượng khi `Instance` được đọc lần đầu.

### Nhược điểm

- Có thể làm lớp gánh hai trách nhiệm. `ApplicationSettings` vừa cung cấp cấu hình vừa quản lý việc tạo và chia sẻ instance. (Vi phạm Single Responsibility Principle.)
- **Thay đổi ở một nơi có thể ảnh hưởng đến nơi khác.** Nếu sau này cho phép sửa `ApplicationName`, màn hình Cài đặt đổi tên ứng dụng thì màn hình Giới thiệu cũng đọc được tên mới vì cả hai dùng chung `ApplicationSettings.Instance`.

### Ứng dụng thực tế

Vì class dùng Singleton chỉ tồn tại 1 Instance (thể hiện) nên nó thường được dùng cho các trường hợp giải quyết các bài toán cần truy cập vào các ứng dụng như: Shared resource, Logger, Configuration, Caching, Thread pool, …

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

## Adapter

### Mục đích sử dụng

Dùng Adapter để kết nối code hiện tại với một lớp có API khác. Trong demo, ứng dụng gọi `INotification.Send(message)`, còn thư viện SMS cũ chỉ có `SendText(message)`. Adapter nối hai cách gọi này mà không cần sửa thư viện cũ.

### Đặc trưng khi triển khai

1. Dùng `INotification` làm interface mà ứng dụng mong đợi.
2. [`LegacySmsSender`](DesignPatterns.Structural/Adapter/LegacySmsSender.cs) mô phỏng thư viện cũ và cung cấp `SendText(message)`.
3. [`LegacySmsAdapter`](DesignPatterns.Structural/Adapter/LegacySmsAdapter.cs) triển khai `INotification` và giữ một `LegacySmsSender` được truyền vào constructor. Khi nhận `Send(message)`, Adapter gọi `_sender.SendText(message)`.

### Luồng chạy

1. `Program.cs` gọi `legacySms.Send("Đơn hàng đã được xác nhận")` qua `INotification`.
2. `LegacySmsAdapter` chuyển lời gọi sang `LegacySmsSender.SendText(message)`.
3. Console in `[Legacy SMS] Đơn hàng đã được xác nhận`.

### Ưu điểm

- Dùng lại code hoặc thư viện cũ và giữ nguyên cách gọi `Send()` của ứng dụng.
- Gom phần chuyển đổi lời gọi hoặc dữ liệu vào một lớp.

### Nhược điểm

- Có thêm một lớp trung gian cần duy trì. Khi thư viện đổi API, có thể phải cập nhật Adapter.

### Ứng dụng thực tế

- Kết nối thư viện gửi SMS cũ với hệ thống thông báo hiện tại.
- Kết nối cổng thanh toán của đối tác có `MakePayment()` với ứng dụng đang gọi `Pay()`.

## Facade

### Mục đích sử dụng

Dùng Facade để cung cấp một cách gọi đơn giản cho thao tác cần phối hợp nhiều lớp. Trong demo, bên gọi dùng `CheckoutService.Instance.PlaceOrder()` để kiểm tra tồn kho, thanh toán và gửi xác nhận.

### Đặc trưng khi triển khai

1. Tách từng việc ra riêng. `InventoryService` kiểm tra tồn kho, `IPaymentStrategy` cung cấp cách thanh toán, `ConfirmationService` gửi xác nhận.
2. [`CheckoutService`](DesignPatterns.Structural/Facade/CheckoutService.cs) điều phối các bước trong `PlaceOrder()`. `Instance` là phần Singleton dùng để chia sẻ cùng một `CheckoutService`.

### Luồng chạy

1. `Program.cs` gọi `PlaceOrder()` với email, số lượng, tổng tiền và strategy thanh toán.
2. Facade kiểm tra tồn kho. Nếu thiếu hàng, trả về `false` và dừng trước bước thanh toán.
3. Nếu đủ hàng, Facade gọi thanh toán. Khi thanh toán thành công, Facade gửi xác nhận và trả về `true`.

Trong demo, `CheckoutService` khởi tạo kho với `5` món. Đơn `2` món đi tiếp đến thanh toán, còn đơn `6` món dừng ở bước kiểm tra kho.

### Ưu điểm

- Nơi gọi chỉ cần biết `PlaceOrder()` và các tham số cần truyền.
- Thứ tự xử lý nằm ở một chỗ, nên nhiều màn hình có thể dùng chung quy trình đặt hàng.

### Nhược điểm

- Facade dễ phình to nếu gom nhiều quy trình khác nhau vào cùng một lớp.

### Ứng dụng thực tế

- Đặt hàng trong ứng dụng bán hàng.
- Đăng ký tài khoản, tạo hồ sơ và gửi email chào mừng.

## Strategy

### Mục đích sử dụng

Dùng Strategy khi một việc có nhiều cách thực hiện và cần chọn cách phù hợp lúc chạy. Ví dụ trong bài Viblo là xuất cùng một thiết kế sang PNG, JPG hoặc PDF. Trong demo này, một đơn hàng có thể thanh toán bằng thẻ hoặc chuyển khoản.

### Đặc trưng khi triển khai

1. **Strategy** là `IPaymentStrategy`, quy định thao tác chung `bool Pay(decimal amount)`.
2. **Concrete Strategy** là `CardPayment` và `BankTransferPayment`. Mỗi lớp triển khai `Pay()` theo cách riêng.
3. **Context** là `CheckoutService`. Trong `PlaceOrder()`, nó gọi `payment.Pay(total)` qua interface.
4. **Client** là nơi chọn strategy. Demo chọn trong `Program.cs`, còn ứng dụng thực tế có thể chọn theo phương thức người dùng chọn trên màn hình thanh toán.

### Luồng chạy

1. Bên gọi truyền strategy vào `PlaceOrder()` cho từng đơn.
2. Facade kiểm tra kho rồi gọi `Pay()`. Nếu thiếu hàng hoặc `Pay()` trả về `false`, luồng dừng trước bước gửi xác nhận.
3. Nếu `Pay()` trả về `true`, Facade gửi xác nhận. Hai strategy trong demo chỉ in ra console và mô phỏng thanh toán thành công.

### Strategy thể hiện ở đâu?

`Program.cs` gọi `PlaceOrder()` hai lần trên cùng `CheckoutService`. Lần đầu truyền `CardPayment` nên console in `[Thẻ]`. Lần sau truyền `BankTransferPayment` nên console in `[Chuyển khoản]`. Cả hai đều đi qua cùng một dòng `payment.Pay(total)`.

Trong demo Factory Method, lớp con ghi đè `CreateNotification()` để quyết định **tạo đối tượng nào**. Trong demo Strategy, `Program.cs` truyền sẵn đối tượng để quyết định **thanh toán theo cách nào**.

### Ứng dụng thực tế và kết hợp pattern

- **Thanh toán:** UI hoặc API chọn strategy theo phương thức thanh toán của đơn. Facade giữ luồng kiểm tra kho → thanh toán → xác nhận. Vì Facade hiện là Singleton, truyền strategy vào từng lần gọi để các đơn có thể dùng cách thanh toán khác nhau.
- **Xuất báo cáo:** Chọn `CsvExportStrategy` hoặc `PdfExportStrategy` theo định dạng người dùng yêu cầu. Phần lấy dữ liệu báo cáo giữ nguyên.
- **Kết hợp Adapter:** Nếu cổng thanh toán bên thứ ba có API riêng, strategy có thể gọi qua một Adapter để chuyển từ lời gọi của ứng dụng sang lời gọi của SDK.
