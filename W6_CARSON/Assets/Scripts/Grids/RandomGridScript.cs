using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class RandomGridScript : GridScript
{

	public static readonly float rockPercentage = 0.2f; 		// 20% chance of rocks
	public static readonly float forestPercentage = 0.05f;		// 5% chance of forest
	public static readonly float waterPercentage = 0.1f;		// 10% chance of water

	private List<String> gridString = new List<string>();

	private void Awake()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			string row = "";
			for (int X = 0; X < gridWidth; X++)
			{
				if (UnityEngine.Random.value <= rockPercentage)
				{
					row += "d";
				}

				if (UnityEngine.Random.value <= forestPercentage)
				{
					row += "w";
				}

				if (UnityEngine.Random.value <= waterPercentage)
				{
					row += "r";
				}
				else
				{
					row += "-";
				}
			}
			gridString.Add(row);
		}
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
