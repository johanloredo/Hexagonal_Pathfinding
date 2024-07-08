using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>, IMap
{
    private const float HEX_VERTICAL_OFFSET = 0.75f;

    [SerializeField]
    private Cell cellPrefab;

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private float cellSize;
    [SerializeField]
    private Vector3 origin;

    private ICell[,] storedMap;

    #region Interface
    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public float CellSize
    {
        get { return cellSize; }
        set { cellSize = value; }
    }

    public Vector3 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public ICell[,] StoredMap
    {
        get { return storedMap; }
    }

    public ICell[,] CreateMap()
    {
        storedMap = new ICell[width, height];

        for (int x = 0; x < storedMap.GetLength(0); x++)
        {
            for (int z = 0; z < storedMap.GetLength(1); z++)
            {
                storedMap[x, z] = CreateCell(x, z);
            }
        }

        return storedMap;
    }

    public ICell CreateCell(int x, int z)
    {
        ICell newCell = Instantiate(cellPrefab, GetWorldPosition(x, z), Quaternion.identity);
        newCell.IndexX = x;
        newCell.IndexY = z;

        newCell.SetWalkable(Random.Range(1, 10) % 3 != 0);
        newCell.Highlight(false);
        return newCell;
    }

    public ICell GetCell(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height) return storedMap[x, z];
        else return null;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return
            new Vector3(x, 0, 0) * cellSize +
            new Vector3(0, 0, z) * cellSize * HEX_VERTICAL_OFFSET +
            ((Mathf.Abs(z) % 2) == 1 ? new Vector3(1, 0, 0) * cellSize * 0.5f : Vector3.zero) +
            origin;
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        int roughX = Mathf.RoundToInt((worldPosition - origin).x / cellSize);
        int roughZ = Mathf.RoundToInt((worldPosition - origin).z / cellSize / HEX_VERTICAL_OFFSET);

        Vector3Int roughXZ = new Vector3Int(roughX, 0, roughZ);

        bool oddRow = roughZ % 2 == 1;

        List<Vector3Int> neighbourXZList = new List<Vector3Int> {
             roughXZ + new Vector3Int(-1, 0, 0),
             roughXZ + new Vector3Int(+1, 0, 0),

             roughXZ + new Vector3Int(oddRow ? +1 : -1, 0, +1),
             roughXZ + new Vector3Int(+0, 0, +1),

             roughXZ + new Vector3Int(oddRow ? +1 : -1, 0, -1),
             roughXZ + new Vector3Int(+0, 0, -1),
        };

        Vector3Int closestXZ = roughXZ;

        foreach (Vector3Int neighbourXZ in neighbourXZList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourXZ.x, neighbourXZ.z)) <
                Vector3.Distance(worldPosition, GetWorldPosition(closestXZ.x, closestXZ.z)))
            {
                // Closer than closest
                closestXZ = neighbourXZ;
            }

        }

        x = closestXZ.x;
        z = closestXZ.z;
    }
    #endregion
}
