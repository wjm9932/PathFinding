using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Character character;

    private LineRenderer lineRenderer;
    private PathFinder pathFinder;
    private List<Cell> path = new List<Cell>();
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pathFinder = new PathFinder(board, lineRenderer);
    }
    void Start()
    {
        if(character != null)
        board.onBoardChanged += character.ChangePathIfNeeded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            path = pathFinder.FindPath(board.start.transform.position, board.destination.transform.position, true);
            if (path == null)
            {
                Debug.Log("There is no path to reach the target cell");
            }
        }


        if(Input.GetMouseButtonDown(1))
        {
            if (character != null)
            {
                character.SetDestination(Utility.GetMouseWorldPosition(), pathFinder);
            }
        }
    }

}
