using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    private readonly Dictionary<StatType, float> _defaultValues = new(), _addValues = new(), _multiplyValues = new();

    public Stat()
    {
        StatType[] statTypes = (StatType[]) System.Enum.GetValues(typeof(StatType));
        foreach(var type in statTypes)
        {
            _defaultValues[type] = 0f;
        }
    }
}

public enum StatType
{
    MaxHP, // �ִ� HP
    MaxDashStamina, // ��� ������ �ִ�ġ
    MaxParryingStamina, // �и� ������ �ִ�ġ
    ParryingSpeed, // �и� �ӵ�
    ParryingAttackForce, // �и� ���ݷ� 
    JumpForce, // ������
    MoveSpeed, // �̵� �ӵ�
    DashCooldown, // ��� ��Ÿ��
    ParryingStaminaRegen, // ���¹̳� �����
    DashStaminaRegen // ��� �����
}
