using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private TestGrid grid;
    void Start()
    {
        grid = new TestGrid(4, 2, 10f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utility.GetMouseWorldPosition(), 56);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(Utility.GetMouseWorldPosition()));
        }
    }
}
