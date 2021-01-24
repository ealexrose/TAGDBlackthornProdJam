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
        transform.position = transform.position += (Vector3.down * moveSpeed * Time.deltaTime);
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
