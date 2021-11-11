    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool canShoot;
    bool gameOver = false;
    bool gameClear = false;
    public static GameState gameStateInstance;
    PlayerController player_Instance;
    Monster monster_Instance;
    public bool GameOver
    {
        set { gameOver = value; }
        get { return gameOver; }
    }
    public bool GameClear
    {
        set { gameClear = value; }
        get { return gameClear; }
    }
    void Awake()
    {
        CheckInstance();
        
    }
    void Start()
    {
        player_Instance = GameObject.Find("Player").GetComponent<PlayerController>();
        monster_Instance = GameObject.Find("BossMonster").GetComponent<Monster>();
    }
    void Update()
    {
        
    }
    public void CheckGameState()
    {
        if (player_Instance.PlayerHP <= 0 && !gameOver)
        {
            gameOver = true;
        }
        if(monster_Instance.BossHp <= 0 && !gameClear)
        {
            gameClear = true;
        }

    }
    public void CheckInstance()
    {
        if (gameStateInstance == null)
        {
            gameStateInstance = this;
        }
    }
 
}
