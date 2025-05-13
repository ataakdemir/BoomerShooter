using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManaManager : MonoBehaviour
{
    public static PlayerManaManager Instance;

    [Header("Mana Settings")]
    public float maxMana = 100f;
    public float regenRate = 1f;
    public float regenDelayAfterFire = 2f;

    [Header("UI")]
    public TextMeshProUGUI manaText;

    [HideInInspector] public float currentMana;

    private float regenCooldownTimer = 0f;
    private float regenTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentMana = maxMana;
        UpdateManaUI();
    }

    void Update()
    {
        if (regenCooldownTimer > 0f)
        {
            regenCooldownTimer -= Time.deltaTime;
        }
        else
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= 1f)
            {
                regenTimer = 0f;
                currentMana = Mathf.Min(currentMana + regenRate, maxMana);
                UpdateManaUI();
            }
        }
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            regenCooldownTimer = regenDelayAfterFire;
            UpdateManaUI();
            return true;
        }
        return false;
    }

    public void UpdateManaUI()
    {
        if (manaText != null)
            manaText.text = "Mana: " + Mathf.FloorToInt(currentMana);
    }
}