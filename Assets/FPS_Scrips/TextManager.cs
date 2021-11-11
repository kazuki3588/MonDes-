using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TextManager : MonoBehaviour
{
    Text gameText;
    GameState updateText;
    PlayerController checkplayer;
    GameObject reloadButton;
    GameObject gunButton;
    // Start is called before the first frame update
    void Start()
    {
        gameText = GetComponent<Text>();
        updateText = GameObject.Find("GameState").GetComponent<GameState>();
        checkplayer = GameObject.Find("Player").GetComponent<PlayerController>();
        reloadButton = GameObject.Find("ReloadBotton");
        gunButton = GameObject.Find("GunBotton");
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameText();
    }
    void CheckGameText()
    {
        if (updateText.GameClear)
        {
        
            gameText.text = "GAMECLEAR";
            StartCoroutine("EndScene");
        }
        else if(updateText.GameOver)
        {
           
            checkplayer.enabled = false;
            gameText.text = "GAMEOVER";
            StartCoroutine("EndScene"); 

        }
    }
    IEnumerator EndScene()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("TitleScene");
    }
}
