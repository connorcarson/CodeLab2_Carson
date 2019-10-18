using System;
using UnityEngine;
using System.Collections;

public class HueristicScript : MonoBehaviour {
		
	public virtual float Hueristic(int x, int y, Vector3 start, Vector3 goal, GridScript gridScript)
	{
		var _pos = gridScript.GetGrid();
		
		return gridScript.GetMovementCost(_pos[x, y]) * Math.Abs(goal.x - x) + Math.Abs(goal.y - y);
		//return Random.Range(0, 500);
	}
}
