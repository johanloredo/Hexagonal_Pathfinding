using System.Collections.Generic;

interface IPathFinder
{


    IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map);
}
