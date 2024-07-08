using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    #region Events
    public class SetCellArgs : EventArgs
    {
        public ICell TogglingCell;

        public SetCellArgs(ICell togglingCell)
        {
            TogglingCell = togglingCell;
        }
    }

    public event EventHandler<SetCellArgs> OnSettingCell;
    #endregion

    [SerializeField]
    private Pathfinder pathFinder;

    [SerializeField]
    private Map map;

    [SerializeField]
    private Transform startHighlighter;
    [SerializeField]
    private Transform endHighlighter;

    [SerializeField]
    private LayerMask mouseInputLayerMask;


    private IList<ICell> path;
    private ICell cellStart;
    private ICell cellEnd;

    private Vector3 previousHitPoint;

    private void Start()
    {
        map.CreateMap();

        cellStart = map.GetCell(0, 0);
        cellEnd = map.GetCell(map.Width - 1, map.Height - 1);

        UIController.Instance.SetEnd();
        OnSettingCell?.Invoke(this, new SetCellArgs(cellEnd));
        UIController.Instance.SetStart();
        OnSettingCell?.Invoke(this, new SetCellArgs(cellStart));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))// || Input.GetMouseButtonUp(0))
        //if (Input.GetMouseButton(0))
        {
            map.GetGridPosition(GetMouseWorldPosition(), out int x, out int y);
            ICell targetCell = map.GetCell(x, y);

            if (targetCell != null)
                OnSettingCell?.Invoke(this, new SetCellArgs(targetCell));
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseInputLayerMask))
        {
            previousHitPoint = hit.point;
            return hit.point;
        }
        else
        {
            return previousHitPoint;
        }
    }

    public IList<ICell> GetPath()
    {
        if (cellStart.IsWalkable && cellEnd.IsWalkable)
        {
            path = pathFinder.FindPathOnMap(cellStart, cellEnd, map);
            return path;
        }
        else
        {
            return null;
        }
    }

    #region Public Methods
    public void SetCellStart(ICell cell)
    {
        if (Pathfinder.Instance.IsHighlighted())
        {
            Pathfinder.Instance.UnhighlightPath();
            //cellEnd.Highlight(true);
        }

        //cellStart.Highlight(false);

        cellStart = cell;
        startHighlighter.position = cellStart.Position;
        //cellStart.Highlight(true);
    }

    public void SetCellEnd(ICell cell)
    {
        if (Pathfinder.Instance.IsHighlighted())
        {
            Pathfinder.Instance.UnhighlightPath();
            //cellStart.Highlight(true);
        }

        //cellEnd.Highlight(false);

        cellEnd = cell;
        endHighlighter.position = cellEnd.Position;
        //cellEnd.Highlight(true);
    }

    public void SetObstacle(ICell cell)
    {
        if (Pathfinder.Instance.IsHighlighted())
        {
            Pathfinder.Instance.UnhighlightPath();
        }

        cell.SetWalkable(!cell.IsWalkable);
    }
    #endregion
}
