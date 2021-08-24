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
    private float baseMaxHealth = 100;
    private float baseMaxShield = 0;
    private float baseBallisticArmor = 100;
    private float baseEnergyArmor = 50;
    private float baseShieldRegen = 10;
    #endregion

    #region Current Stats
    public ScriptableFloat healthRef;
    public ScriptableFloat shieldRef;
    [SerializeField]
    float currentMaxHealth;
    [SerializeField]
    float currentMaxShield;
    [SerializeField]
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
        
        if (this.gameObject.tag == "Player")
            EquipmentManager.Instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void Update()
    {
        if (Time.time >= lastDamageTime + ShieldRegenDelay && currentMaxShield > 0 && currentShield < currentMaxShield)
        {
            RegenShield();
        }
    }

    public void OnEquipmentChanged(Item newItem, Item oldItem)
	{
		if (newItem != null) {
			//armor.AddModifier (newItem.armorModifier);
			//damage.AddModifier (newItem.damageModifier);
            AddHealthMax(newItem.health);
            AddShieldMax(newItem.shield);
            AddShieldRegen(newItem.shieldRegen);
            AddBallisticArmor(newItem.ballisticArmor);
            AddEnergyArmor(newItem.energyArmor);
		}

		if (oldItem != null)
		{
			//armor.RemoveModifier(oldItem.armorModifier);
			//damage.RemoveModifier(oldItem.armorModifier);
            AddHealthMax(-oldItem.health);
            AddShieldMax(-oldItem.shield);
            AddShieldRegen(-oldItem.shieldRegen);
            AddBallisticArmor(-oldItem.ballisticArmor);
            AddEnergyArmor(-oldItem.energyArmor);
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

        Debug.Log("Took " + totalDamage + " Damage, Current Health: " + currentHealth);

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

    public void HealShield(float add)
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
            Debug.Log("I Died");
            //gameObject.SetActive(false);
        }
    }

    public void EquipItem(Item item)
    {
        AddHealthMax(item.health);
        AddShieldMax(item.shield);
        AddShieldRegen(item.shieldRegen);
        AddBallisticArmor(item.ballisticArmor);
        AddEnergyArmor(item.energyArmor);
    }

    public void AddHealthMax(float add)
    {
        currentMaxHealth += add;
        currentHealth += add;
        if (healthRef != null) healthRef.value = currentMaxHealth;
    }

    public void AddShieldMax(float add)
    {
        currentMaxShield += add;
        currentShield += add;
        if (shieldRef != null) shieldRef.value = currentMaxShield;
    }

    public void AddShieldRegen(float add)
    {
        currentShieldRegen += add;
    }

    public void AddBallisticArmor(float add)
    {
        currentBallisticArmor += add;
    }

    public void AddEnergyArmor(float add)
    {
        currentEnergyArmor += add;
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
