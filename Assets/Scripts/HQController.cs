﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQController : MonoBehaviour
{
    public float health;
    [SerializeField] Healthbar HealthBar_UI;

    private void Awake()
    {
        HealthBar_UI.InitializeHealth(health);
    }

    public void DamageHQ(float damage) 
    {
        health -= damage;
        ScreenShakeController.instance.ShakeScreen(0.4f, 0.15f);        // big shake!!
        HealthBar_UI.UpdateHealth_UI(health);
        if (health <= 0) 
        {
            GameManager.instance.LoseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enem = collision.gameObject.GetComponent<EnemyController>();
            if (enem.Death_State) { return; }       // if the enemy is dead, dont let it attack
            DamageHQ(enem.strength);
            Destroy(enem.transform.parent.gameObject);
        }
    }
}
