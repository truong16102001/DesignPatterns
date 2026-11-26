using DesignPatterns.Creational.FactoryMethod;

namespace DesignPatterns.Structural.Adapter
{
    public class LegacySmsAdapter : INotification
    {
        private readonly LegacySmsSender _sender;

        public LegacySmsAdapter(LegacySmsSender sender)
        {
            _sender = sender;
        }

        public void Send(string message)
        {
            // Chuyển lời gọi Send() của ứng dụng sang SendText() để dùng lại thư viện SMS cũ.
            _sender.SendText(message);
        }
    }
}
