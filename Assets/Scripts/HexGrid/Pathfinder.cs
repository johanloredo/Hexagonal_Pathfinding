using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Pathfinder : Singleton<Pathfinder>, IPathFinder
{
    private Map map;

    private List<ICell> openList;
    private List<ICell> closedList;
    private List<ICell> highlightedPath = new List<ICell>();

    private Coroutine highlightingRoutine;

    private void Start()
    {
        map = Map.Instance;
    }

    #region Interface
    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        UnhighlightPath();

        openList = new List<ICell> { cellStart };
        closedList = new List<ICell>();

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                ICell cell = map.StoredMap[x, y];

                cell.GCost = 99999999;
                cell.CalculateFCost();
                cell.CameFromCell = null;
            }
        }

        cellStart.GCost = 0;
        cellStart.HCost = CalculateDistanceCost(cellStart, cellEnd);
        cellStart.CalculateFCost();

        while (openList.Count > 0)
        {
            ICell currentCell = GetLowestFCostCell(openList);

            if (currentCell == cellEnd)
            {
                // Path completed
                return CalculatePath(cellEnd);
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (ICell neighbourCell in GetNeighbourList(currentCell))
            {
                if (closedList.Contains(neighbourCell)) continue;

                if (!neighbourCell.IsWalkable)
                {
                    closedList.Add(neighbourCell);
                    continue;
                }

                int tentativeGCost = currentCell.GCost + CalculateDistanceCost(currentCell, neighbourCell);

                if (tentativeGCost < neighbourCell.GCost)
                {
                    neighbourCell.CameFromCell = currentCell;
                    neighbourCell.GCost = tentativeGCost;
                    neighbourCell.HCost = CalculateDistanceCost(neighbourCell, cellEnd);
                    neighbourCell.CalculateFCost();

                    if (!openList.Contains(neighbourCell)) openList.Add(neighbourCell);
                }
            }
        }

        return null;
    }
    #endregion

    private int CalculateDistanceCost(ICell a, ICell b)
    {
        int xDistance = Mathf.Abs(a.IndexX - b.IndexX);
        int yDistance = Mathf.Abs(a.IndexY - b.IndexY);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return Mathf.Min(xDistance, yDistance) + remaining;
    }

    private ICell GetLowestFCostCell(List<ICell> cellPathList)
    {
        ICell lowestFCostNode = cellPathList[0];

        for (int i = 1; i < cellPathList.Count; i++)
        {
            if (cellPathList[i].FCost < lowestFCostNode.FCost) lowestFCostNode = cellPathList[i];
        }

        return lowestFCostNode;
    }

    private List<ICell> GetNeighbourList(ICell currentCell)
    {
        List<ICell> neighbourList = new List<ICell>();

        bool oddRow = currentCell.IndexY % 2 == 1;

        if (currentCell.IndexX - 1 >= 0) 
            neighbourList.Add(GetCell(currentCell.IndexX - 1, currentCell.IndexY));
        if (currentCell.IndexX + 1 < map.Width)
            neighbourList.Add(GetCell(currentCell.IndexX + 1, currentCell.IndexY));

        if (currentCell.IndexY - 1 >= 0)
            neighbourList.Add(GetCell(currentCell.IndexX, currentCell.IndexY - 1));
        if (currentCell.IndexY + 1 < map.Height)
            neighbourList.Add(GetCell(currentCell.IndexX, currentCell.IndexY + 1));

        if (oddRow)
        {
            if (currentCell.IndexX + 1 < map.Width && currentCell.IndexY + 1 < map.Height)
                neighbourList.Add(GetCell(currentCell.IndexX + 1, currentCell.IndexY + 1));
            if (currentCell.IndexX + 1 < map.Width && currentCell.IndexY - 1 >= 0)
                neighbourList.Add(GetCell(currentCell.IndexX + 1, currentCell.IndexY - 1));
        }
        else
        {
            if (currentCell.IndexX - 1 >= 0 && currentCell.IndexY + 1 < map.Height)
                neighbourList.Add(GetCell(currentCell.IndexX - 1, currentCell.IndexY + 1));
            if (currentCell.IndexX - 1 >= 0 && currentCell.IndexY - 1 >= 0)
                neighbourList.Add(GetCell(currentCell.IndexX - 1, currentCell.IndexY - 1));
        }

        return neighbourList;
    }

    private ICell GetCell(int x, int y)
    {
        return map.GetCell(x, y);
    }

    private List<ICell> CalculatePath(ICell endCell)
    {
        List<ICell> path = new List<ICell>();
        path.Add(endCell);
        ICell currentCell = endCell;

        while (currentCell.CameFromCell != null)
        {
            path.Add(currentCell.CameFromCell);
            currentCell = currentCell.CameFromCell;
        }

        path.Reverse();

        AbortHighlight();
        highlightingRoutine = StartCoroutine(DelayHighlight(path));
        return path;
    }

    private IEnumerator DelayHighlight(List<ICell> path)
    {
        foreach (ICell cell in path)
        {
            cell.Highlight(true);
            highlightedPath.Add(cell);

            yield return new WaitForSeconds(0.075f);
        }
    }

    public void AbortHighlight()
    {
        if (highlightingRoutine != null) StopCoroutine(highlightingRoutine);
    }

    public void UnhighlightPath()
    {
        foreach (ICell cell in highlightedPath)
        {
            cell.Highlight(false);
        }

        highlightedPath.Clear();
    }

    public bool IsHighlighted()
    {
        return highlightedPath.Count > 0;
    }
}
