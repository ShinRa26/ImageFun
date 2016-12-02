using System;
using System.IO;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//var s = new Steg("test.bmp", "paper.pdf");
            //s.Hide();

			var st = new Steg("altered.bmp");
			st.ExtractFile();
		}
	}
}

