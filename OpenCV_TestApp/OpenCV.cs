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

		public void VideoFramesConverter(string route, int videoNumber)
		{
			string videoPath = route;

			// Ruta del proyecto
			string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string mainDirectory = Path.Combine(projectDirectory, "..", "..", "..");

			// Carpeta para almacenar los frames
			string framesFolder = Path.Combine(mainDirectory, videoNumber == 1 ? "frames\\" : "frames2\\");
			Directory.CreateDirectory(framesFolder);

			// Carpeta para almacenar los rostros
			string facesFolder = Path.Combine(mainDirectory, videoNumber == 1 ? "faces\\" : "faces2\\");
			Directory.CreateDirectory(facesFolder);

			// Ruta del clasificador Haar para rostros
			string faceCascadePath = Path.Combine(mainDirectory, "resources\\haarcascade_frontalface_default.xml");

			if (!File.Exists(faceCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			string eyeCascadePath = Path.Combine(mainDirectory, "resources\\haarcascade_eye.xml");
			if (!File.Exists(eyeCascadePath))
			{
				Console.WriteLine("El archivo del clasificador Haar no existe en la ruta especificada.");
				return;
			}

			// Inicializar capturador de video
			using (var videoCapture = new VideoCapture(videoPath))
			using (var faceCascade = new CascadeClassifier(faceCascadePath))
			using (var eyeCascade = new CascadeClassifier(eyeCascadePath))
			{
				Console.WriteLine("Procesando video...");

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

		public void CompareFacesAndSaveDifferences()
		{
			// Ruta del proyecto
			string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string mainDirectory = Path.Combine(projectDirectory, "..", "..", "..");

			// Carpeta para almacenar los rostros
			string facesFolder = Path.Combine(mainDirectory, "faces");
			Directory.CreateDirectory(facesFolder);

			// Obtener la lista de archivos de rostros en ambas carpetas
			string[] faceFilesVideo1 = Directory.GetFiles(facesFolder);
			string[] faceFilesVideo2 = Directory.GetFiles(facesFolder + "2");

			string outputFolder = Path.Combine(mainDirectory, "differences");
			Directory.CreateDirectory(outputFolder);

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

			Console.WriteLine("Se guardaron las diferencias entre los rostros.");
			System.Diagnostics.Process.Start("explorer.exe", outputFolder);
		}
	}
}
