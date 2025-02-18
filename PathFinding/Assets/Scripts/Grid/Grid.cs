using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        
        gridArray = new int[width, height];

        for(int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.Log(i + ", " + j);
            }
        }
    }

}
