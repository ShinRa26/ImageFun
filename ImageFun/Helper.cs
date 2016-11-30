using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

namespace ImageFun
{
	public class Helper
	{
		public Helper (){}

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

		public void SaveImage(string n, byte[] b)
		{
			var fs = new BinaryWriter(new FileStream(n, FileMode.Append, FileAccess.Write));
			fs.Write(b);
			fs.Close();
		}

		public void SaveFile(string n, byte[] b)
		{
			var fs = new BinaryWriter(new FileStream(n, FileMode.Append, FileAccess.Write));
			fs.Write(b);
			fs.Close();
		}
	}
}

