using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridScript : MonoBehaviour {

	public int gridWidth;
	public int gridHeight;
	public float spacing;
	
	public Material[] mats;
	public float[]   costs;

	public Vector3 start = new Vector3(0,0);
	public Vector3 goal = new Vector3(14,14);
	
	GameObject[,] gridArray;
	
	public GameObject startSprite;
	public GameObject goalSprite;

	public GameObject[,] GetGrid(){
		
		if (gridArray != null) return gridArray;
		
		gridArray = new GameObject[gridWidth, gridHeight];
			
		var offsetX = (gridWidth  * -spacing)/2f;
		var offsetY = (gridHeight * spacing)/2f;

		for(var x = 0; x < gridWidth; x++){
			for(var y = 0; y < gridHeight; y++){
				var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.transform.localScale = new Vector3(spacing, spacing, spacing);
				quad.transform.position = new Vector3(offsetX + x * spacing, 
					offsetY - y * spacing, 0);

				quad.transform.parent = transform;

				gridArray[x, y] = quad;
					
				quad.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(x, y);

				if(Math.Abs(goal.x - x) < 0.01f && Math.Abs(goal.y - y) < 0.01f){
					goalSprite.transform.position = quad.transform.position;
				}
				if(Math.Abs(start.x - x) < 0.01f && Math.Abs(start.y - y) < 0.01f){
					startSprite.transform.position = quad.transform.position;
				}
			}
		}

		return gridArray;
	}

	public virtual float GetMovementCost(GameObject go){
		var mat = go.GetComponent<MeshRenderer>().sharedMaterial;
		int i;

		for(i = 0; i < mats.Length; i++){
			if(mat.name.StartsWith(mats[i].name))
			{
				break;
			}
		}
		return costs[i];
	}

	protected virtual Material GetMaterial(int x, int y)
	{
		return mats[Random.Range(0,mats.Length)];
	}
}
