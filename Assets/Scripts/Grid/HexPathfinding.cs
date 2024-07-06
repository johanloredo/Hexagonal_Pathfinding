using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class HexPathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static HexPathfinding Instance { get; private set; }

        private HexGrid<PathNode> grid;
        private List<PathNode> openList;
        private List<PathNode> closedList;

        public HexPathfinding(int width, int height, float cellSize)
        {
            Instance = this;
            grid = new HexGrid<PathNode>(width, height, cellSize, Vector3.zero, (HexGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        }

        public HexGrid<PathNode> GetGrid()
        {
            return grid;
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            grid.GetXZ(startWorldPosition, out int startX, out int startY);
            grid.GetXZ(endWorldPosition, out int endX, out int endY);

            List<PathNode> path = FindPath(startX, startY, endX, endY);
            if (path == null)
            {
                return null;
            }
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (PathNode pathNode in path)
                {
                    vectorPath.Add(grid.GetWorldPosition(pathNode.x, pathNode.y));
                    //vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
                }
                return vectorPath;
            }
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);

            if (startNode == null || endNode == null)
            {
                // Invalid Path
                return null;
            }

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = 99999999;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    // Reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // Out of nodes on the openList
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            bool oddRow = currentNode.y % 2 == 1;

            if (currentNode.x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            }
            if (currentNode.x + 1 < grid.GetWidth())
            {
                // Right
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            }
            if (currentNode.y - 1 >= 0)
            {
                // Down
                neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            }
            if (currentNode.y + 1 < grid.GetHeight())
            {
                // Up
                neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
            }

            if (oddRow)
            {
                if (currentNode.y + 1 < grid.GetHeight() && currentNode.x + 1 < grid.GetWidth())
                {
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                }
                if (currentNode.y - 1 >= 0 && currentNode.x + 1 < grid.GetWidth())
                {
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                }
            }
            else
            {
                if (currentNode.y + 1 < grid.GetHeight() && currentNode.x - 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                }
                if (currentNode.y - 1 >= 0 && currentNode.x - 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                }
            }


            return neighbourList;
        }

        public PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_STRAIGHT_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }
    }
}
