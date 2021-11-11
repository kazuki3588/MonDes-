using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shotDirection;
   
    [SerializeField]
    int attackDamege;
    PlayerController pcl;
    // Start is called before the first frame update
    void Start()
    {
        pcl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shooting()
    {
        RaycastHit hitInfo;
       
        if(Physics.Raycast(shotDirection.transform.position, shotDirection.transform.forward, out hitInfo, 300))
        {
            

            if (hitInfo.collider.gameObject.GetComponent<Monster>() != null)
            {
                Monster hitMonster = hitInfo.collider.gameObject.GetComponent<Monster>();
                pcl.HitSE();
                hitMonster.EnemyTakeHit(attackDamege);
                if(hitMonster.BossHp <= 0)
                {
                    hitMonster.MonDeath();
                }
           
            }
            
        }
        RaycastHit hitMidumInfo;
        if (Physics.Raycast(shotDirection.transform.position, shotDirection.transform.forward, out hitMidumInfo, 300))
        {

            if (hitMidumInfo.collider.gameObject.GetComponent<MidiumMonster>() != null)
            {
                MidiumMonster hitMidumMonster = hitMidumInfo.collider.gameObject.GetComponent<MidiumMonster>();
                pcl.HitSE();
                hitMidumMonster.EnemyTakeHit(attackDamege);
                
                if (hitMidumMonster.MidumHp <= 0)
                {
                    hitMidumMonster.MidumMonDeath();
                }

            }

        }

    }
}
