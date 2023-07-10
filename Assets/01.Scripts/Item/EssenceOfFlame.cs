using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfFlame : Item
{
    public float ActiveRadius = 10;
    private static readonly float ActiveDamage = 5f;

    public float PassiveTick;
    public float PassiveRadius;
    private static readonly float PassiveDamage = 5f;
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private float _dT;

    public EssenceOfFlame()
        : base(ItemType.Essence, "ȭ���� ����",
            string.Format(
                "��� �� : �ֺ� ������ ����ü�� ���� <color=red>{0}</color>��ŭ ���ظ� ������ <color=red>{1}</color>��ŭ �������ظ� �����ϴ�.\n" +
                " <color=gray>(���� ��ñⰣ : {2:0.0}��)</color>\n" +
                "�⺻ ���� ȿ�� : �ֺ� ������ <color=red>{3}</color> ��ŭ �������ظ� �����ϴ�.", ActiveDamage, PassiveDamage, PassiveDamage, Cooldown),
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        //�� ���̾� �߰��ؾ���
        Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, PassiveRadius);
        
        if(enemies.Length > 0)
        {
            int minDist = 0;
            for (int i = 0; i < enemies.Length; i++) 
            {
                if (enemies[i].GetComponent<Entity>() is Monster && Vector2.Distance(Player.Instance.transform.position, enemies[i].transform.position) < minDist)
                {
                    minDist = i;
                }
            }

            //������ ����ü �߻� �ڵ�, ���� ������ ����� ������ �ڵ�
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, PassiveRadius);
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<Entity>() is Monster)
                {
                    enemy.GetComponent<Entity>().Damage(PassiveDamage);
                }
            }
            _dT = 0;
        }
    }

    public override void OnMount()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUnmount()
    {
        throw new System.NotImplementedException();
    }
}
