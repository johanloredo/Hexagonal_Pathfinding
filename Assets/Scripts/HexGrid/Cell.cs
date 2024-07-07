using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    private int indexX;
    private int indexY;

    private int gCost;
    private int hCost;
    private int fCost;

    private bool isWalkable;
    private ICell cameFromNode;

    [SerializeField]
    private Transform obstaclePrefab;

    #region Interface
    public int IndexX
    {
        get { return indexX; }
        set { indexX = value; }
    }
    public int IndexY
    {
        get { return indexY; }
        set { indexY = value; }
    }

    public int GCost
    {
        get { return gCost; }
        set { gCost = value; }
    }
    public int HCost
    {
        get { return hCost; }
        set { hCost = value; }
    }
    public int FCost
    {
        get { return fCost; }
        set { fCost = value; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
    }

    public bool IsWalkable
    {
        get { return isWalkable; }
    }

    public void SetWalkable(bool walkable)
    {
        isWalkable = walkable;
        transform.GetChild(2).gameObject.SetActive(!walkable);
    }

    public ICell CameFromCell
    {
        get { return cameFromNode; }
        set { cameFromNode = value; }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void HighLight(bool turnOn)
    {
        transform.GetChild(1).gameObject.SetActive(turnOn);
    }
    #endregion
}
