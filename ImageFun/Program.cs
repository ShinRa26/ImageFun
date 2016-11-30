using System;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var s = new Steg("test.bmp", "review.txt");
			//s.HideFile();
			s.ExtractFile();
		}
	}
}

