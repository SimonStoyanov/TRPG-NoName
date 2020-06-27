using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarNode : Tile
{
    public int g { get; set; }
    public int h { get; set; }
    public int f { get; set; }

    public Vector3Int Position { get; set; }
    public AStarNode Parent { get; set; }

    public AStarNode(Vector3Int position)
    {
        Position = position;
    }
}

public enum TileType { start, goal, block, path }



public class AStar : MonoBehaviour
{
    private TileType tileType;

    [SerializeField]
    private List<Tilemap> tm;

    private Vector3Int startPos, goalPos;
    private AStarNode current;
    private HashSet<AStarNode> openList, closedList;
    private Dictionary<Vector3Int, AStarNode> nodes = new Dictionary<Vector3Int, AStarNode>();
    private Stack<Vector3Int> path;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Stack<Vector3Int> RunAlgorithm(Vector3Int startPos, Vector3Int goalPos)
    {
        if (tm[0].HasTile(goalPos))
        {
            Reset();

            if (current == null)
            {
                Initialize();
            }

            while (openList.Count > 0 && path == null)
            {
                List<AStarNode> neighbors = FindNeighbors(current.Position);

                ExamineNeighbors(neighbors, current);

                UpdateCurrentTile(ref current);

                path = GeneratePath(current);
            }

            AStarDebugger.myInstance.CreateTiles(startPos, goalPos, path);
        }

        return path;
    }

    public void MoveToGoal(GameObject go)
    {
        PathMovement movement = go.AddComponent<PathMovement>();
        movement.SetData(go, path, tm[0]);
    }

    private void Initialize()
    {
        current = GetNode(startPos);

        openList = new HashSet<AStarNode>();
        closedList = new HashSet<AStarNode>();

        openList.Add(current);
    }

    public void Reset()
    {
        if (current != null)
        {
            foreach (Vector3Int pos in nodes.Keys)
            {
                tm[2].SetTileFlags(pos, TileFlags.None);
                tm[2].SetTile(pos, null);
            }

            foreach (AStarNode pos in closedList)
            {
                tm[2].SetTileFlags(pos.Position, TileFlags.None);
                tm[2].SetTile(pos.Position, null);
            }

            tm[2].RefreshAllTiles();
            
            openList.Clear();
            closedList.Clear();
            nodes.Clear();
            path.Clear();

            path = null;
            current = null;
        }
    }

    private List<AStarNode> FindNeighbors(Vector3Int parentPos)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPos.x - i, parentPos.y - j, parentPos.z);

                if (j != 0 || i != 0)
                {
                    if (neighborPos != startPos && tm[0].HasTile(neighborPos))
                    {
                        AStarNode neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }        
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<AStarNode> neighbors, AStarNode current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            AStarNode neighbor = neighbors[i];
            int gScore = Manhattan(neighbors[i].Position, current.Position);

            if (openList.Contains(neighbor))
            {
                if (current.g + gScore < neighbor.g)
                {
                    CalculateValues(current, neighbor, gScore);
                }
            }
            else if (!closedList.Contains(neighbor))
            {
                CalculateValues(current, neighbor, gScore);

                openList.Add(neighbor);
            }
        }
    }

    private void CalculateValues(AStarNode parent, AStarNode  neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.g = parent.g + cost;
        neighbor.h = (Mathf.Abs(neighbor.Position.x - goalPos.x) + Mathf.Abs(neighbor.Position.y - goalPos.y)) * 10;
        neighbor.f = neighbor.g + neighbor.h;
    }

    int Manhattan(Vector3Int pointA, Vector3Int pointB)
    {
        int gScore = 0;
        float manhattan = Mathf.Abs(pointA.x - pointB.x) + Mathf.Abs(pointB.x - pointB.y);

        if (manhattan % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref AStarNode current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.f).First();
        }
    }

    private AStarNode GetNode(Vector3Int position)
    {
        if (nodes.ContainsKey(position))
        {
            return nodes[position];
        }

        AStarNode node = new AStarNode(position);
        nodes.Add(position, node);
        return node;
    }

    public void SetPathPositions(Vector3Int start, Vector3Int goal)
    {
        startPos = start;
        goalPos = goal;
    }

    private Stack<Vector3Int> GeneratePath(AStarNode current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while (current.Position != startPos)
            {
                finalPath.Push(current.Position);

                current = current.Parent;
            }

            return finalPath;
        }

        return null;
    }
}
