using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivingUpSkillsViewItem : TileViewItem
{
    // Start is called before the first frame update

    public void OpenGiveupSkillsPage()
    {

        EventCenter.Instance.EventTrigger("PartEnd", 1);


    }
}
