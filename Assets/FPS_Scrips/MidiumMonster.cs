using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidiumMonster : MonoBehaviour
{
    [SerializeField]
    Animator midum_Animator;
    [SerializeField]
    AudioSource midum_voise;
    [SerializeField]
    NavMeshAgent midum_agent;
    [SerializeField]
    float midum_attackDamege;
    [SerializeField]
    float walkingSpeed;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject death_Particle;
    enum STATE { M_WANDER, M_ATTACK, M_CHASE, M_DEAD }
    [SerializeField]
    float runSpeed;
    STATE state = STATE.M_WANDER;
    GameObject target;
    int midumHp = 100, midumHpMax = 100;
    [SerializeField]
    bool isDamage;
    Vector3 midum_deathposition;
    GameState midumCheckState;
 
    public int MidumHp
    {
        get { return midumHp; }
    }
    public bool IsDamage
    {
        get { return isDamage; }
        set { isDamage = value; }
    }

    
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        midumCheckState = GameObject.Find("GameState").GetComponent<GameState>();
        
    }

    public void TurnOffTrigger()
    {
        midum_Animator.SetBool("MRun", false);
        midum_Animator.SetBool("MDeath", false);
        midum_Animator.SetBool("MAttack", false);
    }
    float DistanceToPlayer()
    {
        if (midumCheckState.GameOver)
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(target.transform.position, transform.position);
    }
    bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 15)
        {
            return true;
        }
        return false;
    }
    bool ForGetPlayer()
    {
        if (DistanceToPlayer() > 20)
        {
            return true;
        }
        return false;
    }
    public void DamagePlayer()
    {
        if (target != null)
        {
           
            target.GetComponent<PlayerController>().TakeHit(midum_attackDamege);
            
        }
    }
    public void DamageJuge()
    {
        if (isDamage)
        {
            DamagePlayer();
        }
        else
        {
            return;
        }
    }
    public void MidumMonDeath()
    {
        TurnOffTrigger();
        midum_Animator.SetBool("MDeath", true);
        state = STATE.M_DEAD;
    }
    public void EnemyTakeHit(float damage)
    {
        midumHp = (int)Mathf.Clamp(midumHp - damage, 0, midumHpMax);

    }
    void Update()
    {
    
        
        midum_deathposition = transform.position;
        
        switch (state)
        {


            case STATE.M_WANDER:
                if (!midum_agent.hasPath)
                {
                    float newX = transform.position.x + Random.Range(-50, 50);
                    float newZ = transform.position.z + Random.Range(-50, 50);
                    Vector3 NextPos = new Vector3(newX, transform.position.y, newZ);
                    midum_agent.SetDestination(NextPos);
                    midum_agent.stoppingDistance = 0;
                    TurnOffTrigger();
                    midum_agent.speed = walkingSpeed;

                }

                if (CanSeePlayer())
                {
                    state = STATE.M_CHASE;
                }
                if (Random.Range(0, 5000) < 5)
                {
                    MidumHowl();
                }
                break;
            case STATE.M_CHASE:
                if (midumCheckState.GameOver)
                {
                    TurnOffTrigger();
                    midum_agent.ResetPath();
                    state = STATE.M_WANDER;
                    return;
                }

                midum_agent.SetDestination(target.transform.position);
                midum_agent.stoppingDistance = 3;

                TurnOffTrigger();
                midum_agent.speed = runSpeed;
                midum_Animator.SetBool("MRun", true);

                if (midum_agent.remainingDistance < midum_agent.stoppingDistance)
                {
                    state = STATE.M_ATTACK;
                }

                if (ForGetPlayer())
                {
                    midum_agent.ResetPath();
                    state = STATE.M_WANDER;
                }
                break;
            case STATE.M_ATTACK:
                if (midumCheckState.GameOver)
                {
                    TurnOffTrigger();
                    midum_agent.ResetPath();
                    state = STATE.M_WANDER;
                    return;
                }
                TurnOffTrigger();
                midum_Animator.SetBool("MAttack", true);
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                if (DistanceToPlayer() > midum_agent.stoppingDistance + 2)
                {
                    state = STATE.M_CHASE;
                    MidumHowl();
                }
             
              
                break;
            case STATE.M_DEAD:
                Destroy(midum_agent);
                break;
        }

    }
    public void MidumHowl()
    {
        if (!midum_voise.isPlaying)
        {
            midum_voise.Play();
        }
    }
 
    public void Death()
    {
        Destroy(gameObject);
        midum_voise.Stop();
        GenerateDeathEffect();
        Instantiate(bullet,new Vector3(midum_deathposition.x, midum_deathposition.y + 1, midum_deathposition.z) ,Quaternion.Euler(0,0, -106.228f));

    }
    void GenerateDeathEffect()
    {
        var desposi = new Vector3(midum_deathposition.x, midum_deathposition.y, midum_deathposition.z);
        GameObject effect = Instantiate(death_Particle, desposi, Quaternion.identity);
    }


}
