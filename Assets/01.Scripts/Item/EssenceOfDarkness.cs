using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfDarkness : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    private static readonly float SlowMag = 0.4f;
    private static readonly float MaintainTime = 5;
    private static readonly float Radius = 3;
    private static readonly float CastTime = 0.2f;

    private List<GameObject> _skillList = new List<GameObject>();
    private List<float> _speedList = new List<float>();

    public EssenceOfDarkness()
        : base(ItemType.Essence, "������ ����",
            string.Format(
                "��� �� : �ֺ��� �������� �ϴ� ������ �����մϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
                "�⺻ ���� ȿ�� : 1�ʴ� HP�� <color=green>2</color> ȸ���մϴ�.", Cooldown),
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

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
        effect.transform.localScale = Vector2.zero;
        effect.transform.position = Player.Instance.transform.position;

        //���� �ڵ�
        float dT = 0;
        while (dT < castTime)
        {
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT += Time.deltaTime;
        }
        //

        _skillList.Clear();
        _speedList.Clear();

        //���� �� ����ü ������
        effect.transform.localScale = Vector2.one * radius;
        dT = 0;

        while (dT < maintainTime)
        {
            Collider2D[] bullets = Physics2D.OverlapCircleAll(Player.Instance.transform.position, radius);
            foreach (var b in bullets)
            {
                if (b.name.Contains("Bullet"))
                {
                    if(!_skillList.Contains(b.gameObject))
                    {
                        Debug.LogError(b.GetComponent<Bullet>().Speed *= SlowMag);
                        _skillList.Add(b.gameObject);
                        _speedList.Add(b.GetComponent<Bullet>().Speed);
                        b.GetComponent<Bullet>().Speed *= SlowMag;
                    }
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
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT -= Time.deltaTime;
        }

        //�����·� ����
        for (int i = 0; i < _speedList.Count; i++)
        {
            if (_skillList[i] != null)
            {
                Debug.LogError(_speedList[i] / SlowMag);
                _skillList[i].GetComponent<Bullet>().Speed = _speedList[i] / SlowMag;
            }
        }

        Object.Destroy(effect);
        //
    }
}