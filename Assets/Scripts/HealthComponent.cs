using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public int m_HealthPoints;
    private int m_MaxHealthPoints;
    public float HealthPercent
    {
        get
        {
            return (float)m_HealthPoints / (float)m_MaxHealthPoints;
        }
    }
    private void Awake()
    {
        m_MaxHealthPoints = m_HealthPoints;
    }

    public UnityEvent<DamageType> m_DeathReaction;

    public DamageReaction[] m_DamageReactions;

    private bool m_ReceievedDamageThisFrame = false;
    public void ReceieveDamage(DamageType p_Type, int p_Amount)
    {
        if (m_ReceievedDamageThisFrame)
        {
            return;
        }
        m_ReceievedDamageThisFrame = true;


        m_HealthPoints -= p_Amount;
        if (m_HealthPoints <= 0)
        {
            m_DeathReaction?.Invoke(p_Type);
            return;
        }


        foreach (var DR in m_DamageReactions)
        {
            if ((DR.m_DamageType & p_Type) != p_Type)
            {
                continue;
            }

            DR.m_Reaction?.Invoke(p_Amount);
        }
    }

    private void Update()
    {
        m_ReceievedDamageThisFrame = false;
    }
}
