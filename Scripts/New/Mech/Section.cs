using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(HitRegister))]
public class Section : MonoBehaviour
{
    [HideInInspector]
    public bool isDestroyed;
    [HideInInspector]
    public bool isDestructible;

    [HideInInspector]
    public float lastDamageTime;
    [HideInInspector]
    public float ShieldRegenDelay;

    #region Base Stats
    private float maxHealth = 100;
    private float maxShield = 0;
    private float maxArmor = 100;
    private float shieldRegen = 0;
    #endregion

    #region Current Stats
    [SerializeField]
    float currentHealth;
    [SerializeField]
    float currentShield;
    [SerializeField]
    float currentShieldRegen;
    [SerializeField]
    float currentArmor;
    #endregion

    #region Bonus Stats

    #endregion

    // Use this for initialization
    void Start()
    {
        isDestroyed = false;

        currentShieldRegen = shieldRegen;           //In Points/Sec

        currentArmor = maxArmor;

        currentHealth = maxHealth;

        currentShield = maxShield;
    }

    void Update()
    {
        if (Time.time >= lastDamageTime + ShieldRegenDelay && maxShield > 0 && currentShield < maxShield)
        {
            RegenShield();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed)
            return;

        float totalDamage = 0;

        if (currentArmor > 0)
        {
            totalDamage += damage * Mathf.Pow(.8f, (currentArmor / 100));
        }
        else
        {
            totalDamage += damage;
        }

        float overflow = totalDamage - currentShield;

        // Damage breaks through shield
        if (overflow > 0)
        {
            currentHealth -= overflow;
            currentShield = 0;
        }
        // Damage doesnt break through shield
        else
        {
            currentShield -= totalDamage;
        }

        Debug.Log("Took " + totalDamage + " Damage, Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                ExecuteDestroy();
            }
        }

        lastDamageTime = Time.time;
    }

    /* Explode!!! */
    void ExecuteDestroy()
    {
        if (isDestructible)
        {
            gameObject.SetActive(false);
        }
    }

    public void HealHealth(float add)
    {
        currentHealth += add;
        Mathf.Clamp(currentHealth, -1, maxHealth);
    }

    public void HealShield(float add)
    {
        currentShield += add;
        Mathf.Clamp(currentShield, 0, maxShield);
    }

    public void RegenShield()
    {
        currentShield += currentShieldRegen * Time.deltaTime;
        Mathf.Clamp(currentShield, 0, maxShield);
    }

    public void AddHealthMax(float add)
    {
        maxHealth += add;
        currentHealth += add;
    }

    public void AddShieldMax(float add)
    {
        maxShield += add;
        currentShield += add;
    }

    public void AddShieldRegen(float add)
    {
        currentShieldRegen += add;
    }

    public void AddBallisticArmor(float add)
    {
        maxArmor += add;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public float GetMaxShield()
    {
        return maxShield;
    }

    public float GetBallisticArmor()
    {
        return currentArmor;
    }
}
