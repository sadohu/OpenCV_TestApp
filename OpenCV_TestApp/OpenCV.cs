using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_TestApp
{
	class OpenCV
	{

		public void VideoFramesConverter()
		{
			// Ruta del video
			// Console.WriteLine("Ingrese la ruta del video:");
			// string videoPath = Console.ReadLine();
			string videoPath = "E:/Files/workspace/Digital Solutions 324/sources/video7.mkv";

			if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
			{
				Console.WriteLine("Ruta de video no válida.");
				return;
			}
			// Ruta del proyecto
			string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string mainDirectory = Path.Combine(projectDirectory, "..", "..", "..");

			// Carpeta para almacenar los frames
			//string framesFolder = "E:/Files/workspace/Digital Solutions 324/frames/";
			string framesFolder = Path.Combine(mainDirectory, "frames\\");
			Directory.CreateDirectory(framesFolder);

			// Carpeta para almacenar los rostros
			// string facesFolder = "E:/Files/workspace/Digital Solutions 324/faces/";
			string facesFolder = Path.Combine(mainDirectory, "faces\\");
			Directory.CreateDirectory(facesFolder);

			// Ruta del clasificador Haar para rostros
			string faceCascadePath = Path.Combine(mainDirectory, "resources\\haarcascade_frontalface_default.xml");

			if (!File.Exists(faceCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			string eyeCascadePath = "E:/Files/workspace/Digital Solutions 324/project/OpenCV_TestApp/OpenCV_TestApp/resources/haarcascade_eye.xml";
			if (!File.Exists(eyeCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			string profileFaceCascadePath = "E:/Files/workspace/Digital Solutions 324/project/OpenCV_TestApp/OpenCV_TestApp/resources/haarcascade_profileface.xml";
			if (!File.Exists(profileFaceCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar para rostros de perfil no existe en la ruta especificada.");
				return;
			}

			// Inicializar capturador de video
			using (var videoCapture = new VideoCapture(videoPath))
			using (var faceCascade = new CascadeClassifier(faceCascadePath))
			using (var eyeCascade = new CascadeClassifier(eyeCascadePath))
			{
				if (!videoCapture.IsOpened())
				{
					Console.WriteLine("No se pudo abrir el video.");
					return;
				}

				int frameCount = (int)videoCapture.Get(VideoCaptureProperties.FrameCount);

				// Procesar cada frame
				for (int i = 0; i < frameCount; i++)
				{
					Mat frame = new Mat();
					videoCapture.Read(frame);

					if (frame.Empty())
						break;

					// Guardar cada frame como una imagen
					string frameFilePath = Path.Combine(framesFolder, $"frame_{i}.jpg");
					Cv2.ImWrite(frameFilePath, frame);

					// Detectar rostros en el frame y guardarlos
					DetectAndSaveFaces(faceCascade, eyeCascade, frame, facesFolder, i);
				}

				Console.WriteLine("Se guardaron los frames y se detectaron los rostros.\n");
			}
		}

		private static void DetectAndSaveFaces(CascadeClassifier faceCascade, CascadeClassifier eyeCascade, Mat frame, string outputFolder, int frameIndex)
		{

			// Convertir a escala de grises para el detector Haar
			Mat grayFrame = new Mat();
			Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

			// Detectar rostros en el frame
			Rect[] faces = faceCascade.DetectMultiScale(
				grayFrame,
				scaleFactor: 1.1,
				minNeighbors: 6,
				flags: HaarDetectionTypes.ScaleImage,
				minSize: new Size(30, 30),
				maxSize: new Size(300, 300)
			);

			// Guardar cada rostro como una imagen
			foreach (var face in faces)
			{
				Mat faceImage = new Mat(grayFrame, face);

				// Detectar ojos en el rostro
				Rect[] eyes = eyeCascade.DetectMultiScale(
					faceImage,
					scaleFactor: 1.1,
					minNeighbors: 6,
					flags: HaarDetectionTypes.ScaleImage,
					minSize: new Size(20, 20),
					maxSize: new Size(30, 30)
				);

				// Validar que se hayan detectado ojos
				if (eyes.Length > 0)
				{
					// Guardar el rostro solo si se detectaron ojos
					string faceFilePath = Path.Combine(outputFolder, $"face_{frameIndex}_{faces.Length}.jpg");
					Cv2.ImWrite(faceFilePath, faceImage);
				}
			}
		}

		public static void VideoFramesConverter2()
		{
			// Ruta del video
			// Console.WriteLine("Ingrese la ruta del video:");
			// string videoPath = Console.ReadLine();
			string videoPath = "E:/Files/workspace/Digital Solutions 324/sources/video8.mkv";

			if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
			{
				Console.WriteLine("Ruta de video no válida.");
				return;
			}

			// Carpeta para almacenar los frames
			string framesFolder = "E:/Files/workspace/Digital Solutions 324/frames2/";
			Directory.CreateDirectory(framesFolder); // Crear la carpeta si no existe

			// Carpeta para almacenar los rostros
			string facesFolder = "E:/Files/workspace/Digital Solutions 324/faces2/";
			Directory.CreateDirectory(facesFolder); // Crear la carpeta si no existe

			// Ruta del clasificador Haar para rostros
			string faceCascadePath = "E:/Files/workspace/Digital Solutions 324/project/OpenCV_TestApp/OpenCV_TestApp/resources/haarcascade_frontalface_default.xml";
			if (!File.Exists(faceCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			string eyeCascadePath = "E:/Files/workspace/Digital Solutions 324/project/OpenCV_TestApp/OpenCV_TestApp/resources/haarcascade_eye.xml";
			if (!File.Exists(eyeCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			string profileFaceCascadePath = "E:/Files/workspace/Digital Solutions 324/project/OpenCV_TestApp/OpenCV_TestApp/resources/haarcascade_profileface.xml";
			if (!File.Exists(profileFaceCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar para rostros de perfil no existe en la ruta especificada.");
				return;
			}

			// Inicializar capturador de video
			using (var videoCapture = new VideoCapture(videoPath))
			using (var faceCascade = new CascadeClassifier(faceCascadePath))
			using (var eyeCascade = new CascadeClassifier(eyeCascadePath))
			{
				if (!videoCapture.IsOpened())
				{
					Console.WriteLine("No se pudo abrir el video.");
					return;
				}

				int frameCount = (int)videoCapture.Get(VideoCaptureProperties.FrameCount);

				// Procesar cada frame
				for (int i = 0; i < frameCount; i++)
				{
					Mat frame = new Mat();
					videoCapture.Read(frame);

					if (frame.Empty())
						break;

					// Guardar cada frame como una imagen
					string frameFilePath = Path.Combine(framesFolder, $"frame_{i}.jpg");
					Cv2.ImWrite(frameFilePath, frame);

					// Detectar rostros en el frame y guardarlos
					DetectAndSaveFaces(faceCascade, eyeCascade, frame, facesFolder, i);
				}

				Console.WriteLine("Se guardaron los frames y se detectaron los rostros.\n");
			}

		}

		public void CompareFacesAndSaveDifferences()
		{
			// Obtener la lista de archivos de rostros en ambas carpetas
			string[] faceFilesVideo1 = Directory.GetFiles("E:/Files/workspace/Digital Solutions 324/faces/");
			string[] faceFilesVideo2 = Directory.GetFiles("E:/Files/workspace/Digital Solutions 324/faces2/");

			string outputFolder = "E:/Files/workspace/Digital Solutions 324/differences/";
			Directory.CreateDirectory(outputFolder); // Crear la carpeta si no existe

			// Comparar cada par de rostros
			for (int i = 0; i < Math.Min(faceFilesVideo1.Length, faceFilesVideo2.Length); i++)
			{
				// Leer las imágenes de los rostros
				Mat faceImageVideo1 = Cv2.ImRead(faceFilesVideo1[i]);
				Mat faceImageVideo2 = Cv2.ImRead(faceFilesVideo2[i]);

				if (faceImageVideo1.Empty() || faceImageVideo2.Empty())
				{
					Console.WriteLine($"Error al leer las imágenes de los rostros en el índice {i}.");
					continue;
				}

				// Asegurarse de que ambas imágenes tengan las mismas dimensiones
				if (faceImageVideo1.Size() != faceImageVideo2.Size())
				{
					// Ajustar el tamaño de una de las imágenes
					Cv2.Resize(faceImageVideo1, faceImageVideo1, faceImageVideo2.Size());
				}

				// Realizar la comparación de píxel por píxel
				Mat difference = new Mat();
				Cv2.Absdiff(faceImageVideo1, faceImageVideo2, difference);

				// Threshold para resaltar las diferencias
				Cv2.Threshold(difference, difference, 30, 255, ThresholdTypes.Binary);
				// Cv2.Threshold(faceImageVideo2, difference, 30, 255, ThresholdTypes.Tozero);

				// Convertir a escala de grises si no lo está
				if (difference.Channels() > 1)
				{
					Cv2.CvtColor(difference, difference, ColorConversionCodes.BGR2GRAY);
				}

				// Inicializar contours antes de la llamada a Cv2.FindContours
				Point[][] contours;
				HierarchyIndex[] hierarchyIndexes;

				// Encontrar contornos en las diferencias
				Cv2.FindContours(difference, out contours, out hierarchyIndexes, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

				// Dibujar contornos alrededor de las diferencias
				Cv2.DrawContours(faceImageVideo2, contours, -1, Scalar.Red, 2);


				// Guardar las diferencias resaltadas en una carpeta
				string outputFilePath = Path.Combine(outputFolder, $"difference_{i}.jpg");
				difference.SaveImage(outputFilePath);

			}
		}
	}
}
