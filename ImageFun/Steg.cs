using System;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;

namespace ImageFun
{
	public class Steg
	{
		private FileManipulation f;

		private const string altImg = "altered.jpg"; 
		private const string extFile = "extracted.pdf";

        private const int byteSize = 8;
		private const int maxByteSize = 254;
		private const int sizeCompensation = 1024;

		private int sizeBytes;

		private byte[] imgBytes, fileBytes, extBytes, altBytes;
        private int imgByteLength, fileByteLength;

		public Steg (string imgPath, string filePath)
		{
			f = new FileManipulation();
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
				ImageLarger();
			else
				TooLarge();
			
		}

		private void ImageLarger()
		{
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
		    //SaveImage();
		}

		private void HideFileSize()
		{
			int quotient = fileByteLength / maxByteSize;
			int remainder = fileByteLength % maxByteSize;
			this.sizeBytes = sizeCompensation + quotient + 1;

            Console.WriteLine("Quotient: " + quotient);
            Console.WriteLine("Remainder: " + remainder);
            Console.WriteLine("Size Bytes: " + sizeBytes + "\nSize Compensation: " + (sizeCompensation + quotient));

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
		private int ExtractFileSize()
		{
			int fileSize = 0;
			this.altBytes = f.ReadFileBytes(altImg);

			for(int i = sizeCompensation; i > -1; i++)
			{
                if (altBytes[i+1] != altBytes[i])
                {
                    fileSize += altBytes[i+1];
                    break;
                }
                else
                    fileSize += altBytes[i];
			}

			Console.WriteLine ("Filesize: " + fileSize);
			return fileSize;
		}

		public void ExtractFile()
		{
			int fileSize = ExtractFileSize();

			int quotient = fileSize / maxByteSize;
			int start = sizeCompensation + quotient + 1;
			var tempBits = new BitArray(fileSize*8);

			for(int i = start; i < fileSize * 8; i++)
			{
				int bit = f.GetLSB(altBytes[i]);
				bool bitVal = ConvertBitToBool(bit);
				tempBits.Set(i, bitVal);
			}

			this.extBytes = ConvertToByteArray(tempBits);

			//SaveFile();
		}


        /*
         * SECTION: END
         */


		private byte[] ConvertToByteArray(BitArray b)
		{
			byte[] bytes = new byte[b.Length / 8];
			
            //TODO FIX THIS SHIT

			return bytes;
		}

		private byte ConvertBoolArrayToByte(bool[] source)
		{
			byte result = 0;
			// This assumes the array never contains more than 8 elements!
			int index = 8 - source.Length;

			// Loop through the array
			foreach (bool b in source)
			{
				// if the element is 'true' set the bit at that position
				if (b)
					result |= (byte)(1 << (7 - index));

				index++;
			}

			return result;
		}

		private bool ConvertBitToBool(int bit)
		{
            if (bit == 0)
                return false;
            else if (bit == 1)
                return true;
            else
                return false;
		}

		private void SaveImage()
		{
			var fs = new BinaryWriter(new FileStream(altImg, FileMode.Append, FileAccess.Write));
			fs.Write(this.imgBytes);
			fs.Close();
		}

		private void SaveFile()
		{
			var fs = new BinaryWriter(new FileStream(extFile, FileMode.Append, FileAccess.Write));
			fs.Write(extBytes);
			fs.Close();
		}

		private void TooLarge()
		{
			Console.WriteLine ("File is too large");
		}
	}
}

