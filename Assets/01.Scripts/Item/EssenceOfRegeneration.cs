using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    private float _lastUsed = -1;
    private float _cooldown = 5f;

    public EssenceOfRegenerate(int count) 
        : base(ItemType.Essence, "����� ����", 
            "��� �� : HP�� 10 ȸ���մϴ�. (���� ��ñⰣ : 5��)\n" +
            "�⺻ ���� ȿ�� : 1�ʴ� HP�� 2 ȸ���մϴ�.", 
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"), count)
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < _cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Player.Instance.Heal(10);
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Heal(Time.deltaTime * 2);
    }
}
