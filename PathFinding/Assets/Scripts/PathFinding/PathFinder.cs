using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Board board;
    private LineRenderer lineRenderer;

    private List<Cell> openList = new List<Cell>();
    private List<Cell> closedList = new List<Cell>();


    public PathFinder(Board board, LineRenderer lineRenderer)
    {
        this.board = board;
        this.lineRenderer = lineRenderer;
    }

    public List<Cell> FindPath(Vector3 start, Vector3 end, bool isDrawPath = false)
    {
        if (board.IsInRange(board.grid.WorldToCell(end)) == false)
        {
            return null;
        }

        Reset();

        Cell startCell = board.GetCell(start);
        Cell targetCell = board.GetCell(end);

        startCell.gCost = 0;
        startCell.hCost = CalculateHCost(startCell, targetCell);
        openList.Add(startCell);

        while (openList.Count > 0)
        {
            Cell currentCell = GetLowestFCostCell(openList);

            if (currentCell == targetCell)
            {
                var path = CalculatePath(targetCell);
                DrawPath(path);
                return path;
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (Cell neighborCell in GetNeighborCell(currentCell))
            {
                if (closedList.Contains(neighborCell) == true)
                {
                    continue;
                }
                if (neighborCell.isWall == true || CanMoveDiagonally(currentCell, neighborCell) == false)
                {
                    continue;
                }

                int tentativeGCost = currentCell.gCost + CalculateGCost(currentCell, neighborCell);
                if (tentativeGCost < neighborCell.gCost)
                {
                    neighborCell.cameFromCell = currentCell;
                    neighborCell.gCost = tentativeGCost;
                    neighborCell.hCost = CalculateHCost(neighborCell, targetCell);

                    if (openList.Contains(neighborCell) == false)
                    {
                        openList.Add(neighborCell);
                    }
                }
            }
        }
        DrawPath(null);
        return null;
    }

    private List<Cell> CalculatePath(Cell targetCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = targetCell;

        path.Add(targetCell);

        while (currentCell.cameFromCell != null)
        {
            path.Add(currentCell.cameFromCell);
            currentCell = currentCell.cameFromCell;
        }

        path.Reverse();
        return path;
    }

    public bool CanMoveDiagonally(Cell currentCell, Cell neighbor)
    {
        if (GetCell(currentCell.x, neighbor.y).isWall == true && GetCell(neighbor.x, currentCell.y).isWall == true)
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

    private void DrawPath(List<Cell> path)
    {
        Grid grid = board.GetComponent<Grid>();
        float cellOffset = grid.cellSize.x * 0.5f;

        if (path == null)
        {
            lineRenderer.positionCount = 0;
        }
        else
        {
            lineRenderer.positionCount = path.Count;
        }

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 worldPos = new Vector3(path[i].x, path[i].y, 0);
            worldPos.x += cellOffset;
            worldPos.y += cellOffset;

            lineRenderer.SetPosition(i, worldPos);
        }
    }

}
