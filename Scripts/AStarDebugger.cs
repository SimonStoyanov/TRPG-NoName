using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    private static AStarDebugger instance;

    public static AStarDebugger myInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AStarDebugger>();
            }

            return instance;
        }
    }

    private AStar pathfind;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private List<Tilemap> tm;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private Color pathColor, startColor, goalColor;

    private Vector3Int startPos, goalPos;

    [SerializeField]
    private GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();

    private void Awake()
    {
        pathfind = FindObjectOfType<AStar>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int posInt = grid.WorldToCell(pos);
            posInt.z = 0;

            if (tm[0].HasTile(posInt))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 playerPos = player.transform.position;

                startPos = grid.WorldToCell(playerPos);
                goalPos = posInt;

                if (startPos != goalPos)
                {
                    pathfind.SetPathPositions(startPos, goalPos);
                    pathfind.RunAlgorithm(startPos, goalPos);
                    pathfind.MoveToGoal(player);
                }
            }
        }
    }

    public void CreateTiles(Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        tm[2].ClearAllTiles();
        tm[2].ClearAllEditorPreviewTiles();
        tm[2].RefreshAllTiles();

        if (path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);
    }

    public void ColorTile(Vector3Int pos, Color color)
    {
        tm[2].SetTile(pos, tile);
        tm[2].SetTileFlags(pos, TileFlags.None);
        tm[2].SetColor(pos, color);
    }
}