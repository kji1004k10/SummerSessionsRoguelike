using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryTest : Item
{

    public AccessoryTest() 
        : base(ItemType.Accessory, "�׽�Ʈ ��ű�", 
            string.Format(
                "�����縮 �׽�Ʈ"), 
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_9"))
    {
    }

    public override void OnActiveUse()
    {
    }

    public override void OnMount()
    {
        throw new NotImplementedException();
    }

    public override void OnUnmount()
    {
        throw new NotImplementedException();
    }

    public override void PassiveUpdate()
    {
    }
}
