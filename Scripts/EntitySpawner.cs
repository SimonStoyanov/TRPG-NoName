using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntitySpawner : MonoBehaviour
{
    enum EntityType
    {
        player1,
        player2,
        player3,
        player4,

        enemy1,
        enemy2,
        enemy3,
        enemy4,
        enemy5
    }

    [SerializeField]
    private GameObject entity = null;

    Grid grid = null;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        SpawnEntitiesInMap();
        //Instantiate(entity, grid.CellToWorld(new Vector3Int(-7, -2, 0)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEntitiesInMap()
    {
        Tilemap tm = GameObject.FindGameObjectWithTag("EntityTM").GetComponent<Tilemap>();
        BoundsInt bounds = tm.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (tm.HasTile(position))
                Instantiate(entity, grid.CellToWorld(position), Quaternion.identity);
        }

    }
}
