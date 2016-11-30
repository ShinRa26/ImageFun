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
        private const int extensionBytes = 8;

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
            imgBytes = h.GetImagePixelBytes(img);
            fileBytes = f.ReadFileBytes(filePath);
		}

		/*
		 * SECTION: HIDES THE FILE IN THE IMAGE 
		 * 
		 */
		public void HideFileExt()
		{
            string ext = h.GetFileExt(filePath);
            var extBytes = Encoding.ASCII.GetBytes(ext);
            int count = 0;
            
            for(int i = header; i < header + extensionBytes; i++)
            {
                imgBytes[i] = extBytes[count];
                count++;
            }	
		}

        private void HideFileSize()
        {
            int fileSize = fileBytes.Length;
            int start = header + extensionBytes;

            for(int i = start; i < start + sizeBytes; i++)
            {
                //TODO PICK UP HERE
            }
        }

        private void ConvertAndSave()
		{
			//PH
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
	}
}

