using UnityEngine;
using System.Collections;

public class Step {

	public GameObject gameObject;
	public float moveCost;
	private Vector3 _gridPos;

	
	public Step(GameObject gameObject, float cost){
		this.gameObject = gameObject;
		moveCost = cost;
	}

	public Step(GameObject gameObject, float cost, Vector3 gridPos){
		this.gameObject = gameObject;
		moveCost = cost;
		_gridPos = gridPos;
	}
}
