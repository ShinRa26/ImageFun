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

			var fileBits = new BitArray(fileBytes);
			int count = 0;

			if(fileBits.Length != fileBytes.Length * 8)
			{
				Console.WriteLine (string.Format("Error in bit array length. Bit Array length: {0}\tByte Array Length * 8: {1}", fileBits.Length, fileBytes.Length*8));
				return;
			}

			for(int i = start; i < imgBytes.Length; i++)
			{
                try
                {
					int fileBit = h.ConvertBoolToBit(fileBits.Get(count));
                    int imgLSB = f.GetLSB(imgBytes[i]);

                    if (fileBit != imgLSB)
						imgBytes[i] = f.FlipLSB(fileBit, imgBytes[i]);

					count++;
                }
                catch(Exception) { break; }
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
            var bits = new BitArray(filesize * 8);

			int start = header + extensionBytes + sizeBytes;
			int count = 0;

			for(int i = start; i < bits.Length; i++)
			{
                int bit = f.GetLSB(imgBytes[i]);
                bool bitVal = h.ConvertBitToBool(bit);
                bits.Set(count, bitVal);
				count++;
			}

            filename += ext;
			fileBytes = h.ConvertToByteArray(bits);

			Console.WriteLine (fileBytes[0]);
			h.SaveFile(filename,fileBytes);
		}
			
		/*
		 * SECTION: END
		 */
	}
}

