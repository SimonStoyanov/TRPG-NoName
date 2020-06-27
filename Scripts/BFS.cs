using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BFSNode : Tile
{
    public Vector3Int Position { get; set; }
    public AStarNode Parent { get; set; }

    public BFSNode(Vector3Int position)
    {
        Position = position;
    }
}

public class BFS : MonoBehaviour
{
    [SerializeField]
    private Tile tile;

    private Tilemap tmGround, tmMovement, tmEntity, tmBlock;

    [SerializeField]
    private Color walkableColor, nonWalkableColor, entityColor;

    private HashSet<BFSNode> nodes;

    // Start is called before the first frame update
    void Start()
    {
        tmGround = GameObject.FindGameObjectWithTag("GroundTM").GetComponent<Tilemap>();
        tmMovement = GameObject.FindGameObjectWithTag("MovementTM").GetComponent<Tilemap>();
        tmEntity = GameObject.FindGameObjectWithTag("EntityTM").GetComponent<Tilemap>();
        tmBlock = GameObject.FindGameObjectWithTag("BlockTM").GetComponent<Tilemap>();

        nodes = new HashSet<BFSNode>();


        CreateTiles();
    }

    public void RunAlgorithm(Vector3Int startPos, int range)
    {
        if (nodes.Count == 0)
        {
            
        }
    }

    private List<BFSNode> FindNeighbors(Vector3Int parentPos)
    {
        List<BFSNode> neighbors = new List<BFSNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPos.x - i, parentPos.y - j, parentPos.z);

                if (j != 0 || i != 0)
                {
                    if (neighborPos != startPos && tm[0].HasTile(neighborPos))
                    {
                        BFSNode neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<BFSNode> neighbors, BFSNode current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            BFSNode neighbor = neighbors[i];
            
            if (!nodes.Contains(neighbor))
            {
                nodes.Add()
            }
        }
    }

    public void CreateTiles()
    {
        tmMovement.ClearAllTiles();
        tmMovement.ClearAllEditorPreviewTiles();
        tmMovement.RefreshAllTiles();

        BoundsInt walkableBounds = tmGround.cellBounds;
        BoundsInt blockBounds = tmGround.cellBounds;
        BoundsInt entityBounds = tmGround.cellBounds;

        foreach (Vector3Int position in walkableBounds.allPositionsWithin)
        {
            if (tmGround.HasTile(position) && !tmBlock.HasTile(position))
            {
                ColorTile(position, walkableColor);
            }
        }

        foreach (Vector3Int position in blockBounds.allPositionsWithin)
        {
            if (tmBlock.HasTile(position))
            {
                ColorTile(position, nonWalkableColor);
            }
        }

        foreach (Vector3Int position in entityBounds.allPositionsWithin)
        {
            if (tmEntity.HasTile(position))
            {
                ColorTile(position, entityColor);
            }
        }
    }

    public void ColorTile(Vector3Int pos, Color color)
    {
        tmMovement.SetTile(pos, tile);
        tmMovement.SetTileFlags(pos, TileFlags.None);
        tmMovement.SetColor(pos, color);
    }
}
