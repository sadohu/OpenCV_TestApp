// See https://aka.ms/new-console-template for more information
using OpenCV_TestApp;

Console.WriteLine("Hello, World!");

VideoFramesConverter videoConverter = new VideoFramesConverter();
Console.WriteLine("Ingrese la ruta del video de referencia");
string route = Console.ReadLine();
videoConverter.Converter(route);


