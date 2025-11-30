using System.Collections.Generic;
using UnityEngine;

public class GridOccupancyBehavior : MonoBehaviour
{

    [Header("Settings")]
    public float cellSize = 1.0f;

    public Dictionary<Vector2Int, OccupancyState> occupancyMap = new Dictionary<Vector2Int, OccupancyState>();

    public void SetOccupancyList(List<GameObject> gameObjects, OccupancyState state)
    {
        foreach (GameObject obj in gameObjects)
        {
            SetOccupancy(obj, state);
        }
    }

    public void SetOccupancy(GameObject gameObject, OccupancyState state)
    {
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        List<Vector2Int> occupiedCells = GetOccupiedCells(bounds);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            occupancyMap[occupiedCells[i]] = state;
        }
    }

    public void SetOccupancy(List<Vector2Int> cellList, OccupancyState state)
    {
        foreach (Vector2Int cell in cellList)
        {
            occupancyMap[cell] = state;
        }
    }

    public bool IsOccupied(GameObject gameObject)
    {
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        List<Vector2Int> occupiedCells = GetOccupiedCells(bounds);

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            if (occupancyMap.TryGetValue(occupiedCells[i], out OccupancyState state))
            {
                if (state == OccupancyState.Occupied)
                {
                    return true;
                }
            }
        }


        return false;
    }

    public bool IsCellOccupied(Vector2Int cell)
    {
        if (occupancyMap.TryGetValue(cell, out OccupancyState state))
        {
            if (state == OccupancyState.Occupied)
            {
                return true;
            }
        }


        return false;
    }

    public bool doCellsOverlap(List<Vector2Int> listA, List<Vector2Int> listB)
    {
        foreach (Vector2Int cell in listA)
        {
            if (listB.Contains(cell))
            {
                return true;
            }
        }
        return false;
    }

    public List<Vector2Int> GetOccupiedCells(Bounds bounds)
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        int minX = Mathf.FloorToInt(min.x / cellSize);
        int maxX = Mathf.CeilToInt(max.x / cellSize) - 1;
        int minY = Mathf.FloorToInt(min.z / cellSize);
        int maxY = Mathf.CeilToInt(max.z / cellSize) - 1;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                cells.Add(new Vector2Int(x, y));
            }
        }

        return cells;
    }

    public void printOccupiedCells(Bounds bounds)
    {
        List<Vector2Int> occupiedCells = GetOccupiedCells(bounds);
        foreach (var cell in occupiedCells)
        {
            Debug.Log($"Occupied Cell: {cell}");
        }
    }

    public void visualizeOccupiedCells()
    {
        // for 50 seconds
        foreach (var cell in occupancyMap)
        {
            Vector3 cellCenter = new Vector3((cell.Key.x + 0.5f) * cellSize, 0.1f, (cell.Key.y + 0.5f) * cellSize);
            Color color = Color.green;
            if (cell.Value == OccupancyState.Occupied)
            {
                color = Color.red;
            }
            else if (cell.Value == OccupancyState.Reserved)
            {
                color = Color.yellow;
            }
            // all four sides of the cell
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, -cellSize / 2), color, 10f);
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, -cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, cellSize / 2), color, 10f);
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(-cellSize / 2, 0, cellSize / 2), color, 10f);
            Debug.DrawLine(cellCenter + new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, -cellSize / 2), color, 10f);
        }
    }
    
    public void visualizeCells(List<Vector2Int> cellList)
    {
        // for 50 seconds
        foreach (Vector2Int cell in cellList)
        {
            Vector3 cellCenter = new Vector3((cell.x + 0.5f) * cellSize, 0.1f, (cell.y + 0.5f) * cellSize);
            Color color = Color.green;

            // all four sides of the cell
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, -cellSize / 2), color, 50000f);
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, -cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, cellSize / 2), color, 50000f);
            Debug.DrawLine(cellCenter - new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(-cellSize / 2, 0, cellSize / 2), color, 50000f);
            Debug.DrawLine(cellCenter + new Vector3(cellSize / 2, 0, cellSize / 2), cellCenter + new Vector3(cellSize / 2, 0, -cellSize / 2), color, 50000f);
        }
    }
}

public enum OccupancyState
    {
        Free,
        Occupied,
        Reserved
    }