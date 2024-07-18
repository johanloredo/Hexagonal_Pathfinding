using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, ICell
{
    private int indexX;
    private int indexY;

    private int gCost;
    private int hCost;
    private int fCost;

    private bool isWalkable;
    private ICell cameFromNode;

    private Color standby = new Color(0.33f, 0.43f, 0.9f);
    private Color highlighted = new Color(0.96f, 0.84f, 0.58f);

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

    public void Highlight(bool turnOn)
    {
        transform.GetChild(0).gameObject.SetActive(!turnOn);
        transform.GetChild(1).gameObject.SetActive(turnOn);

        if (turnOn)
        {
            SFXController.Instance.PlayHexSpawn();
        }
    }

    //public void SetHighlightColor(Color color)
    //{
    //    transform.GetChild(1).GetComponent<Image>().color = color;
    //}
    #endregion

    //private void Start()
    //{
    //    transform.GetChild(0).GetComponent<Renderer>().material.color = standby;
    //    transform.GetChild(1).GetComponent<Renderer>().material.color = highlighted;
    //}
}
