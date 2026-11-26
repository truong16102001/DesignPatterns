# DesignPatterns

## Singleton

Singleton cung cấp một đối tượng dùng chung cho các phần của ứng dụng. Ví dụ này dùng [`ApplicationSettings`](DesignPatterns.Creational/Singleton/ApplicationSettings.cs) để minh họa cách khởi tạo đối tượng khi cần và bảo đảm các luồng nhận cùng một instance.

### Đặc trưng khi triển khai

1. Đặt constructor là `private` để code bên ngoài lớp không thể gọi `new ApplicationSettings()`.
2. Khai báo field `private static ApplicationSettings? _instance` để lưu đối tượng dùng chung. Field bắt đầu là `null` và được gán khi `Instance` được đọc lần đầu.
3. Cung cấp property `public static ApplicationSettings Instance` để code bên ngoài lấy đối tượng đã lưu.
4. Tạo một đối tượng khóa dùng chung bằng `private static readonly object _lock = new object()`. Đặt cả bước kiểm tra `_instance == null` và bước tạo đối tượng bên trong `lock (_lock)` để hai luồng không thể cùng thấy `_instance` là `null` rồi cùng tạo đối tượng.

### Luồng chạy

1. Luồng A đọc `Instance`, lấy khóa `_lock`, thấy `_instance` là `null` và tạo `ApplicationSettings`.
2. Nếu luồng B đọc `Instance` trong lúc A đang giữ khóa, B chờ ở `lock (_lock)`.
3. A trả về đối tượng và tự nhả khóa khi rời khối `lock`. B lấy khóa, thấy `_instance` đã được gán và trả về cùng đối tượng.
