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
		// Make the grid be generated into gridString at random w/ the above percentages.
		//for each row in our grid
		for (int y = 0; y < gridHeight; y++)
		{
			//start with an empty string
			string row = "";
			//for each column in our grid
			for (int X = 0; X < gridWidth; X++)
			{
				//if a random number between 0.0 and 1.0 is less than or equal to the chance of rocks
				if (UnityEngine.Random.value <= rockPercentage)
				{
					//add the letter d to our empty string
					row += "d";
				}
				//if a random number between 0.0 and 1.0 is less than or equal to the chance of forest
				if (UnityEngine.Random.value <= forestPercentage)
				{
					//add the letter w to our empty string
					row += "w";
				}
				//if a random number between 0.0 and 1.0 is less than or equal to the chance of water
				if (UnityEngine.Random.value <= waterPercentage)
				{
					//add the letter r to our empty string
					row += "r";
				}
				else
				{
					//add the meaningless '-' to our string
					row += "-";
				}
			}
			//add our randomly generated string to our list of strings
			gridString.Add(row);
		}
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
