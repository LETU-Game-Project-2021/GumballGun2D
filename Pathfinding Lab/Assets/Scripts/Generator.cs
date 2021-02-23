using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int rows, columns;
    public Vector2 start, end;
    // Start is called before the first frame update
    void Start()
    {
        float intervalX = (end.x - start.x) / (columns - 1);
        float intervalY = (end.y - start.y) / (rows - 1);
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                GameObject node = Instantiate(Resources.Load("NodePrefab"), new Vector3(start.x+intervalX*j, start.y+intervalY*i, 0), Quaternion.identity) as GameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
