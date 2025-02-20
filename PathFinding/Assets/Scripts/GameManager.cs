using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;

    private LineRenderer lineRenderer;
    private PathFinder pathFinder;
    private List<Cell> path = new List<Cell>();
    private void Awake()
    {
        pathFinder = new PathFinder(board);
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            path = pathFinder.FindPath();
            if (path == null)
            {
                Debug.Log("There is no path to reach the target cell");
                DrawPath();
            }
            else
            {
                DrawPath();
            }
        }
    }

    private void DrawPath()
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
            // Cell ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥·Î º¯È¯
            Vector3 worldPos = grid.CellToWorld(new Vector3Int(path[i].x, path[i].y, 0));
            worldPos.x += cellOffset;
            worldPos.y += cellOffset;

            lineRenderer.SetPosition(i, worldPos);
        }
    }
}
