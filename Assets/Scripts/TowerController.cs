using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    [HideInInspector]
    public GameObject target;
    public GameObject destructionPreview;
    GameObject instantiatedDestructionPreview;
    public float range;
    public float shootSpeedCooldown;
    public float damage;
    float cooldownTimer;
    bool isMouseOver;
    bool rangeDisplayed;

    public bool towerSlatedForDestruction;
    public bool destroyed;

    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<GameManager>().AddTower(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        isMouseOver = MouseCast();
        if (!destroyed)
        {
            if (!GameManager.BetweenWaves)
            {
                SearchAndShoot();
            }
            else
            {
                ChooseDestruction();
            }

            if (isMouseOver) 
            {
                ShowRange();
            }
        }
        if (!isMouseOver && rangeDisplayed) 
        {
            HideRange();
        }


    }

    private void ChooseDestruction()
    {

        if (isMouseOver)
        {
            if (!towerSlatedForDestruction && !instantiatedDestructionPreview)
            {
                instantiatedDestructionPreview = Instantiate(destructionPreview, gameObject.transform, false);
                instantiatedDestructionPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                instantiatedDestructionPreview.transform.localPosition = Vector3.zero;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (!towerSlatedForDestruction)
                {
                    instantiatedDestructionPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    towerSlatedForDestruction = true;
                    FindObjectOfType<GameManager>().towersSlatedForDestruction++;
                }
                else
                {
                    instantiatedDestructionPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
                    towerSlatedForDestruction = false;
                    FindObjectOfType<GameManager>().towersSlatedForDestruction--;
                }
            }
        }
        else if (!towerSlatedForDestruction && instantiatedDestructionPreview)
        {
            Destroy(instantiatedDestructionPreview);
        }
    }

    private bool MouseCast()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider)
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Mouse over tower");
                return true;
            }
        }

        return false;

    }

    private void SearchAndShoot()
    {
        bool isTargetValid = CheckTargetValid();
        ReduceCooldown();
        if (isTargetValid)
        {
            if (cooldownTimer <= 0)
            {
                ShootTarget();
            }
        }
        else
        {
            ScanForTarget();
        }
    }
    private void ReduceCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void ScanForTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        if (colliders.Length > 0)
        {
            GameObject HighestPriority = null;            // priority is based on the lowest amount of nodes left to travel.
            int LowestValue = 99999;                // amount of nodes that should be impossible to reach
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))       // all enemies should have a pathfinder.seeker component.
                {
                    Pathfinding.Seeker TestEnemy = collider.GetComponent<Pathfinding.Seeker>();
                    if (LowestValue >= TestEnemy.RetreivePriority())
                    {
                        LowestValue = TestEnemy.RetreivePriority();
                        HighestPriority = collider.gameObject;
                    }
                }
            }

            target = HighestPriority;
        }
    }

    private void ShootTarget()
    {
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity, null);
        bullet.GetComponent<BulletController>().SetTarget(target);
        bullet.GetComponent<BulletController>().bulletDamage = damage;
        cooldownTimer = shootSpeedCooldown;
    }

    private bool CheckTargetValid()
    {
        if (!target)
        {
            return false;
        }
        if (Vector3.Distance(target.transform.position, transform.position) > range)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ShowRange()
    {
        transform.GetChild(0).localScale = Vector3.one * range;
        rangeDisplayed = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void HideRange()
    {
        rangeDisplayed = false;
        transform.GetChild(0).localScale = Vector3.one;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, .5f, .5f);
        Gizmos.DrawSphere(transform.position, range);
        Gizmos.color = new Color(1f, 0, 0, 1f);
        if (target)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

    }
}
