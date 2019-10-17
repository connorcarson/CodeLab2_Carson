using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowAStarScript : MonoBehaviour {
	private bool _move = false;
	private Path _path;
	public AStarScript astar;
	public Step startPos;
	public Step destPos;

	private int _currentStep = 1;

	private float _lerpPer = 0;

	private float _startTime;
	private float _travelStartTime;

	protected virtual void Start () {
		_path = astar.path;
		startPos = _path.Get(0);
		destPos  = _path.Get(_currentStep);

		transform.position = startPos.gameObject.transform.position;

		Invoke(nameof(StartMove), _path.nodeInspected/100f);

		_startTime = Time.realtimeSinceStartup;
	}
	
	protected virtual void Update () {
		if (!_move) return;
		
		_lerpPer += Time.deltaTime/destPos.moveCost;

		transform.position = Vector3.Lerp(startPos.gameObject.transform.position, 
			destPos.gameObject.transform.position, 
			_lerpPer);

		if (!(_lerpPer >= 1)) return;
		
		_lerpPer = 0;

		_currentStep++;

		if(_currentStep >= _path.steps){
			_currentStep = 0;
			_move = false;
			Debug.Log(_path.pathName + " got to the goal in: " + (Time.realtimeSinceStartup - _startTime));
			Debug.Log(_path.pathName + " travel time: " + (Time.realtimeSinceStartup - _travelStartTime));
		} 

		startPos = destPos;
		destPos = _path.Get(_currentStep);
	}

	protected virtual void StartMove(){
		_move = true;
		_travelStartTime = Time.realtimeSinceStartup;
	}
}

