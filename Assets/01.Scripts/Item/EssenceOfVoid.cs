using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfVoid : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    public float MaintainTime;
    public float Radius;
    public float CastTime;

    public EssenceOfVoid()
    : base(ItemType.Essence, "������ ����",
        string.Format(
            "��� �� : �ֺ� ����ü�� <>�ı��ϴ� ������ �����մϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n", Cooldown),
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
        //Player.Instance.Heal(Time.deltaTime * 2);
    }

    private IEnumerator SkillCor(float maintainTime, float radius, float castTime)
    {
        Sprite sprite = Resources.Load<Sprite>("Item/Icon/Area");
        GameObject effect = new GameObject();
        effect.AddComponent<SpriteRenderer>().sprite = sprite;
        effect.transform.localScale = Vector2.zero;
        effect.transform.position = Player.Instance.transform.position;

        float dT = 0;
        while(dT < castTime)
        {
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT += Time.deltaTime;
        }

        effect.transform.localScale = Vector2.one * radius;
        dT = 0;

        while (dT < maintainTime)
        {
            Collider2D[] bullets = Physics2D.OverlapCircleAll(Player.Instance.transform.position, radius);
            foreach (var b in bullets)
            {
                Debug.LogError(b.name.Contains("Bullet"));
                if (b.name.Contains("Bullet"))
                    Object.Destroy(b.gameObject);
            }
            yield return null;
            dT += Time.deltaTime;
        }

        dT = castTime;

        while (dT > 0)
        {
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT -= Time.deltaTime;
        }

        Object.Destroy(effect);
    }

    
}
