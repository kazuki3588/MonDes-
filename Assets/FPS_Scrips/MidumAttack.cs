using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidumAttack :MonoBehaviour
{
    MidiumMonster midum_Instance;
    void Start()
    {
        midum_Instance = GameObject.FindGameObjectWithTag("MidumEnemy").GetComponent<MidiumMonster>();
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player")
        {
            midum_Instance.DamagePlayer();
        }
    } 
}
