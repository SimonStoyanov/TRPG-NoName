using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathMovement : MonoBehaviour
{
    GameObject objectToMove;
    Stack<Vector3Int> path;
    Tilemap tm;
    Vector3 nextPos;

    public void SetData(GameObject objectToMove, Stack<Vector3Int> path, Tilemap tm)
    {
        this.objectToMove = objectToMove;
        this.path = path;
        this.tm = tm;

        nextPos = tm.CellToWorld(path.Pop());
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (path.Count >= 0)
        {
            if (Vector3.Distance(objectToMove.GetComponent<Rigidbody2D>().position, nextPos) > 0.01f)
            {
                objectToMove.GetComponent<Rigidbody2D>().position = Vector3.MoveTowards(objectToMove.GetComponent<Rigidbody2D>().position, nextPos, Time.deltaTime * 2f);
            }
            else
            {
                if (path.Count == 0)
                {
                    Destroy(objectToMove.GetComponent<PathMovement>());
                }
                else
                {
                    nextPos = tm.CellToWorld(path.Pop());
                }
            }
        }

    }
}
