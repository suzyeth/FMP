using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PublicTool
{
    public static void ClearChildItem(UnityEngine.Transform tf)
    {
        foreach (UnityEngine.Transform item in tf)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }

    public static Vector2 ConvertPosFromID(Vector2Int posID)
    {
        return new Vector2(posID.x, posID.y);
    }
}
