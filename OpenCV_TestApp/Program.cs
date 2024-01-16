// See https://aka.ms/new-console-template for more information
using OpenCV_TestApp;

Console.WriteLine("Bienvenido a Digital Solutions 324 OpenCV test.\n");

OpenCV openCV = new OpenCV();


// Ruta del video 1
Console.WriteLine("Ingrese la ruta del video base:");
string videoPath = Console.ReadLine();

if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
{
	Console.WriteLine("Ruta de video no válida.");
	return;
}

openCV.VideoFramesConverter(videoPath, 1);

// Ruta del video 2
Console.WriteLine("Ingrese la ruta del video de la persona:");
string videoPath2 = Console.ReadLine();

if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
{
	Console.WriteLine("Ruta de video no válida.");
	return;
}

openCV.VideoFramesConverter(videoPath2, 2);

// Procesar diferencias entre frames
Console.WriteLine("Procesando differencias...");
openCV.CompareFacesAndSaveDifferences();


