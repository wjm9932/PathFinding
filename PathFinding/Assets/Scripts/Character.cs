using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed;

    private List<Vector3> path;
    private int currentPathIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(path != null)
        {
            Vector3 destination = path[currentPathIndex];
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
    }

    public void SetDestination(Vector3 targetPosition, PathFinder pathFinder)
    {
        var tentativePath = pathFinder.FindPath(transform.position, targetPosition, true);

        if(tentativePath != null)
        {
            path = tentativePath;
            currentPathIndex = 0;

            if (path != null && path.Count > 1)
            {
                path.RemoveAt(0);
            }
        }
    }
}
