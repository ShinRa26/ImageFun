﻿using System;

namespace ImageFun
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            //var s = new Steg("test.bmp", "reviews.pdf");
			//s.ConvertAndSave();

			var s = new Steg("altered.bmp");
			s.ExtractFile();
		}
	}
}

