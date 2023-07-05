using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stat
{
    private readonly Dictionary<StatType, float> 
        _defaultValues = new(), 
        _addValues = new(), 
        _multiplyValues = new(), 
        _currentValues = new();

    public Stat()
    {
        _defaultValues[StatType.MaxHP] = 100f;
        _defaultValues[StatType.MaxParryingStamina] = 100f;
        _defaultValues[StatType.MaxDashStamina] = 100f;
        _defaultValues[StatType.ParryingTime] = 0.2f;
        _defaultValues[StatType.ParryingStaminaRegen] = 10f;
        _defaultValues[StatType.DashCooldown] = 2f;
        _defaultValues[StatType.DashStaminaRegen] = 10f;
        _defaultValues[StatType.JumpForce] = 9f;
        _defaultValues[StatType.ParryingAttackForce] = 10f;
        _defaultValues[StatType.MoveSpeed] = 5f;
        _defaultValues[StatType.DashLength] = 4f;

        InitStats();
        UpdateStatValues();
    }

    private void InitStats()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
        foreach (var type in statTypes)
        {
            _addValues[type] = 0f;
            _multiplyValues[type] = 1f;
        }
    }

    private void UpdateStatValues()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
        foreach (var type in statTypes)
        {
            _currentValues[type] = (_defaultValues[type] + _addValues[type]) * _multiplyValues[type];
        }
    }

    public void Update()
    {
        UpdateStatValues();
        InitStats();
    }

    public void Add(StatType type, float addition)
    {
        _addValues[type] += addition;
    }

    public void Multiply(StatType type, float multiplier)
    {
        _multiplyValues[type] *= multiplier;
    }

    public float Get(StatType type)
    {
        return _currentValues[type];
    }
}

public enum StatType
{
    MaxHP, // �ִ� HP
    MaxDashStamina, // ��� ������ �ִ�ġ
    MaxParryingStamina, // �и� ������ �ִ�ġ
    ParryingTime, // �и� �ӵ�
    ParryingAttackForce, // �и� ���ݷ� 
    JumpForce, // ������
    MoveSpeed, // �̵� �ӵ�
    DashCooldown, // ��� ��Ÿ��
    ParryingStaminaRegen, // ���¹̳� �����
    DashStaminaRegen, // ��� �����
    DashLength // ��� �Ÿ�
}
