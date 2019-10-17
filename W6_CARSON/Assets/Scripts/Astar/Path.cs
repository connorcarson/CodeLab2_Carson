using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path {

	public string pathName;
	public int nodeInspected;

	private GridScript _gridScipt;
	private List<Step> _path = new List<Step>();

	private float _score;
	public int steps;

	public Path(string name, GridScript gridScipt){
		_gridScipt = gridScipt;
		pathName = name;
	}

	public Step Get(int index){
		return _path[index];
	}
	
	public virtual void Insert(int index, GameObject go){
		var stepCost = _gridScipt.GetMovementCost(go);
		_score += stepCost;
		
		_path.Insert(index, new Step(go, stepCost));
		
		steps++;
	}

	public virtual void Insert(int index, GameObject go, Vector3 gridPos){
		var stepCost = _gridScipt.GetMovementCost(go);
		_score += stepCost;
		
		_path.Insert(index, new Step(go, stepCost, gridPos));

		steps++;
	}
}
