using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject board;

    public Cell[,] cell { get; private set; }

    public Dictionary<Vector2Int, GameObject> walls = new Dictionary<Vector2Int, GameObject>();

    public int width { get; private set; }
    public int height { get; private set; }

    void Start()
    {
        width = Mathf.RoundToInt(board.transform.localScale.x * 10f);
        height = Mathf.RoundToInt(board.transform.localScale.z * 10f);

        cell = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell[x, y] = new Cell(x, y);
            }
        }

    }
    private void Update()
    {
        //Debug.Log(cell[0, 0].isWall);
    }
    public bool IsInRange(Vector3 position)
    {
        return position.x >= 0 && position.x < width &&
       position.y >= 0 && position.y < height;
    }

    public bool IsWall(Vector3 pos)
    {
        if (IsInRange(pos) == false)
        {
            return false;
        }

        Vector2Int gridPos = ConvertToGridIndex(pos);
        if (cell[gridPos.x, gridPos.y].isWall == false)
        {
            return false;
        }

        return true;
    }

    public void SetWall(GameObject wallPrefab, Vector3 pos, Quaternion rotation)
    {
        Vector2Int gridPos = ConvertToGridIndex(pos);

        var wall = Instantiate(wallPrefab, pos, rotation);
        walls.Add(gridPos, wall);

        cell[gridPos.x, gridPos.y].isWall = true;
    }

    public void DeleteWall(Vector3 pos)
    {
        Vector2Int gridPos = ConvertToGridIndex(pos);
        if (walls.ContainsKey(gridPos) == true)
        {
            cell[gridPos.x, gridPos.y].isWall = false;
            Destroy(walls[gridPos]);
            walls.Remove(gridPos);
        }
    }

    private Vector2Int ConvertToGridIndex(Vector3 worldPos)
    {
        int gridX = Mathf.FloorToInt(worldPos.x);
        int gridY = Mathf.FloorToInt(worldPos.y);
        return new Vector2Int(gridX, gridY);
    }

    public void ResetCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell[x, y].Reset();
            }
        }
    }
}
