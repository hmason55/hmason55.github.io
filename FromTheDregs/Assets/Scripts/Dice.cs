using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice {

	public static int Roll(int dice, int sides, int multiplier = 1, int add = 0) {
		int product = 0;
		for(int d = 0; d < dice; d++) {
			product += Random.Range(1, sides+1);
		}
		return product*multiplier + add;
	}
}
