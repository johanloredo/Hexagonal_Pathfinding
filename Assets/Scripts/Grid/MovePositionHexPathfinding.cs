using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovePositionHexPathfinding : MonoBehaviour, IMovePosition
{
    [SerializeField] private float reachedPositionDistance = 1f;


    private List<Vector3> pathVectorList;
    private int pathIndex = -1;
    private Action onReachedMovePosition;

    public void SetMovePosition(Vector3 movePosition, Action onReachedMovePosition)
    {
        this.onReachedMovePosition = onReachedMovePosition;
        pathVectorList = HexPathfinding.Instance.FindPath(transform.position, movePosition);
        if (pathVectorList.Count > 0)
        {
            // Remove first position so he doesn't go backwards
            pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0)
        {
            pathIndex = 0;
        }
        else
        {
            pathIndex = -1;
            onReachedMovePosition();
        }
    }

    private void Update()
    {
        if (pathIndex != -1)
        {
            // Move to next path position
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPositionDistance)
            {
                pathIndex++;
                if (pathIndex >= pathVectorList.Count)
                {
                    // End of path
                    pathIndex = -1;
                    onReachedMovePosition();
                }
            }
        }
        else
        {
            // Idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }
}
