using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Round : MonoBehaviour
{
    Text round_Text;
    int count_Round = 1;
    [SerializeField]
    GameObject midum_Monster;
    Sound bossSound;
    bool isCall = false;
    GameObject boss_Exit;
    Dictionary<int, Vector3> position = new Dictionary<int, Vector3>()
    {
        {1, new Vector3(30, 0, 30)},
        {2, new Vector3 (20, 0, 80)},
        {3, new Vector3(60, 0, 30)},
        {4, new Vector3(80,0,80)}
    };
    int jug_Count=0;
    void Start()
    {
        bossSound = GameObject.Find("Sound").GetComponent<Sound>();
        boss_Exit = GameObject.FindGameObjectWithTag("Enemy");
        boss_Exit.SetActive(false);
        round_Text = GameObject.Find("RoundText").GetComponent<Text>();
        if (count_Round != 1) return;
        jug_Count = 4;
        for (float n = 1; jug_Count-1 >= n; n++)
         {
            GenerateMidumMonster();
         }
        round_Text.text = count_Round + "R";
    }

    // Update is called once per frame
    void Update()
    {
        if (Bullet.bulletCount != jug_Count) return;
        count_Round++;
        round_Text.text = count_Round + "R";
        isCall = true;

        if (!isCall) return;

        switch (count_Round)
        {
           case 2:
             isCall = false;
             Bullet.bulletCount = 0;
             jug_Count = 6;
             for (float n = 1; jug_Count >= n; n++)
              {
                    GenerateMidumMonster();
              }
              break;
            case 3:
               isCall = false;
                Bullet.bulletCount = 0;
                jug_Count = 8;
               for (float n = 1; jug_Count >= n; n++)
               {
                    GenerateMidumMonster();
               }
               break;
            case 4:
               isCall = false;
               Bullet.bulletCount = 0;
               jug_Count =10;
               for (float n = 1; jug_Count >= n; n++)
                {
                    GenerateMidumMonster();
               }
               break;
            case 5:
                bossSound.BossSound();
                boss_Exit.SetActive(true);
                isCall = false;
                Bullet.bulletCount = 0;
                jug_Count = 10;
                for (float n = 1; jug_Count - 1 >= n; n++)
                {
                    GenerateMidumMonster();
                }
                break;

        }
    }
    void GenerateMidumMonster()
    {
        Instantiate(midum_Monster, position[Random.Range(1,4)], Quaternion.identity);
    }
    


}
