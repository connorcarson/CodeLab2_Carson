using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomGridScript : GridScript
{

	public static readonly float rockPercentage = 0.2f; 		// 20% chance of rocks
	public static readonly float forestPercentage = 0.05f;		// 5% chance of forest
	public static readonly float waterPercentage = 0.1f;		// 10% chance of water

	string[] gridString;

	private void Awake()
	{
		
		// Make the grid be generated into gridString at random w/ the above percentages.
	}

	protected override Material GetMaterial(int x, int y){

		var c = gridString[y].ToCharArray()[x];

		Material mat;

		switch(c){
			case 'd': 
				mat = mats[1];
				break;
			case 'w': 
				mat = mats[2];
				break;
			case 'r': 
				mat = mats[3];
				break;
			default: 
				mat = mats[0];
				break;
		}
	
		return mat;
	}
}
