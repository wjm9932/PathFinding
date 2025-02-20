using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Board board;

    private List<Cell> openList = new List<Cell>();
    private List<Cell> closedList = new List<Cell>();


    public PathFinder(Board board)
    {
        this.board = board;
    }

    public List<Cell> FindPath()
    {
        Reset();

        Cell startCell = board.cell[0, 0];
        Cell targetCell = board.cell[board.width - 1, board.height - 1];

        startCell.gCost = 0;
        startCell.hCost = CalculateHCost(startCell, targetCell);
        openList.Add(startCell);

        while (openList.Count > 0)
        {
            Cell currentCell = GetLowestFCostCell(openList);

            if (currentCell == targetCell)
            {
                return CalculatePath(targetCell);
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (Cell neighborCell in GetNeighborCell(currentCell))
            {
                if(closedList.Contains(neighborCell) == true)
                {
                    continue;
                }
                if(neighborCell.isWall == true || CanMoveDiagonally(currentCell, neighborCell) == false)
                {
                    continue;
                }

                int tentativeGCost = currentCell.gCost + CalculateGCost(currentCell, neighborCell);
                if(tentativeGCost < neighborCell.gCost)
                {
                    neighborCell.cameFromCell = currentCell;
                    neighborCell.gCost = tentativeGCost;
                    neighborCell.hCost = CalculateHCost(neighborCell, targetCell);

                    if(openList.Contains(neighborCell) == false)
                    {
                        openList.Add(neighborCell);
                    }
                }
            }
        }

        return null;
    }

    private List<Cell> CalculatePath(Cell targetCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = targetCell;

        path.Add(targetCell);
        
        while(currentCell.cameFromCell != null)
        {
            path.Add(currentCell.cameFromCell);
            currentCell = currentCell.cameFromCell;
        }

        path.Reverse();
        return path;
    }

    private bool CanMoveDiagonally(Cell currentCell, Cell neighbor)
    {
        if(GetCell(currentCell.x, neighbor.y).isWall == true && GetCell(neighbor.x, currentCell.y).isWall == true)
        {
            return false;
        }
        return true;
    }

    private int CalculateHCost(Cell a, Cell b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        return (xDistance + yDistance) * 10;
    }

    private int CalculateGCost(Cell a, Cell b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        if (xDistance != 0 && yDistance != 0)
        {
            return MOVE_DIAGONAL_COST;  
        }
        else
        {
            return MOVE_STRAIGHT_COST; 
        }
    }

    private List<Cell> GetNeighborCell(Cell currentCell)
    {
        List<Cell> neighborCell = new List<Cell>();

        //Left
        if (currentCell.x - 1 >= 0)
        {
            neighborCell.Add(GetCell(currentCell.x - 1, currentCell.y));

            //Left Down
            if (currentCell.y - 1 >= 0)
            {
                neighborCell.Add(GetCell(currentCell.x - 1, currentCell.y - 1));
            }

            //Left Up
            if (currentCell.y + 1 < board.height)
            {

                neighborCell.Add(GetCell(currentCell.x - 1, currentCell.y + 1));
            }
        }

        //Right
        if (currentCell.x + 1 < board.width)
        {
            neighborCell.Add(GetCell(currentCell.x + 1, currentCell.y));

            //Right Down
            if (currentCell.y - 1 >= 0)
            {
                neighborCell.Add(GetCell(currentCell.x + 1, currentCell.y - 1));
            }

            //Right Up
            if (currentCell.y + 1 < board.height)
            {
                neighborCell.Add(GetCell(currentCell.x + 1, currentCell.y + 1));
            }
        }

        //Down
        if (currentCell.y - 1 >= 0)
        {
            neighborCell.Add(GetCell(currentCell.x, currentCell.y - 1));
        }

        //Up
        if (currentCell.y + 1 < board.height)
        {
            neighborCell.Add(GetCell(currentCell.x, currentCell.y + 1));
        }

        return neighborCell;
    }

    private Cell GetCell(int x, int y)
    {
        return board.cell[x, y];
    }

    private Cell GetLowestFCostCell(List<Cell> cellList)
    {
        Cell lowestCell = cellList[0];

        for (int i = 1; i < cellList.Count; i++)
        {
            if (cellList[i].fCost <= lowestCell.fCost && cellList[i].gCost < lowestCell.gCost)
            {
                lowestCell = cellList[i];
            }
        }
        return lowestCell;
    }

    private void Reset()
    { 
        openList.Clear();
        closedList.Clear();
        board.ResetCells();
    }
}
