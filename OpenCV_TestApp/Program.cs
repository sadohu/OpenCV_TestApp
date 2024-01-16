// See https://aka.ms/new-console-template for more information
using OpenCV_TestApp;

Console.WriteLine("Bienvenido a Digital Solutions 324 OpenCV test.");

OpenCV openCV = new OpenCV();
//openCV.VideoFramesConverter();

//Console.WriteLine("Ingrese la ruta de la imagen para hallar diferencias:");
//openCV.Differences();

openCV.VideoFramesConverter();
//openCV.VideoFramesConverter2();

//openCV.CompareFacesAndSaveDifferences();


