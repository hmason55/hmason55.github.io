﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swatch {

	public static int size = 10;

	// Grayscale
	public static string[] template = new string[] {
		"#FFFFFFFF",
		"#E6E6E6FF",
		"#CCCCCCFF",
		"#B3B3B3FF",
		"#999999FF",
		"#808080FF",
		"#666666FF",
		"#4D4D4DFF",
		"#333333FF",
		"#1A1A1AFF"
	};

	#region Skin
	public static string[] skinHumanLight = new string[] {
		"#FFFFFFFF",
		"#FFFFD9FF",
		"#F5F5BAFF",
		"#DFDF92FF",
		"#B3A36BFF",
		"#807655FF"
	};

	public static string[] skinHumanMedium = new string[] {
		"#FFFFFFFF",
		"#DDB07DFF",
		"#DDB07DFF",
		"#C19E72FF",
		"#938252FF",
		"#6A6346FF"
	};
	
	public static string[] skinHumanDark = new string[] {
		"#FFFFFFFF",
		"#D7B38BFF",
		"#BFA07CFF",
		"#A38B6CFF",
		"#706443FF",
		"#46412DFF"
	};
	#endregion

	#region Hair
	public static string[] hairBrown = new string[] {
		"#9E8D54FF",
		"#807655FF",
		"#635C42FF"
	};

	public static string[] hairWhite = new string[] {
		"#FFFFFFFF",
		"#E3F6FBFF",
		"#CCDDE1FF"
	};

	public static string[] hairRed = new string[] {
		"#E52E2EFF",
		"#C12626FF",
		"#AA2323FF"
	};
	#endregion

	public static Color SwapToColor(Color templateColor, Color[] swatch) {
		//Debug.Log(ColorUtility.ToHtmlStringRGB(templateColor));
		switch(ColorUtility.ToHtmlStringRGB(templateColor)) {
			case "FFFFFF":
				return swatch[0];

			case "E6E6E6":
				return swatch[1];

			case "CCCCCC":
				return swatch[2];

			case "B3B3B3":
				return swatch[3];

			case "999999":
				return swatch[4];

			case "808080":
				return swatch[5];

			case "666666":
				return swatch[6];

			case "4D4D4D":		
				return swatch[7];

			case "333333":
				return swatch[8];

			case "1A1A1A":
				return swatch[9];

			default:
				return swatch[9];
		}
	}

}
