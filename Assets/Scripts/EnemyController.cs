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

    #region Big Rat
    [Header("Big Rat Only")]
    [Tooltip("Is the rat beeg?")]
    [SerializeField] bool Gigantamax = false;
    [SerializeField] GameObject SmallRat;
    Pathfinding.AIDestinationSetter Target;
    [SerializeField] int MaxEnemiesToSpawn = 20;
    [Range(0,3)][SerializeField] float MaxPositionOffset;
    float totalHealth;          // enemy will shrink based on their health
    #endregion

    private void Awake()
    {
        HealthBar_UI.InitializeHealth(health);
        if (Gigantamax)
        {
            Target = GetComponent<Pathfinding.AIDestinationSetter>();
            totalHealth = health;
        }
    }


    public void Damage(float damage) 
    {
        health -= damage;
        HealthBar_UI.UpdateHealth_UI(health);

        if (Gigantamax)         // big rats spawning smaller rats
        {
            HandleLargeEnemy();
        }

        if (health <= 0) 
        {
            ScreenShakeController.instance.ShakeScreen(0.1f, 0.1f);
            Destroy(this.gameObject);
        }
    }

    private void HandleLargeEnemy()
    {
        float newScale = 1 + health / totalHealth;
        transform.localScale = new Vector3(newScale, newScale, 1);          // The rat will shrink as it is shot

        if (Target.target != null && MaxEnemiesToSpawn > 0)
        {
            var newEnemy = Instantiate(SmallRat, transform.position, Quaternion.identity);
            float _XOffset = Random.Range(-MaxPositionOffset, MaxPositionOffset);
            float _YOffset = Random.Range(-MaxPositionOffset, MaxPositionOffset);
            newEnemy.transform.position += new Vector3(_XOffset, _YOffset, 0);
            MaxEnemiesToSpawn--;
            newEnemy.GetComponent<Pathfinding.AIDestinationSetter>().target = Target.target;
            
        }
    }
}
