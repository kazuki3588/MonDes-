using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
class OperationManager : MonoBehaviour
{
    [SerializeField]
    AudioSource ClickSound;
    public void BackBotton()
    {
        StartCoroutine("LoadTitleScene");
        ClickSound.PlayOneShot(ClickSound.clip);
    }
    IEnumerator LoadTitleScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TitleScene");
    }
}
