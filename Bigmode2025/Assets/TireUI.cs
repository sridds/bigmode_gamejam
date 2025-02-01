using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TireUI : MonoBehaviour
{
    private List<TireUIObject> tires = new List<TireUIObject>();

    [SerializeField]
    private TireUIObject tireUIPrefab;

    PlayerHealthScript health;

    private void Start()
    {
        UpdateTireCount(4);

        health = FindObjectOfType<PlayerHealthScript>();

        health.OnDamageTaken += TakeDamage;
        health.OnHealed += Heal;
    }

    private void TakeDamage(int oldHealth, int newHealth)
    {
        Debug.Log("take damage: " + (newHealth - oldHealth));

        for (int i = 0; i < oldHealth - newHealth; i++)
        {
            Destroy(tires[0].gameObject);
            tires.RemoveAt(0);
        }

        int shakeAmount = 24;

        for(int i = tires.Count - 1; i >= 0; i--)
        {
            if(shakeAmount > 0)
            {
                tires[i].Shake(shakeAmount);
                shakeAmount -= 8;
            }
        }
    }

    private void Heal(int oldHealth, int newHealth)
    {
        for (int i = 0; i < newHealth - oldHealth; i++)
        {
            TireUIObject newTire = Instantiate(tireUIPrefab, transform);
            tires.Add(newTire);
        }
    }

    public void UpdateTireCount(int newCount)
    {
        int count = newCount - tires.Count;
        Debug.Log("update: new count: " + newCount + ", tire count: " + tires.Count);

        if (newCount > tires.Count)
        {
            for(int i = 0; i < count; i++)
            {
                TireUIObject newTire = Instantiate(tireUIPrefab, transform);
                tires.Add(newTire);
            }
        }
    }

    void Update()
    {
        // normal jitter

        // low health jitter
    }
}
