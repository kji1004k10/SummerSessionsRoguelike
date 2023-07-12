using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    public EssenceOfStorm()
: base(ItemType.Essence, "��ǳ�� ����",
    string.Format(
        "��� �� : �ֺ� ������ <color=brown>���</color>�ϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
        "�⺻ ���� ȿ�� : {1}�ʸ��� �ֺ� ���� <color=brown>����</color>��ŵ�ϴ�.", Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_0"))
    {
    }

    public override void OnActiveUse()
    {
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }

    public override void PassiveUpdate()
    {
    }
}
