using System;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

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
			return (b&0x1);
		}

		//Horrible Hack
		public byte FlipLSB(int bit, byte b)
		{
			if(bit == 0)
				b += 1;
			else if(bit == 1)
				b -= 1;
			
			return b;
		}
	}
}

 