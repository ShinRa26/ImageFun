using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel;

namespace ImageFun
{
	public class Helper
	{
		public Helper (){}

        //Get Color array from Bitmap
        public Color[] GetColors(Bitmap m)
        {
            var colors = new Color[m.Height * m.Width];
            int count = 0;

            for(int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    colors[count] = m.GetPixel(i, j);
                    count++;
                }
            }

            return colors;
        }

		public byte[] GetImagePixelBytes(Bitmap img)
        {
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			System.Drawing.Imaging.BitmapData bmpData = img.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, img.PixelFormat); 
			// Get the address of the first line.

			IntPtr ptr = bmpData.Scan0; 
			// Declare an array to hold the bytes of the bitmap.  
			int bytes = Math.Abs(bmpData.Stride) * img.Height;

			byte[] rgbValues = new byte[bytes];
			// Copy the RGB values into the array.
			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
			img.UnlockBits(bmpData);

			/*
            var colors = GetColors(img);
            byte[] rgb = new byte[colors.Length * 3];
            int count = 0;

            for(int i = 0; i < colors.Length; i++)
            {
                byte r = colors[i].R; byte g = colors[i].G; byte b = colors[i].B;

                rgb[count] = r; rgb[count + 1] = g; rgb[count + 2] = b;
                count += 3;
            }

            return rgb;
            */
			return rgbValues;
        }

        public string GetFileExt(string path)
        {
            string text = path;
            string[] split = path.Split('.');
            return split[1];
        }

		public byte[] GetImageHeader(Image img, int headerSize)
		{
			var bmp = ImageToByte(img);
			var header = new byte[headerSize];

			for(int i = 0; i < headerSize; i++)
				header[i] = bmp[i];

			return header;
		}

		public byte[] GetImageBytes(Bitmap img, int header)
		{
			var rgb = GetImagePixelBytes(img);
			var headerBytes = GetImageHeader(img, header);
			var imgBytes = new byte[rgb.Length + headerBytes.Length];

			int count = 0;

			for(int i = 0; i < imgBytes.Length; i++)
			{
				if(i < headerBytes.Length)
					imgBytes[i] = headerBytes[i];
				else
				{
					imgBytes[i] = rgb[count];
					count++;
				}
				
			}

			return imgBytes;
		}















		public byte[] ConvertToByteArray(BitArray b)
		{
			int byteSize = 8;
			byte[] bytes = new byte[b.Length / 8];
			bool[] temp = new bool[byteSize];
			int counter = 0;

			for(int i = 0; i < bytes.Length; i++)
			{
				byte val = 0;

				for(int j = 0; j < byteSize; j++)
				{
					temp[j] = b.Get(counter);
					counter++;
				}

				val = ConvertBoolArrayToByte(temp);
				bytes[i] = val;
			}

			return bytes;
		}

		//Terrible hack
		public byte ConvertBoolArrayToByte(bool[] b)
		{
			var binary = BinaryAssignment();
			int value = 0;

			for(int i = 0; i < b.Length; i++)
			{
				int bit = ConvertBoolToBit(b[i]);

				if(bit == 1)
					value += binary[i];
			}

			return (byte)value;
		}

		public bool ConvertBitToBool(int bit)
		{
			if (bit == 0)
				return false;
			else if (bit == 1)
				return true;
			else
				return false;
		}

		private int ConvertBoolToBit(bool b)
		{
			if(b)
				return 1;
			else
				return 0;
		}

		//Terrible Hack
		private Dictionary<int, int> BinaryAssignment()
		{
			var b = new Dictionary<int, int>();

			b[0] = 1;
			b[1] = 2;
			b[2] = 4;
			b[3] = 8;
			b[4] = 16;
			b[5] = 32;
			b[6] = 64;
			b[7] = 128;

			return b;
		}
			
















		public void SaveImageBmp(string n, byte[] b)
		{
			using(var ms = new MemoryStream(b))
			{
				var img = Bitmap.FromStream(ms);
				img.RotateFlip(RotateFlipType.Rotate180FlipX);
				img.Save(n, ImageFormat.Bmp);
			}
		}

		public void SaveFile(string n, byte[] b)
		{
			var fs = new BinaryWriter(new FileStream(n, FileMode.Append, FileAccess.Write));
			fs.Write(b);
			fs.Close();
		}

		/*
		 * TEMP SHIT
		 */
		public byte[] ImageToByte(Image img)
		{
			ImageConverter converter = new ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
	}
}

