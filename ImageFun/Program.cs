using System;
using System.IO;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//var s = new Steg("test.bmp", "reviews.pdf");
           	//s.ConvertAndSave();

			var st = new Steg("altered.bmp");
			st.ExtractFile();
		}
	}
}

