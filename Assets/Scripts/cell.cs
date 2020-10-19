using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell
{
    private bool alive_status;
    private int neighbors;

    private grid<cell> Grid;
    private int x, y, value;


    public cell(grid<cell> Grid, int x, int y)
    {
        alive_status = false;
        this.Grid = Grid;
        this.x = x;
        this.y = y;
    }

    public void update_alive_status()
    {
        if (neighbors == 3 && alive_status == false)
        {
            become_alive();
            
        }
        else if ((neighbors > 3 || neighbors < 2) && alive_status == true)
        {
            die();
        }
        else
        {
        }
    }

    public void die()
    {
        alive_status = false;
        Grid.TriggerGridObjectChanged(x, y);
    }
    public void become_alive()
    {
        alive_status = true;
        Grid.TriggerGridObjectChanged(x, y);
    }

    public bool isAlive()
    {
        return alive_status;
    }

    public void update_Alive_neighbors(int count)
    {
        neighbors = count;
    }
    public override string ToString()
    {
        return alive_status.ToString();
    }
}
