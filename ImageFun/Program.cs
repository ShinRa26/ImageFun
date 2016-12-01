using System;
using System.IO;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{

            //var s = new Steg("test.bmp", "reviews.txt");
            //s.ConvertAndSave();

			var st = new Steg("altered.bmp");
			st.ExtractFile();

            /*
            var before = s.fileBytes;
            var after = st.fileBytes;
            
            for(int i = 0; i < before.Length; i++)
            {
                if(before[i] != after[i])
                    Console.WriteLine(string.Format("\nException found at: {0}\nBefore Value: {1}\tAfter Value: {2}", i, before[i], after[i]));
            }
            */
		}
	}
}

