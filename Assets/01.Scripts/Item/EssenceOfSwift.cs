using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfSwift : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    public EssenceOfSwift()
: base(ItemType.Essence, "������ ����",
    string.Format(
        "��� �� : �ֺ� ����ü�� <color=darkviolet>�ı�</color>�ϴ� ������ �����մϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n", Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_3"))
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
