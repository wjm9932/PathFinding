using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Character character;

    private PathFinder pathFinder;
    private List<Cell> path = new List<Cell>();

    private LineRenderer lineRenderer;
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
            if(board.start != null)
            {
                path = pathFinder.FindPath(board.start.transform.position, board.destination.transform.position, true);
                if (path == null)
                {
                    Debug.Log("There is no path to reach the target cell");
                }
            }
        }


        if(Input.GetMouseButtonDown(1))
        {
            if (character != null)
            {
                character.SetDestination(Utility.GetMouseWorldPosition(), pathFinder);
            }
        }

        if(Input.GetKeyDown(KeyCode.C) == true)
        {
            board.ClearBoard();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) == true)
        {
            if (SceneManager.GetActiveScene().name != "First")
            {
                SceneManager.LoadScene("First");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
        {
            if (SceneManager.GetActiveScene().name != "Second")
            {
                SceneManager.LoadScene("Second");
            }
        }
    }
}
