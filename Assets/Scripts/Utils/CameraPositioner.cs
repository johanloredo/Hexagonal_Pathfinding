using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    [SerializeField]
    float buffer;

    private void Update()
    {
        Bounds bounds = new Bounds();

        bounds.Encapsulate(Map.Instance.GetCell(0, 0).Position);
        bounds.Encapsulate(Map.Instance.GetCell(Map.Instance.Width - 1, Map.Instance.Height - 1).Position);

        bounds.Expand(buffer);

        float vertical = bounds.size.y;
        float horizontal = bounds.size.x * Camera.main.pixelHeight / Camera.main.pixelWidth;
        Camera.main.orthographicSize = Mathf.Max(horizontal, vertical) * 0.5f;
        Camera.main.transform.position = bounds.center + new Vector3(0, 20, 0);
    }
}
