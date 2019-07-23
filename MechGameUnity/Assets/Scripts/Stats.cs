using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    public bool isDead;

    public float lastDamageTime;
    public float ShieldRegenDelay;

    #region Base Stats
    float baseMaxHealth = 100;
    float baseMaxShield = 0;
    float baseBallisticArmor = 100;
    float baseEnergyArmor = 50;
    float baseShieldRegen = 10;
    #endregion

    #region Current Stats
    ScriptableFloat healthRef;
    ScriptableFloat shieldRef;
    float currentMaxHealth;
    float currentMaxShield;
    float currentShieldRegen;
    [SerializeField]
    float currentShield;
    [SerializeField]
    float currentHealth;
    [SerializeField]
    float currentBallisticArmor;
    [SerializeField]
    float currentEnergyArmor;
    #endregion

    #region Bonus Stats

    #endregion

    // Use this for initialization
    void Start()
    {
        isDead = false;

        currentMaxHealth = baseMaxHealth;
        currentMaxShield = baseMaxShield;
        currentBallisticArmor = baseBallisticArmor;
        currentEnergyArmor = baseEnergyArmor;
        currentShieldRegen = baseShieldRegen;           //In Points/Sec

        currentHealth = currentMaxHealth;
        if (healthRef != null) healthRef.value = currentMaxHealth;

        currentShield = currentMaxShield;
        if (shieldRef != null) shieldRef.value = currentMaxShield;
    }

    void Update()
    {
        if (Time.time >= lastDamageTime + ShieldRegenDelay)
        {
            RegenShield();
        }
    }

    public void TakeDamage(float ballisticDam, float energyDam)
    {
        float totalDamage = 0;

        if (currentBallisticArmor > 0)
        {
            totalDamage += ballisticDam * Mathf.Pow(.8f, (currentBallisticArmor / 100));
        }
        else
        {
            totalDamage += ballisticDam;
        }

        if (currentEnergyArmor > 0)
        {
            totalDamage += energyDam * Mathf.Pow(.8f, (currentEnergyArmor / 100));
        }
        else
        {
            totalDamage += energyDam;
        }

        float overflow = totalDamage - currentShield;

        // Damage breaks through shield
        if (overflow > 0)
        {
            currentHealth -= overflow;
            currentShield = 0;

            if (healthRef != null) healthRef.value = currentHealth;
            if (shieldRef != null) shieldRef.value = 0;
        }
        // Damage doesnt break through shield
        else
        {
            currentShield -= totalDamage;

            if (shieldRef != null) shieldRef.value = currentShield;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        lastDamageTime = Time.time;
    }

    public void HealHealth(float add)
    {
        currentHealth += add;
        Mathf.Clamp(currentHealth, -1, currentMaxHealth);
        if (healthRef != null) healthRef.value = currentHealth;
    }

    public void HealSheild(float add)
    {
        currentShield += add;
        Mathf.Clamp(currentShield, 0, currentMaxShield);
        if (shieldRef != null) shieldRef.value = currentShield;
    }

    public void RegenShield()
    {
        currentShield += currentShieldRegen * Time.deltaTime;
        Mathf.Clamp(currentShield, 0, currentMaxShield);
        if (shieldRef != null) shieldRef.value = currentShield;
    }

    void Die()
    {
        if (!isDead)
        {
            isDead = true;
            this.enabled = false;
        }
    }

    public void SetHealthMax(float add)
    {
        currentMaxHealth = baseMaxHealth + add;
        if (healthRef != null) healthRef.value = currentMaxHealth;
    }

    public void SetShieldMax(float add)
    {
        currentMaxShield = baseMaxShield + add;
        if (shieldRef != null) shieldRef.value = currentMaxShield;
    }

    public void SetShieldRegen(float add)
    {
        currentShieldRegen = baseShieldRegen + add;
    }

    public void SetBallisticArmor(float add)
    {
        currentBallisticArmor = baseBallisticArmor + add;
    }

    public void SetEnergyArmor(float add)
    {
        currentEnergyArmor = baseEnergyArmor + add;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return currentMaxHealth;
    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public float GetMaxShield()
    {
        return currentMaxShield;
    }

    public float GetBallisticArmor()
    {
        return currentBallisticArmor;
    }

    public float GetEnergyArmor()
    {
        return currentEnergyArmor;
    }


}
