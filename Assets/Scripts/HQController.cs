using System;
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

    // Update is called once per frame
    void Update()
    {

    }



    public void DamageHQ(float damage) 
    {
        health -= damage;
        HealthBar_UI.UpdateHealth_UI(health);
        if (health <= 0) 
        {
            GameManager.LoseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DamageHQ(collision.gameObject.GetComponent<EnemyController>().strength);
            Destroy(collision.gameObject);
        }
    }
}
