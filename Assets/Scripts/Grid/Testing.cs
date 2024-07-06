using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexUtilities;

public class Testing : MonoBehaviour
{
    //private Grid grid;

    //private void Start()
    //{
    //    grid = new Grid(20, 10, 10f, Vector3.zero);
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
    //    }
    //}

    [SerializeField] private Transform pfHex;

    private HexGrid<GridObject> gridHexXZ;
    private GridObject lastGridObject;

    private class GridObject
    {
        public Transform visualTransform;

        public void Show()
        {
            visualTransform.Find("Highlight").gameObject.SetActive(true);
        }

        public void Hide()
        {
            visualTransform.Find("Highlight").gameObject.SetActive(false);
        }

    }

    private void Awake()
    {
        int width = 30;
        int height = 30;
        float cellSize = 1f;
        gridHexXZ =
            new HexGrid<GridObject>(width, height, cellSize, Vector3.zero, (HexGrid<GridObject> g, int x, int y) => new GridObject());

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                gridHexXZ.GetGridObject(x, z).visualTransform = visualTransform;
                gridHexXZ.GetGridObject(x, z).Hide();
            }
        }

    }

    private void Update()
    {
        if (lastGridObject != null)
        {
            lastGridObject.Hide();
        }

        lastGridObject = gridHexXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());

        if (lastGridObject != null)
        {
            lastGridObject.Show();
        }

    }
}
