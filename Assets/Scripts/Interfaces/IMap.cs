using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    public int Width { get; set; }

    public int Height { get; set; }

    public float CellSize { get; set; }

    public Vector3 Origin { get; set; }

    ICell[,] StoredMap { get; }

    ICell[,] CreateMap();

    ICell CreateCell(int x, int z);

    ICell GetCell(int x, int z);

    Vector3 GetWorldPosition(int x, int z);

    void GetGridPosition(Vector3 worldPosition, out int x, out int z);
}
