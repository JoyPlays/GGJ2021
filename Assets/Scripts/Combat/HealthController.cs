using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthCanvas healthCanvas;
    [SerializeField] private float startHealth = 100f;

    [SerializeField, ReadOnly] public float health;

    public bool Alive { get; set; } = true;

    public float Health
    {
        get => health;
        set => health = value;
    }

    private IDamageable owner;

    private void Awake()
    {
        owner = GetComponent<IDamageable>();
        Health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!Alive)
        {
            return;
        }

        if (amount > 0f)
        {
            Health -= amount;

            healthCanvas.ChangeBar(Health / startHealth);
        }

        if (Health <= 0f)
        {
            Alive = false;
            owner.Die();
        }
    }
}
