using UnityEngine;

public interface ICell
{
    int IndexX { get; set; }
    int IndexY { get; set; }

    int GCost { get; set; }
    int HCost { get; set; }
    int FCost { get; set; }

    Vector3 Position { get; }

    bool IsWalkable { get;/* set;*/ }

    void SetWalkable(bool walkable);

    ICell CameFromCell { get; set; }

    void CalculateFCost();

    void HighLight(bool turnOn);
}
