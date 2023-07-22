using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public Transform tfBox;
    public Transform tfCharacter;
    private CharacterViewItem curCharacter;
    /// <summary>
    /// List of Box View
    /// </summary>
    private List<BoxViewItem> listBox = new List<BoxViewItem>();
    /// <summary>
    /// Dictionary of Box Position. Key is Pos ID such as (0,1)
    /// </summary>
    private Dictionary<Vector2Int, UnitViewItem> dicBlock = new Dictionary<Vector2Int, UnitViewItem>();

    public void Init()
    {
        CheckAllCharacter();
        CheckAllBoxPos();
        ScanAllPos();
    }

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("CharacterMove", CharacterMoveEvent);

    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("CharacterMove", CharacterMoveEvent);

    }

    private void CharacterMoveEvent(object arg0)
    {
        Vector2Int dir = (Vector2Int)arg0;
        if (curCharacter != null)
        {
            Vector2Int targetPosCharacter = curCharacter.posID + dir;
            if (dicBlock.ContainsKey(targetPosCharacter))
            {
                //Check whether this block is box


                BoxViewItem box = (BoxViewItem)dicBlock[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;

                if (!dicBlock.ContainsKey(targetPosBox))
                {
                    curCharacter.Move(dir);
                    box.Move(dir);
                }
            }
            else
            {
                curCharacter.Move(dir);
            }
            ScanAllPos();
        }


    }

    #endregion

    #region InitCheck

    public void CheckAllCharacter()
    {
        foreach (Transform child in tfCharacter)
        {
            curCharacter = child.GetComponent<CharacterViewItem>();
            curCharacter.Init();
        }
    }

    public void CheckAllBoxPos()
    {
        listBox.Clear();
        foreach (Transform child in tfBox)
        {
            BoxViewItem itemBox = child.GetComponent<BoxViewItem>();
            itemBox.Init();
            listBox.Add(itemBox);
        }
    }

    #endregion

    #region Scan

    public void ScanAllPos()
    {
        dicBlock.Clear();
        foreach(var box in listBox)
        {
            dicBlock.Add(box.posID, box);
        }
    }

    #endregion
}
