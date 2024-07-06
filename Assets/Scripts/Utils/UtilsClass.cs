using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUtilities
{
    public class UtilsClass
    {
        public const int sortingOrderDefault = 1000;

        public static TextMesh SpawnText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 50, Color? color = null, TextAnchor anchor = TextAnchor.UpperLeft, TextAlignment alignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.black;
            return SpawnText(text, parent, localPosition, fontSize, (Color)color, anchor, alignment, sortingOrder);
        }

        public static TextMesh SpawnText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor anchor, TextAlignment alignment, int sortingOrder)
        {
            GameObject newText = new GameObject("World_Text", typeof(TextMesh));
            newText.transform.SetParent(parent, false);
            newText.transform.localPosition = localPosition;

            TextMesh textMesh = newText.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.anchor = anchor;
            textMesh.alignment = alignment;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }


        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition - new Vector3(0, 0, Camera.main.transform.position.z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;

            return worldPosition;
        }
    }
}
