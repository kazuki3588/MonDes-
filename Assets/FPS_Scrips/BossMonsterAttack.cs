using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterAttack : MonoBehaviour
{
    Monster boss_Instance;
    void Start()
    {
        boss_Instance = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Monster>();
    }
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.tag == "Player")
        {
            boss_Instance.DamagePlayer();
        }
    }

}
