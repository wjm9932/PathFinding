using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditGrid : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;

    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject destinationPrefab;

    private Grid grid;
    private Board board;

    private GameObject draggingObject;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        board = GetComponent<Board>();
    }

    void Start()
    {
        board.SetStartCell(startPrefab, grid.CellToWorld(new Vector3Int(0, 0, 0)));
        board.SetDestinationCell(destinationPrefab, grid.CellToWorld(new Vector3Int(board.width - 1, board.height - 1, 0)));
    }

    void Update()
    {
        Vector3Int mouseCellPos = grid.WorldToCell(Utility.GetMouseWorldPosition());
        Vector3Int startCellPos = grid.WorldToCell(board.start.transform.position);
        Vector3Int destinationCellPos = grid.WorldToCell(board.destination.transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseCellPos == startCellPos)
            {
                draggingObject = board.start;
            }
            else if (mouseCellPos == destinationCellPos)
            {
                draggingObject = board.destination;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingObject = null;
        }

        if (draggingObject != null)
        {
            if (board.IsInRange(mouseCellPos) && !board.IsWall(mouseCellPos))
            {
                draggingObject.transform.position = grid.CellToWorld(mouseCellPos);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (board.IsInRange(mouseCellPos) == true)
            {
                if (board.IsWall(mouseCellPos) == false && !(startCellPos == mouseCellPos || destinationCellPos == mouseCellPos))
                {
                    board.SetWall(wallPrefab, grid.CellToWorld(mouseCellPos), mouseCellPos, wallPrefab.transform.rotation);
                }
            }
        }

        if(Input.GetMouseButton(2))
        {
            board.DeleteWall(mouseCellPos);
        }
    }
}
