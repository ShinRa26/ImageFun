using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

namespace ImageFun
{
	public class Steg
	{
		private FileManipulation f;
		private Helper h;

		private const string altImg = "altered.bmp"; 
		private const string extFile = "extracted.txt";

        private const int byteSize = 8;
		private const int maxByteSize = 254;
		private const int sizeCompensation = 1024;

		private int sizeBytes;

		private byte[] imgBytes, fileBytes, extBytes, altBytes;
        private int imgByteLength, fileByteLength;

		//Ctor
		public Steg (string imgPath, string filePath)
		{
			f = new FileManipulation();
			h = new Helper();

			imgBytes = f.ReadFileBytes(imgPath);
			fileBytes = f.ReadFileBytes(filePath);

			imgByteLength = imgBytes.Length;
			fileByteLength = fileBytes.Length;
		}

		/*
		 * SECTION: HIDES THE FILE IN THE IMAGE 
		 * 
		 */
		public void HideFile()
		{

			if(fileByteLength < imgByteLength)
				ConvertAndSave();
			else
				Console.WriteLine ("File is too large.");
			
		}

		private void ConvertAndSave()
		{
			Console.WriteLine ("Hiding File...");
			HideFileSize();

			var fileBits = new BitArray(fileBytes);

			for(int i = sizeBytes; i < imgByteLength; i++)
			{
				if(i < fileBits.Length)
				{
					int imgLsb = f.GetLSB(imgBytes[i]);

					int fileBit = Convert.ToInt16(fileBits[i]);

					if(imgLsb != fileBit)
						f.FlipLSB(imgBytes[i]);
				}
				else
					break;
			}

			h.SaveImage(altImg, imgBytes);
		}

		private void HideFileSize()
		{
			int quotient = fileByteLength / maxByteSize;
			int remainder = fileByteLength % maxByteSize;
			this.sizeBytes = sizeCompensation + quotient + 1;

			for(int i = sizeCompensation; i <= sizeBytes; i++)
			{
                if (i == sizeBytes)
                    imgBytes[i] = (byte)remainder;
                else
                    imgBytes[i] = maxByteSize;
			}
		}
		/*
		 * SECTION: END
		 */



        /*
         * SECTION: EXTRACTION
         */
		private Dictionary<int,int> ExtractFileSize()
		{
			int fileSize = 0;
			int counter = 0;
			this.altBytes = f.ReadFileBytes(altImg);
			var infoDict = new Dictionary<int, int>();

			for(int i = sizeCompensation; i > -1; i++)
			{
                if (altBytes[i+1] != altBytes[i])
                {
                    fileSize += altBytes[i+1];
					counter++;
                    break;
                }
                else
                    fileSize += altBytes[i];

				counter++;
			}

			infoDict[counter] = fileSize;
			return infoDict;
		}

		public void ExtractFile()
		{
			Console.WriteLine ("Extracting file...");

			var info = ExtractFileSize();
			int fileSize = 0;
			int start = 0;

			foreach(KeyValuePair<int, int> kvp in info)
			{
				fileSize = kvp.Value;
				start = sizeCompensation + kvp.Key;
			}

			var tempBits = new BitArray(fileSize*8);

			for(int i = start + 1; i < fileSize * 8; i++)
			{
				int bit = f.GetLSB(altBytes[i]);
				bool bitVal = h.ConvertBitToBool(bit);
				tempBits.Set(i, bitVal);
			}

			this.extBytes = h.ConvertToByteArray(tempBits);

			h.SaveFile(extFile,extBytes);
		}
			
        /*
         * SECTION: END
         */
	}
}

