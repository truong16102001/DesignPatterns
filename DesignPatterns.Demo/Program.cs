using DesignPatterns.Creational.Singleton;

var first = ApplicationSettings.Instance;
var second = ApplicationSettings.Instance;

Console.WriteLine(object.ReferenceEquals(first, second)); // kì vọng là true