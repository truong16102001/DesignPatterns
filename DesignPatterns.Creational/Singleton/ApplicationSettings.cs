namespace DesignPatterns.Creational.Singleton
{
    public sealed class ApplicationSettings
    {
        private static readonly object _lock = new object();
        private static ApplicationSettings? _instance;

        private ApplicationSettings() { }

        public static ApplicationSettings Instance
        {
            get
            {
                // Giữ bước kiểm tra và khởi tạo trong cùng một khóa để tránh tạo hai instance.
                lock (_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new ApplicationSettings();
                    }
                    return _instance;
                }

            }
        }

        public string ApplicationName { get; } = "Singleton Pattern Demo";
    }
}
