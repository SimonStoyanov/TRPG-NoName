using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Select : MonoBehaviour
{
    RaycastHit2D hit2D;
    //You need references to to the Grid and the Tilemap
    //Tilemap tm;
    Grid gd;

    [SerializeField]
    private Tile target;

    bool loaded = false;
    Vector3Int prevPos;

    void Start()
    {
        gd = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
