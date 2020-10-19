using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;
public class grid<TGridObject> 
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {

        public int x;
        public int y;
        
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public grid(int width, int height, float cellSize, Vector3 originPosition, Func<grid<TGridObject>, int, int, TGridObject> createCells)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];
        //TextMesh[,] debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createCells(this, x, y);
                //debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 10, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs e) =>
        {
            //debugTextArray[e.x, e.y].text = gridArray[e.x, e.y]?.ToString();
        };
    }
    private bool inBounds(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetValue(int x, int y, TGridObject value)
    {
        if (inBounds(x, y))
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }  
    }
    
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (inBounds(x, y))
        {
            return gridArray[x, y];

        } else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }


    public List<TGridObject> list_neighbors(int x, int y)
    {
        if (inBounds(x, y))
        {
            List<TGridObject> neighbors = new List<TGridObject>();
            for (int xoff = -1; xoff <= 1; xoff++)
            {
                for (int yoff = -1; yoff <= 1; yoff++)
                {
                   int i = x + xoff;
                   int j = y + yoff;
                   if (inBounds(i, j) && !(i == x && j == y))
                   {
                        neighbors.Add(gridArray[i, j]);
                   }
                }
            }

            return neighbors;
        }
        else
        {
            return null;
        }
    }

}
