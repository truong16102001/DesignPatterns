namespace DesignPatterns.Structural.Adapter
{
    // Mô phỏng thư viện SMS cũ chỉ cung cấp SendText().
    public class LegacySmsSender
    {
        public void SendText(string message)
        {
            Console.WriteLine($"[Legacy SMS] {message}");
        }
    }
}
