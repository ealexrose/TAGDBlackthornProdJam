﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float travelSpeed;
    public float bulletDamage;
    Vector3 velocity;
    bool hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetTarget(GameObject target) 
    {
        StartCoroutine(ShootTowardsTarget(target));
    }


    IEnumerator ShootTowardsTarget(GameObject target) 
    {
        for (float i = 0; i < 5f; i += Time.deltaTime)
        {
            if (target)
            {
                Vector3 travelVector = target.transform.position - transform.position;
                travelVector = travelVector.normalized;
                velocity = travelVector * travelSpeed * Time.deltaTime;
            }
            transform.position += velocity;
            yield return null;


        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            if (!hit)
            {
                collision.gameObject.GetComponent<EnemyController>().Damage(bulletDamage);
                hit = true;
            }

            Destroy(this.gameObject);
        }
    }

}