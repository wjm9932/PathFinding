
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{ 
    public int x { get; private set; }
    public int y { get; private set; }

    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }

    public bool isWall;
    public bool isStart;
    public bool isDestination;
    public Cell cameFromCell;

    public Cell(int x, int y, bool isWall = false)
    {
        this.x = x;
        this.y = y;
        this.isWall = isWall;
        this.gCost = int.MaxValue;
        cameFromCell = null;
    }

    public void Reset()
    {
        this.gCost = int.MaxValue;
        cameFromCell = null;
    }
}
