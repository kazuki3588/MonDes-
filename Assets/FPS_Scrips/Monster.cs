using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Monster : MonoBehaviour
{
    [SerializeField]
    Animator boss_Animator;
    [SerializeField]
    NavMeshAgent boss_Agent;
    [SerializeField]
    AudioSource bossmonster_voise;
    [SerializeField]
    AudioSource bossmonster_attack_voise;
    [SerializeField]
    GameObject death_Particle;
    [SerializeField]
    int attackDamege;
    [SerializeField]
    float walkingSpeed;
    enum STATE { B_WANDER, B_ATTACK,B_CHASE, B_DEAD};
    [SerializeField]
    float runSpeed;
    STATE state = STATE.B_WANDER;
    GameObject target;
    GameState checkState;
    Vector3 boss_deathposition;
    int bossHp = 1000, bossHpMax = 1000;
    public int BossHp
    {
        get { return bossHp; }
    }

    void Start()
    {
        ChaeckTarget();
        AudioSource[] boss_AudioSources = GetComponents<AudioSource>();
        bossmonster_voise = boss_AudioSources[0];
        bossmonster_attack_voise = boss_AudioSources[1];
        checkState = GameObject.Find("GameState").GetComponent<GameState>();

    }
    public void TurnOffTrigger()
    {

        boss_Animator.SetBool("Run", false);
        boss_Animator.SetBool("Death", false);
        boss_Animator.SetBool("Attack", false);
     
    }
    float DistanceToPlayer()
    {
        if (checkState.GameOver)
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
        if(target != null)
        {
            BossAttackSound();
            target.GetComponent<PlayerController>().TakeHit(attackDamege);
        }
    }

    public void MonDeath()
    {
        TurnOffTrigger();
        boss_Animator.SetBool("Death", true);
        state = STATE.B_DEAD;
    }
    public void EnemyTakeHit(float damage)
    {
        bossHp = (int)Mathf.Clamp(bossHp - damage, 0, bossHpMax);
        if (bossHp <= 0 && !checkState.GameClear)
        {
            checkState.GameClear = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        boss_deathposition = transform.position;
        switch (state){
           

            case STATE.B_WANDER:
                if (!boss_Agent.hasPath)
                {
                    float newX = transform.position.x + Random.Range(-50, 50);
                    float newZ = transform.position.z + Random.Range(-50, 50);
                    Vector3 NextPos = new Vector3(newX, transform.position.y, newZ);
                    boss_Agent.SetDestination(NextPos);
                    boss_Agent.stoppingDistance = 0;
                    TurnOffTrigger();
                    boss_Agent.speed = walkingSpeed;
        
                }
                
                if (CanSeePlayer())
                {
                    state = STATE.B_CHASE;
                }
                if (Random.Range(0, 5000) < 5)
                {
                    BossHowl();
                }
                break;
            case STATE.B_CHASE:
                if (checkState.GameOver)
                {
                    TurnOffTrigger();
                    boss_Agent.ResetPath();
                    state = STATE.B_WANDER;
                    return;
                }

                boss_Agent.SetDestination(target.transform.position);
                boss_Agent.stoppingDistance = 3;

                TurnOffTrigger();
                boss_Agent.speed = runSpeed;
                boss_Animator.SetBool("Run", true);

                if (boss_Agent.remainingDistance < boss_Agent.stoppingDistance)
                {
                    state = STATE.B_ATTACK;
                }

                if (ForGetPlayer())
                {
                    boss_Agent.ResetPath();
                    state = STATE.B_WANDER;
                }
                break;
            case STATE.B_ATTACK:
                if (checkState.GameOver)
                {
                    TurnOffTrigger();
                    boss_Agent.ResetPath();
                    state = STATE.B_WANDER;
                    return;
                }
                TurnOffTrigger();
                boss_Animator.SetBool("Attack" ,true);
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                if(DistanceToPlayer() > boss_Agent.stoppingDistance + 2)
                {
                    state = STATE.B_CHASE;
                    BossHowl();
                }
                break;
            case STATE.B_DEAD:
                Destroy(boss_Agent);
                break;
        }
    }
    public void BossHowl()
    {
        if (!bossmonster_voise.isPlaying)
        {
            bossmonster_voise.Play();
        }
    }
    public void BossAttackSound()
    {
        bossmonster_attack_voise.PlayOneShot(bossmonster_attack_voise.clip);
    }
    public void Death()
    {
        Destroy(gameObject);
        bossmonster_attack_voise.Stop();
        bossmonster_voise.Stop();
        GenerateDeathEffect();
    }
    public void ChaeckTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void GenerateDeathEffect()
    {
        var desposi = new Vector3(boss_deathposition.x, boss_deathposition.y, boss_deathposition.z);
        GameObject effect = Instantiate(death_Particle, desposi, Quaternion.identity);
    }
}
