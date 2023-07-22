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
        return new Vector2(posID.x * GameGlobal.tileSizeX, posID.y * GameGlobal.tileSizeY);
    }

    public static Vector2Int TranslatePosToPosID(Vector2 pos)
    {
        float tempPosX = (pos.x / GameGlobal.tileSizeX);
        float tempPosY = (pos.y / GameGlobal.tileSizeY);

        Vector2Int posID = new Vector2Int(Mathf.RoundToInt(tempPosX), Mathf.RoundToInt(tempPosY));

        return posID;
    }
}
