using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public float strength;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage) 
    {
        health -= damage;
        if (health <= 0) 
        {
            Destroy(this.gameObject);
        }
    }
}
