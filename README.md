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
