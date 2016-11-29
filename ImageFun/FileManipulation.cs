using System;
using System.IO;

namespace ImageFun
{
	public class FileManipulation
	{
		public FileManipulation (){}

		public byte[] ReadFileBytes(string path)
		{
			byte[] data = File.ReadAllBytes(path);
			return data;
		}

		public int GetLSB(byte b)
		{
			return b&0x01;
		}

		public int FlipLSB(byte b)
		{
			int bit = (b & 0x01);

			return (b ^ bit);
		}
	}
}

