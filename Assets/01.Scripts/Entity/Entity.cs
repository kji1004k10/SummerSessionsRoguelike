using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    public readonly Stat Stat = new();
    public float HP { get; private set; }
    public float DashStamina { get; private set; }
    public float ParryingStamina { get; private set; }
    public float MaxHP { get { return Stat.Get(StatType.MaxHP); } }
    public float MaxDashStamina { get { return Stat.Get(StatType.MaxDashStamina); } }
    public float MaxParryingStamina { get { return Stat.Get(StatType.MaxParryingStamina); } }

    private readonly Queue<Action> _lateActions = new();
    private readonly List<StatusEffect> _effects = new();

    private readonly HashSet<StatusEffect> _deleteEffects = new();

    public void Init()
    {
        HP = Stat.Get(StatType.MaxHP);
        DashStamina = Stat.Get(StatType.MaxDashStamina);
        ParryingStamina = Stat.Get(StatType.MaxParryingStamina);
    }

    public void LateAct(Action action)
    {
        _lateActions.Enqueue(action);
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        UpdateEffects();

        print(HP);
    }

    public void AddEffect(StatusEffect eff)
    {
        _effects.Add(eff);
    }

    private void UpdateEffects()
    {
        _deleteEffects.Clear();
        foreach(var eff in _effects)
        {
            eff.OnUpdate(this);
            
            if((eff.Duration -= Time.deltaTime) <= 0f)
            {
                eff.OnFinish(this);
                _deleteEffects.Add(eff);
            }
        }
        
        _effects.RemoveAll(eff => _deleteEffects.Contains(eff));
    }

    private void LateUpdate()
    {
        Stat.Update();
        while(_lateActions.TryDequeue(out Action action))
        {
            action();
        }
    }

    public virtual void Attack(Entity other)
    {
        var damage = 1; // ���� ��, �����δ� ���
        other.Damage(damage);
    }

    public virtual void Damage(float damage)
    {
        // HP ��� �ڵ� ����
        HP -= damage;
        if(HP < 0) HP = 0;
    }

    public virtual void Heal(float amount)
    {
        HP += amount;
        if (HP > MaxHP) HP = MaxHP;
    }
}
