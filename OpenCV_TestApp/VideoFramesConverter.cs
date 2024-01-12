using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_TestApp
{
	internal class VideoFramesConverter
	{
		public void converter(String route)
		{
			Console.WriteLine("Ingrese la ruta del video:");
			string route = Console.ReadLine();


			using (var videoCapture = new VideoCapture(route))
			{
				if (!videoCapture.IsOpened())
				{
					Console.WriteLine("No se pudo abrir el video.");
					return;
				}

				int frameCount = (int)videoCapture.Get(VideoCaptureProperties.FrameCount);

				for (int i = 0; i < frameCount; i++)
				{
					Mat frame = new Mat();
					videoCapture.Read(frame);

					if (frame.Empty())
						break;

					// Guardar cada frame como una imagen
					Cv2.ImWrite($"E:/Files/workspace/Digital Solutions 324/frames/frame_{i}.jpg", frame);
				}
			}
		}
	}
}
