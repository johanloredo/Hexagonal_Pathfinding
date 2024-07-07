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
    private LayerMask mouseInputLayerMask;


    private IList<ICell> path;
    private ICell cellStart;
    private ICell cellEnd;

    private Vector3 previousHitPoint;

    private void Start()
    {
        map.CreateMap();

        cellStart = cellEnd = map.GetCell(0, 0);

        //GetPath(map.GetCell(0, 0), map.GetCell(map.Width - 1, map.Height - 1));
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    map.GetGridPosition(GetMouseWorldPosition(), out int x, out int y);
        //    ICell targetCell = map.GetCell(x, y);

        //    if (targetCell != null)
        //        GetPath(cellEnd, targetCell);
        //}
        if (Input.GetMouseButtonDown(0))
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

    private void GetPath(ICell newCellStart, ICell newCellEnd)
    {
        cellStart = newCellStart;
        cellEnd = newCellEnd;

        path = pathFinder.FindPathOnMap(cellStart, cellEnd, map);
    }

    #region Public Methods
    public void SetCellStart(ICell cell)
    {
        cellStart = cell;
    }

    public void SetCellEnd(ICell cell)
    {
        cellEnd = cell;
    }

    public void SetObstacle(ICell cell)
    {
        cell.SetWalkable(!cell.IsWalkable);
    }
    #endregion
}
