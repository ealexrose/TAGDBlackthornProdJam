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

    #region Death State
    [Header("Death State")]
    [SerializeField] float Death_Velocity = 4;
    private Pathfinding.AIPath Enemy_AI;
    [HideInInspector] public bool Death_State;
    private float Random_X_Velocity;
    private Animator Rat_Anim;
    public AnimationCurve Y_Offset_On_Death;
    private float Death_Time;
    #endregion


    private void Awake()
    {
        Enemy_AI = GetComponent<Pathfinding.AIPath>();
        HealthBar_UI.InitializeHealth(health);
        Random_X_Velocity = Mathf.Sign(Random.Range(-1f,1f)) * Random.RandomRange(0.1f,2f);        // random direction and magnitude.

        Rat_Anim = GetComponent<Animator>();
        if (Gigantamax)
        {
            Target = GetComponent<Pathfinding.AIDestinationSetter>();
            totalHealth = health;
        }
    }


    public void Damage(float damage) 
    {
        if (Death_State) { return; }

        health -= damage;
        HealthBar_UI.UpdateHealth_UI(health);

        if (Gigantamax)         // big rats spawning smaller rats
        {
            HandleLargeEnemy();
        }

        if (health <= 0) 
        {
            if (ScreenShakeController.instance) 
            {
                ScreenShakeController.instance.ShakeScreen(0.1f, 0.1f);
            }
            SFX_Manager.Instance.PlayRandomSqueaks();
            GetComponent<CircleCollider2D>().enabled = false;
            Enemy_AI.enabled = false;
            Death_State = true;
            Rat_Anim.Play(Animator.StringToHash("Rat_Fade"));
            HealthBar_UI.transform.parent.parent.gameObject.SetActive(false);
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
            newEnemy.GetComponentInChildren<Pathfinding.AIDestinationSetter>().target = Target.target;
        }
    }

    private void Update()
    {
        if (Death_State)
        {
            Death_Time += Time.deltaTime;
            transform.position += new Vector3(Random_X_Velocity * moveSpeed * Time.deltaTime, 0, 0);       // shoot the camera in a random direction
            transform.Translate(0, Y_Offset_On_Death.Evaluate(Death_Time) * Time.deltaTime, 0, Space.World);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
