using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfLightning : Item
{
    
    private List<Entity> _entities = new List<Entity>();

    private LineRenderer _line;
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private static readonly float ActiveRadius = 10f;
    private static readonly int ActiveCount = 5;
    private static readonly int ActiveDamage = 10;


    public EssenceOfLightning()
        : base(ItemType.Essence, "������ ����",
            string.Format(
                "��� �� : �ֺ� �� {0}���� <color=yellow>����</color>�� ���� {1}�� ������� �����ϴ�.. <color=gray>(���� ��ñⰣ : {2:0.0}��)</color>\n" +
                "�⺻ ���� ȿ�� : -", ActiveCount, ActiveDamage, Cooldown),
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        _entities.Clear();

        //���� ���� ã��
        Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, ActiveRadius, 1 << LayerMask.NameToLayer("Enemy"));
        int min = 0;
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    if (enemies[i].GetComponent<Entity>() is Monster)
        //    {
        //        min = i;
        //        break;
        //    }
        //}
        ////���ٸ� ����
        //if (min == -1)
        //    return;

        if(enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                //���� ����� �� ã��
                if (Vector2.Distance(Player.Instance.transform.position, enemies[i].transform.position) < Vector2.Distance(Player.Instance.transform.position, enemies[min].transform.position)
                    && enemies[i].GetComponent<Entity>() is Monster
                    && enemies[i].GetComponent<Entity>() is Player == false)
                {
                    min = i;
                }
            }
            if (enemies[min] != null)
            {
                _entities.Add(enemies[min].GetComponent<Entity>());
                Lightning(enemies[min].GetComponent<Entity>(), ActiveCount - 1);
            }
        }
    }

    public override void PassiveUpdate()
    {
    }

    public void Lightning(Entity entity, int cnt)
    {
        if(cnt <= 0)            //���ȣ�� �����(�ֺ� �� ���ٸ�)
        {
            if(_line == null)
            {
                _line = Player.Instance.gameObject.AddComponent<LineRenderer>();
                _line.material = Resources.Load<Material>("Item/LightningLine");
                _line.startColor = new Color(0, 255, 237);
                _line.endColor = new Color(0, 115, 255);
                _line.startWidth = .3f;
                _line.endWidth = .3f;
            }
            _line.positionCount = _entities.Count + 1;
            _line.SetPosition(0, Player.Instance.transform.position);
            for (int i = 0; i < _entities.Count; i++)
            {
                Debug.Log(_entities[i].name);
                _line.SetPosition(i + 1, _entities[i].transform.position);
                _entities[i].Damage(10f);
                Player.Instance.StartCoroutine(LightningAnim(1.5f));
            }
        }
        else        //�ֺ� �� Ž��, ��� ȣ��
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(entity.transform.position, ActiveRadius, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemies.Length > 0)
            {
                int min = 0;
                //for (int i = 0; i < enemies.Length; i++)
                //{
                //    if (enemies[i].GetComponent<Entity>() is Monster
                //        && _entities.Contains(enemies[i].GetComponent<Entity>()) == false)
                //    {
                //        if (enemies[i].GetComponent<Entity>() == entity)
                //            continue;
                //        min = i;
                //        break;
                //    }
                //}


                //if(min == -1)
                //    Lightning(entity, 0);

                for (int i = 0; i < enemies.Length; i++)
                {
                    //���� ����� �� ã��
                    if (Vector2.Distance(entity.transform.position, enemies[i].transform.position) < Vector2.Distance(entity.transform.position, enemies[min].transform.position)
                        && enemies[i].GetComponent<Entity>() is Monster
                        && enemies[i].GetComponent<Entity>() is Player == false
                        && _entities.Contains(enemies[i].GetComponent<Entity>()) == false)
                    {
                        if (enemies[i].GetComponent<Entity>() == entity)
                            continue;
                        min = i;
                    }
                }
                if (_entities.Contains(enemies[min].GetComponent<Entity>()))     //Ž���� ���� �� �̻� ���ٸ�
                {
                    Lightning(entity, 0);
                }
                else
                {
                    _entities.Add(enemies[min].GetComponent<Entity>());
                    Lightning(enemies[min].GetComponent<Entity>(), cnt - 1);
                }
            }
            else
            {
                Lightning(entity, 0);
            }
        }
    }

    private IEnumerator LightningAnim(float time)
    {
        float dT = 0;
        while (dT < time)
        {
            float width = Mathf.Lerp(.3f, .0f, dT / time);
            _line.startWidth = width;
            _line.endWidth = width;
            dT += Time.deltaTime;
            yield return null;
        }
        _line.startWidth = _line.endWidth = 0;
        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, Vector2.zero);
        }
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
