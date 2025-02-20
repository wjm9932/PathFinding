using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed;

    private List<Cell> path;
    private int currentPathIndex;

    private PathFinder pathFinder;
    private Vector3 destination;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(path != null)
        {
            Vector3 destination = new Vector3(path[currentPathIndex].x, path[currentPathIndex].y);
            destination.x += 0.5f;
            destination.y += 0.5f;

            if(Vector3.Distance(transform.position, destination) > 0.1f)
            {
                Vector3 moveDir = (destination - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= path.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        path = null;
        pathFinder = null;
    }


    public void ChangePathIfNeeded(Cell cell)
    {
        if(pathFinder != null)
        {
            for (int i = currentPathIndex; i < path.Count; i++)
            {
                if (path[i] == cell || (i < path.Count - 1 && !pathFinder.CanMoveDiagonally(path[i], path[i + 1])))
                {
                    SetDestination(destination, pathFinder);
                    return;
                }
            }
        }
    }

    public void SetDestination(Vector3 targetPosition, PathFinder pathFinder)
    {
        StopMoving();

        var tentativePath = pathFinder.FindPath(transform.position, targetPosition, true);

        if(tentativePath != null)
        {
            destination = targetPosition;
            this.pathFinder = pathFinder;

            path = tentativePath;
            currentPathIndex = 0;

            if (path != null && path.Count > 1)
            {
                path.RemoveAt(0);
            }
        }
    }
}
