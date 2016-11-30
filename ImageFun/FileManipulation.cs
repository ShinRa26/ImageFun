using System;
using System.IO;
using System.Drawing.Imaging;

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

        public byte[] ReadImageBytes(string path)
        {
            var img = System.Drawing.Image.FromFile(path);

            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
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

