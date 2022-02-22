using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SectionType { Torso, Head, Legs, LeftArm, RightArm, LeftShoulder, RightShoulder, None };

[RequireComponent (typeof(HitRegister))]
public class Section : MonoBehaviour
{
    public SectionType sectionType = SectionType.None;

    [HideInInspector]
    public bool IsDestroyed { get; private set; } = false;
    [HideInInspector]
    public bool IsCritical { get; private set; } = false;

    float lastDamageTime;
    float shieldRegenDelay;

    #region Base Stats
    public float MaxHealth { get; private set; } = 100;
    public float MaxShield { get; private set; } = 0;
    public float MaxArmor { get; private set; } = 100;
    public float ShieldRegen { get; private set; } = 0;
    #endregion

    #region Current Stats
    public float CurrentHealth { get; private set; } = 0;
    public float CurrentHealthPercent { get; private set; } = 1;
    public float CurrentShield { get; private set; } = 0;
    public float CurrentShieldRegen { get; private set; } = 0;
    public float CurrentArmor { get; private set; } = 0;
    #endregion

    #region Bonus Stats

    #endregion

    //public MechManagerEvent OnDamageTaken;
    public Action<MechManager, float> OnDamageTaken;

    // Use this for initialization
    void Start()
    {
        IsDestroyed = false;

        CurrentShieldRegen = ShieldRegen;           //In Points/Sec

        CurrentArmor = MaxArmor;

        CurrentHealth = MaxHealth;
        CurrentHealthPercent = 1;

        CurrentShield = MaxShield;

        if (sectionType == SectionType.Torso) IsCritical = true;
    }

    void Update()
    {
        if (Time.time >= lastDamageTime + shieldRegenDelay && MaxShield > 0 && CurrentShield <  MaxShield)
        {
            RegenShield();
        }
    }

    public void TakeDamage(float damage, MechManager source)
    {
        if (IsDestroyed)
            return;

        float totalDamage = 0;

        if (CurrentArmor > 0)
        {
            totalDamage += damage * Mathf.Pow(.8f, (CurrentArmor / 100));
        }
        else
        {
            totalDamage += damage;
        }

        float overflow = totalDamage - CurrentShield;

        // Damage breaks through shield
        if (overflow > 0)
        {
            CurrentHealth -= overflow;
            CurrentShield = 0;
        }
        // Damage doesnt break through shield
        else
        {
            CurrentShield -= totalDamage;
        }

        if (CurrentHealth <= 0)
        {
            if (!IsDestroyed)
            {
                IsDestroyed = true;
                ExecuteDestroy();
            }
        }

        lastDamageTime = Time.time;

        CurrentHealthPercent = CurrentHealth / MaxHealth;

        OnDamageTaken(source, damage);
    }

    /* Explode!!! */
    void ExecuteDestroy()
    {
        if (IsCritical)
        {
            gameObject.SetActive(false);
        }
    }

    public void HealHealth(float add)
    {
        CurrentHealth += add;
        Mathf.Clamp(CurrentHealth, -1, MaxHealth);
    }

    public void HealShield(float add)
    {
        CurrentShield += add;
        Mathf.Clamp(CurrentShield, 0, MaxShield);
    }

    public void RegenShield()
    {
        CurrentShield += CurrentShieldRegen * Time.deltaTime;
        Mathf.Clamp(CurrentShield, 0, MaxShield);
    }
}
