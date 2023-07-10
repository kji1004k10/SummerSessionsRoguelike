using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfEarth : Item
{


    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private static readonly float PassiveTick = 5f;
    private static readonly float ActiveMag = 5f;
    private static readonly float SkillWidth = 7f;
    private float _dT;

    public EssenceOfEarth()
    : base(ItemType.Essence, "������ ����",
        string.Format(
            "��� �� : �ֺ� ������ <color=brown>���</color>�ϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
            "�⺻ ���� ȿ�� : {1}�ʸ��� �ֺ� ���� <color=brown>����</color>��ŵ�ϴ�.", Cooldown, PassiveTick),
        Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(SkillWidth * 2, 2f), 0f);
        foreach (var enemy in area)
        {
            if(enemy.GetComponent<Entity>() is Monster)
            {
                //������ �Ÿ��� ���� ����� �޶���
                enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (ActiveMag + (SkillWidth - Vector2.Distance(enemy.transform.position, Player.Instance.transform.position))), ForceMode2D.Impulse);
            }
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            _dT = 0;
        }
    }
}
