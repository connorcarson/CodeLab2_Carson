using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarScript : MonoBehaviour {

	public bool check = true;

	public GridScript gridScript;
	public HueristicScript hueristic;

	private int _gridWidth;
	private int _gridHeight;

	private GameObject[,] _pos;

	//A Star stuff
	private Vector3 _start;
	private Vector3 _goal;

	public Path path;

	private PriorityQueue<Vector3> _frontier;
	private Dictionary<Vector3, Vector3> _cameFrom = new Dictionary<Vector3, Vector3>();
	private Dictionary<Vector3, float> _costSoFar = new Dictionary<Vector3, float>();
	private Vector3 _current;

	private List<Vector3> _visited = new List<Vector3>();

	// Use this for initialization
	protected virtual void Start () {
		InitAstar();
	}

	protected virtual void InitAstar(){
		InitAstar(new Path(hueristic.gameObject.name, gridScript));
	}

	protected virtual void InitAstar(Path path){
		this.path = path;

		_start = gridScript.start;
		_goal = gridScript.goal;
		
		_gridWidth = gridScript.gridWidth;
		_gridHeight = gridScript.gridHeight;

		_pos = gridScript.GetGrid();

		_frontier = new PriorityQueue<Vector3>();
		_frontier.Enqueue(_start, 0);

		_cameFrom.Add(_start, _start);
		_costSoFar.Add(_start, 0);

		var exploredNodes = 0;

		while(_frontier.Count != 0){
			exploredNodes++;
			_current = _frontier.Dequeue();

			_visited.Add(_current);

			// _pos[(int)_current.x, (int)_current.y].transform.localScale = Vector3.Scale(_pos[(int)_current.x, (int)_current.y].transform.localScale, new Vector3(.8f, .8f, .8f));

			if(_current.Equals(_goal)){
				Debug.Log("GOOOAL!");
				break;
			}
			
			for(var x = -1; x < 2; x+=2){
				AddNodesToFrontier((int)_current.x + x, (int)_current.y);
			}
			for(var y = -1; y < 2; y+=2){
				AddNodesToFrontier((int)_current.x, (int)_current.y + y);
			}
		}

		_current = _goal;

		var line = GetComponent<LineRenderer>();

		var i = 0;
		float score = 0;

		while(!_current.Equals(_start)){
			line.positionCount++;
			
			var go = _pos[(int)_current.x, (int)_current.y];
			path.Insert(0, go, new Vector3((int)_current.x, (int)_current.y));

			_current = _cameFrom[_current];

			var vec = Util.clone(go.transform.position);
			vec.z = -1;

			line.SetPosition(i, vec);
			score += gridScript.GetMovementCost(go);
			i++;
		}

		path.Insert(0, _pos[(int)_current.x, (int)_current.y]);
		path.nodeInspected = exploredNodes;
		
		Debug.Log(path.pathName + " Terrain Score: " + score);
		Debug.Log(path.pathName + " Nodes Checked: " + exploredNodes);
		Debug.Log(path.pathName + " Total Score: " + (score + exploredNodes));
	}

	private void AddNodesToFrontier(int x, int y){
		if (x < 0 || x >= _gridWidth || y < 0 || y >= _gridHeight) return;
		
		var next = new Vector3(x, y);
		var new_cost = _costSoFar[_current] + gridScript.GetMovementCost(_pos[x, y]);
		
		if (_costSoFar.ContainsKey(next) && !(new_cost < _costSoFar[next])) return;
		
		_costSoFar[next] = new_cost;
		var priority = new_cost + hueristic.Hueristic(x, y, _start, _goal, gridScript);

		_frontier.Enqueue(next, priority);
		_cameFrom[next] = _current;
	}
}
