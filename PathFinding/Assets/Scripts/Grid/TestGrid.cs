using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Vector3 originPosition;
    private TextMesh[,] gridText;
    public TestGrid(int width, int height, float cellSize, Vector3 originPosition = default(Vector3))
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridArray = new int[width, height];
        this.originPosition = originPosition;
        this.gridText = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridText[x, y] = Utility.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y), 20, Color.white, TextAnchor.MiddleCenter);

                Vector3 offset = new Vector3(cellSize / 2, cellSize / 2, 0);
                Vector3 start = GetWorldPosition(x, y) - offset;
                Vector3 right = GetWorldPosition(x + 1, y) - offset;
                Vector3 top = GetWorldPosition(x, y + 1) - offset;

                Debug.DrawLine(start, top, Color.white, 100f);
                Debug.DrawLine(start, right, Color.white, 100f);
            }
        }

        Vector3 offsetBoundary = new Vector3(cellSize / 2, cellSize / 2, 0);
        Debug.DrawLine(GetWorldPosition(0, height) - offsetBoundary, GetWorldPosition(width, height) - offsetBoundary, Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0) - offsetBoundary, GetWorldPosition(width, height) - offsetBoundary, Color.white, 100f);
    }
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x <= width && y <= height)
        {
            gridArray[x, y] = value;
            gridText[x, y].text = gridArray[x, y].ToString();
        }
        else
        {
            Debug.LogWarning("out of board range");
        }
    }
    public void SetValue(Vector3 position, int value)
    {
        var coordinate = GetWorldPosition(position);
        SetValue(coordinate.x, coordinate.y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= width && y <= height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        var coordinate = GetWorldPosition(worldPosition);
        return GetValue(coordinate.x, coordinate.y);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private Vector2Int GetWorldPosition(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - originPosition;

        int gridX = Mathf.FloorToInt((localPosition.x + cellSize / 2f) / cellSize);
        int gridY = Mathf.FloorToInt((localPosition.y + cellSize / 2f) / cellSize);

        return new Vector2Int(gridX, gridY);
    }

}
