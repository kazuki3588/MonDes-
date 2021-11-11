using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    AudioSource clickSound;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushStartButton()
    {
        SceneManager.LoadScene("GameScene");
        clickSound.PlayOneShot(clickSound.clip);
        Bullet.bulletCount = 0;
    }
    public void PushOperationButton()
    {
        StartCoroutine("LoadSceneGame");
        clickSound.PlayOneShot(clickSound.clip);
       
    }
    IEnumerator LoadSceneGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("OperationScene");
    }
}
