using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonMonobehaviour<GridManager>
{

	public int numOfRows = 50;
	public int numOfColums = 50;

	public float girdCellSize = 1.0f;
	public float gridCelWidth = 128.0f;
	public float gridCellHeight = 96.0f;

	public float halfGridCellWidth = 64.0f;
	public float halfGridCellHeight = 48.0f;

	public Vector3 origin = new Vector3(0.0f, -2400.0f, 0.0f);

	public Grid[,] grids { get; set; }

	private Transform myTransform;

	public Vector3 GetGridposition(int index)
    {
		int row = index % numOfRows;
		int colums = index / numOfColums;

		float xPos = colums * halfGridCellWidth - row * halfGridCellWidth;
		float yPos = row * halfGridCellHeight + colums * halfGridCellHeight;

		return myTransform.position + new Vector3(xPos, yPos, 0.0f);

    }

	public Vector3 GetVectorPosition(int index)
    {
		Vector3 girdPosition = GetGridposition(index);
		girdPosition.x += halfGridCellHeight;
		return girdPosition;
    }

	public void CreateGrid()
    {
		this.grids = new Grid[numOfColums, numOfColums];
		int index = 0;
		for(int i = 0; i < numOfColums; i++)
        {
			for(int j =0; j < numOfColums; j++)
            {
				Vector3 girdPosition = GetGridposition(index);
				Grid grid = new Grid(girdPosition);
				grid.SetColRow(i, j);
				this.grids[i, j] = grid;

				GameObject gridGameObject = new GameObject(i.ToString() + "_" + j.ToString() + "_" + index.ToString());
				gridGameObject.transform.position = girdPosition;
				gridGameObject.transform.localScale = Vector3.one;
				gridGameObject.transform.rotation = Quaternion.identity;
				gridGameObject.transform.SetParent(myTransform);
				gridGameObject.layer = LayerMask.NameToLayer("Grid");

				PolygonCollider2D collider2D = gridGameObject.AddComponent<PolygonCollider2D>();
				Vector2[] points = new Vector2[]
				{
					new Vector2(0.0f, halfGridCellHeight),
					new Vector2(0.0f, 0.0f),
					new Vector2(0.0f, halfGridCellHeight),
					new Vector2(-halfGridCellHeight, 0.0f),
					new Vector2(0.0f, -halfGridCellHeight),
					new Vector2(halfGridCellHeight, 0.0f)
				};
				collider2D.points = points;
				collider2D.isTrigger = true;

				index++;

			}
			
        }
    }

	public void Init ()
	{
		myTransform = transform;
		myTransform.position = origin;

		CreateGrid();
		
	}
	

}
