using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public float strength;
    // Update is called once per frame
    public Healthbar HealthBar_UI;

    private void Awake()
    {
        HealthBar_UI.InitializeHealth(health);
    }

    void Update()
    {

    }

    public void Damage(float damage) 
    {
        health -= damage;
        HealthBar_UI.UpdateHealth_UI(health);
        if (health <= 0) 
        {
            Destroy(this.gameObject);
        }
    }
}
