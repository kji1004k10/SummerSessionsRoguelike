using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfDarkness : Item
{
    private static readonly float Cooldown = 5f;

    private static readonly float SlowMag = 0.4f;
    private static readonly float MaintainTime = 3;
    private static readonly float Radius = 6;
    private static readonly float CastTime = 0.2f;

    private List<GameObject> _skillList = new List<GameObject>();
    private List<float> _speedList = new List<float>();

    public EssenceOfDarkness()
        : base(ItemType.Essence, "������ ����",
            string.Format(
                "��� �� : �ֺ��� �������� �ϴ� ������ �����մϴ�. <color=#aaa>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
                "�⺻ ���� ȿ�� : - ", Cooldown),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_6"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Player.Instance.StartCoroutine(SkillCor(MaintainTime, Radius, CastTime));
    }

    public override void PassiveUpdate()
    {
    }

    
    private IEnumerator SkillCor(float maintainTime, float radius, float castTime)
    {
        Sprite sprite = Resources.Load<Sprite>("Item/Icon/Area");
        GameObject effect = new GameObject();
        effect.AddComponent<SpriteRenderer>().sprite = sprite;
        effect.GetComponent<SpriteRenderer>().color = Color.black;
        effect.transform.localScale = Vector2.zero;
        effect.transform.position = Player.Instance.transform.position;

        //���� �ڵ�
        float dT = 0;
        while (dT < castTime)
        {
            effect.transform.localScale = Vector2.one * (radius * Player.Instance.Stat.Get(StatType.EssenceForce)) * dT * (1f / castTime);
            yield return null;
            dT += Time.deltaTime;
        }
        //

        _skillList.Clear();
        _speedList.Clear();

        //���� �� ����ü ������
        effect.transform.localScale = Vector2.one * radius * Player.Instance.Stat.Get(StatType.EssenceForce);
        dT = 0;

        while (dT < maintainTime)
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(effect.transform.position, radius * Player.Instance.Stat.Get(StatType.EssenceForce));
            foreach (var o in objects)
            {
                if (o?.GetComponent<Projectile>() is Projectile)
                {
                    if(!_skillList.Contains(o.gameObject))
                    {
                        _skillList.Add(o.gameObject);
                        _speedList.Add(o.GetComponent<Projectile>().Speed);
                        o.GetComponent<Projectile>().Speed *= SlowMag;
                    }
                }
                else if(o?.GetComponent<Entity>() is Monster)
                {
                    o.GetComponent<Monster>().Stat.Multiply(StatType.MoveSpeed, SlowMag);
                }
            }
            yield return null;
            dT += Time.deltaTime;
        }
        //

        //���� ����
        dT = castTime;

        while (dT > 0)
        {
            effect.transform.localScale = Vector2.one * (radius * Player.Instance.Stat.Get(StatType.EssenceForce)) * dT * (1f / castTime);
            yield return null;
            dT -= Time.deltaTime;
        }

        //�����·� ����
        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i] != null)
            {
                if (_skillList[i].GetComponent<Projectile>() is Projectile)
                    _skillList[i].GetComponent<Projectile>().Speed = _speedList[i];
            }
        }

        Object.Destroy(effect);
        //
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
