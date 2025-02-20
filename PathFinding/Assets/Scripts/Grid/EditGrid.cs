using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditGrid : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;


    private Grid grid;
    private Board board;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        board = GetComponent<Board>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3Int gridPosition = grid.WorldToCell(Utility.GetMouseWorldPosition());
            if (board.IsInRange(gridPosition) == true)
            {
                if (board.IsWall(gridPosition) == false)
                {
                    board.SetWall(wallPrefab, grid.CellToWorld(gridPosition), wallPrefab.transform.rotation);
                }
            }
        }

        if(Input.GetMouseButton(1))
        {
            Vector3Int gridPosition = grid.WorldToCell(Utility.GetMouseWorldPosition());
            board.DeleteWall(gridPosition);
        }
    }
}
