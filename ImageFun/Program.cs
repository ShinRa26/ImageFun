using System;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var s = new Steg("test.jpg", "reviews.pdf");
			//s.HideFile();
			s.ExtractFile();
		}
	}
}

