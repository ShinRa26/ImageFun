using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;

namespace ImageFun
{
	//Steg class
	public class Steg
	{
		private FileManipulation f; 
		private Helper h;

		private const int byteSize = 8;
        private const int header = 54;
        private const int sizeBytes = 4;
        private const int extensionBytes = 4;

        private Bitmap img;
        public byte[] imgBytes;
        private byte[] fileBytes;

        private string imgPath;
        private string filePath;


		//Ctor
		public Steg (string imgPath, string filePath)
		{
			f = new FileManipulation();
			h = new Helper();

            this.imgPath = imgPath;
            this.filePath = filePath;

            img = new Bitmap(imgPath);
			imgBytes = h.ImageToByte(img);
			
            //imgBytes = h.GetImagePixelBytes(img);

            fileBytes = f.ReadFileBytes(filePath);
		}

		/*
		 * SECTION: HIDES THE FILE IN THE IMAGE 
		 * 
		 */
		private void HideFileExt()
		{
            string ext = h.GetFileExt(filePath);
            var extBytes = Encoding.ASCII.GetBytes(ext);
            int count = 0;
            
            for(int i = header; i < header + extensionBytes; i++)
            {
				if(count >= extBytes.Length)
					imgBytes[i] = 0;

				else
				{
                	imgBytes[i] = extBytes[count];
                	count++;
				}
            }
		}

        private void HideFileSize()
        {
            int fileSize = fileBytes.Length;
			int start = header + extensionBytes;
			int count = 0;

			byte[] fileSizeBytes = {
				(byte)(fileSize >> 24),
				(byte)(fileSize >> 16),
				(byte)(fileSize >> 8),
				(byte)(fileSize)
			};

			for(int i = start; i < start + sizeBytes; i++)
				imgBytes[i] = fileSizeBytes[count];
        }

        public void ConvertAndSave()
		{
			HideFileExt();
			HideFileSize();

			int start = header + extensionBytes + sizeBytes;

			for(int i = start; i < imgBytes.Length; i++)
			{
				if(i >= fileBytes.Length)
					break;
				
				int fileLSB = f.GetLSB(fileBytes[i]);
				int imgLSB = f.GetLSB(imgBytes[i]);

				if(imgLSB != fileLSB)
					f.FlipLSB(imgBytes[i]);
			}

			h.SaveImageBmp("altered.bmp", imgBytes);
		}

		
		/*
		 * SECTION: END
		 */



		/*
		 * SECTION: EXTRACTION
		 */
		private void ExtractFileSize()
		{
			//PH
		}

		public void ExtractFile()
		{
			//PH
		}
			
		/*
		 * SECTION: END
		 */

		public void ImgData()
		{
			for(int i = 0; i < header + 12; i++)
				Console.Write(imgBytes[i] + ", ");
		}
	}
}

