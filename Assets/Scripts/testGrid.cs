using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;
//UtilsClass.GetMouseWorldPosition(),
public class testGrid : MonoBehaviour
{
    [SerializeField]
    private int gridXsize;
    [SerializeField]
    private int gridYsize;
    [SerializeField]
    private float cellSize;


    [SerializeField]
    private Tilemap unitytilemap;

    [SerializeField]
    private Tile cellTile;
    

    private grid<cell> CellGrid;
    private void Start()
    {
        
        CellGrid = new grid<cell>(gridXsize, gridYsize, cellSize, transform.position, (grid<cell> g, int x, int y) => new cell(g, x, y));
        Time.timeScale = 0f;
        
       /* CellGrid.GetValue(9, 9).become_alive();
        CellGrid.GetValue(10, 9).become_alive();
        CellGrid.GetValue(11, 9).become_alive();
        CellGrid.GetValue(11, 10).become_alive();
        CellGrid.GetValue(10, 11).become_alive();*/

    }


    private void Update()
    {
        Vector3 mousepos = UtilsClass.GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0))
        {
            CellGrid.GetValue(mousepos).become_alive();
        }
        if (Input.GetMouseButtonDown(1))
        {
            CellGrid.GetValue(mousepos).die();
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale == 0f)
        {
            Debug.Log("Neighbors: " + count_alive_neighbors(1, 1));
            Time.timeScale = 0.2f;
        }

        for (int x = 0; x < gridXsize; x++)
        {
            for (int y = 0; y < gridYsize; y++)
            {
                if(CellGrid.GetValue(x, y).isAlive() && !unitytilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    unitytilemap.SetTile(new Vector3Int(x, y, 0), cellTile);
                }
                else if (!CellGrid.GetValue(x, y).isAlive() && unitytilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    unitytilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
                else
                {

                }
            }
        }

    }

    private void FixedUpdate()
    {

        for (int x = 0; x < gridXsize; x++)
        {
            for(int y = 0; y < gridYsize; y++)
            {
                updateCells(x, y);
            }
        }

        for (int x = 0; x < gridXsize; x++)
        {
            for (int y = 0; y < gridYsize; y++)
            {
                CellGrid.GetValue(x, y).update_alive_status();
            }
        }

        //Time.timeScale = 0f;
    }

    public void updateCells(int x, int y)
    {
        int count = count_alive_neighbors(x, y);
        CellGrid.GetValue(x, y).update_Alive_neighbors(count);  
    }
    public int count_alive_neighbors(int x, int y)
    {
        List<cell> neighbors = CellGrid.list_neighbors(x, y);
        int counter = 0;
        foreach (cell cel in neighbors)
        {
            if(cel.isAlive())
            {
                counter++;
            }
        }

        return counter;
    }


}
