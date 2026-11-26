using DesignPatterns.Creational.Singleton;

namespace DesignPatterns.Tests
{
    public class SingletonTests
    {
        [Fact]
        public void Instance_ReturnsSameObject_WhenAccessedTwice()
        {
            var first = ApplicationSettings.Instance;
            var second = ApplicationSettings.Instance;

            Assert.Same(first, second);
        }
    }
}