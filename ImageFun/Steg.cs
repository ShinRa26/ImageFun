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
		private string filename = "extracted.";

        private Bitmap img;
        public byte[] imgBytes;
        public byte[] fileBytes;

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
			imgBytes = h.GetImageBytes(img, header);
            fileBytes = f.ReadFileBytes(filePath);
		}

		public Steg(string imgPath)
		{
			f = new FileManipulation();
			h = new Helper();

			this.imgPath = imgPath;

			img = new Bitmap(imgPath);
			imgBytes = h.GetImageBytes(img, header);
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
					imgBytes[i] = 32;

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
			byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);
			Console.WriteLine ("FileSize Bytes: " + fileSizeBytes.Length);

			for(int i = start; i < start + sizeBytes; i++)
			{
				imgBytes[i] = fileSizeBytes[count];
				count++;
			}
			
        }

        public void ConvertAndSave()
		{
			HideFileExt();
			HideFileSize();

			int start = header + extensionBytes + sizeBytes;

			for(int i = header; i < start; i++)
				Console.WriteLine (imgBytes[i]);

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
		public string ExtractFileExt()
		{
			string ext;
			var extChars = new char[extensionBytes];
			var eBytes = new byte[extensionBytes];
			int count = 0;

			for(int i = header; i < header + extensionBytes; i++)
			{
				if(imgBytes[i] == 0)
				{
					eBytes[count] = 32;
					extChars[count] = (char)eBytes[count];
					count++;
				}
				else
				{
					eBytes[count] = imgBytes[i];
					extChars[count] = (char)eBytes[count];
					count++;
				}
			}

			ext = new string(extChars);
			return ext;
		}


		private int ExtractFileSize()
		{
			int fileSize = 0;
			byte[] size = new byte[sizeBytes];
			int start = header + extensionBytes;
			int count = 0;

			for(int i = start; i < start + sizeBytes; i++)
			{
				size[count] = imgBytes[i];
				count++;
			}

			fileSize = BitConverter.ToInt32(size,0);
			return fileSize;
		}



		public void ExtractFile()
		{
			string ext = ExtractFileExt();
			string[] split = ext.Split(' ');
			ext = split[0];

			int filesize = ExtractFileSize();
			fileBytes = new byte[filesize];
			int start = header + extensionBytes + sizeBytes;
			int count = 0;

			for(int i = start; i < imgBytes.Length; i++)
			{
				if(i >= filesize)
					break;
				else
				{
					fileBytes[count] = imgBytes[i];
					count++;
				}
			}

			filename += ext;
			h.SaveFile(filename,fileBytes);
		}
			
		/*
		 * SECTION: END
		 */

		public void ImgData(byte[] t)
		{
			for(int i = 0; i < header; i++)
				Console.WriteLine (t[i]);
		}
	}
}

