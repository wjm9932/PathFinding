using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public event Action<Cell> onBoardChanged;

    [SerializeField] private GameObject board;

    public Cell[,] cell { get; private set; }

    public Dictionary<Vector3Int, GameObject> walls = new Dictionary<Vector3Int, GameObject>();
    public GameObject start { get; private set; }
    public GameObject destination { get; private set; }
    public Grid grid { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }


    private void Awake()
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

        grid = GetComponent<Grid>();
    }

    void Start()
    {

    }
    private void Update()
    {
        //Debug.Log(cell[0, 0].isWall);
    }
    public bool IsInRange(Vector3Int position)
    {
        return position.x >= 0 && position.x < width &&
       position.y >= 0 && position.y < height;
    }

    public bool IsWall(Vector3Int pos)
    {
        if (IsInRange(pos) == false)
        {
            return false;
        }

        if (cell[pos.x, pos.y].isWall == false)
        {
            return false;
        }

        return true;
    }

    public void SetWall(GameObject wallPrefab, Vector3 pos, Vector3Int cellIndex, Quaternion rotation)
    {
        var wall = Instantiate(wallPrefab, pos, rotation);
        walls.Add(cellIndex, wall);

        cell[cellIndex.x, cellIndex.y].isWall = true;

        onBoardChanged?.Invoke(cell[cellIndex.x, cellIndex.y]);
    }

    public void SetStartCell(GameObject startPrefab, Vector3 pos)
    {
        start = Instantiate(startPrefab, pos, Quaternion.identity);
    }

    public void SetDestinationCell(GameObject destinationPrefab, Vector3 pos)
    {
        destination = Instantiate(destinationPrefab, pos, Quaternion.identity);
    }

    public void DeleteWall(Vector3Int pos)
    {
        if (walls.ContainsKey(pos) == true)
        {
            Vector3Int gridPos = grid.WorldToCell(pos);
            cell[gridPos.x, gridPos.y].isWall = false;

            Destroy(walls[pos]);
            walls.Remove(pos);
        }
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

    public Cell GetCell(Vector3 pos)
    {
        return cell[grid.WorldToCell(pos).x, grid.WorldToCell(pos).y];
    }

    public void ClearBoard()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (cell[x, y].isWall == true)
                {
                    cell[x, y].isWall = false;
                    Destroy(walls[new Vector3Int(x, y)]);
                    walls.Remove(new Vector3Int(x, y));
                }
            }
        }
    }
}
