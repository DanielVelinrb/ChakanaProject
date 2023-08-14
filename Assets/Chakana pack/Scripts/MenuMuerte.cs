using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuMuerte : MonoBehaviour
{
    bool isDead;
    int sceneLoad;

    [SerializeField] GameObject loadScenePanel;

    private void Start()
    {
        isDead = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            RespawnContinue();
        }
    }

    public void changeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        correccionLogicas();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void changeScene()
    {
        PlayerData data = SaveManager.LoadPlayerData();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        correccionLogicas();
        if (data != null)
        {
            //SceneManager.LoadScene(data.getSceneName());
            //PlayerPrefs.SetFloat("nextPositionXPrefsName", data.getX());
            //PlayerPrefs.SetFloat("nextPositionYPrefsName", data.getY());
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RespawnContinue()
    {
        PlayerPrefs.SetInt("scenePos", 0);
        Time.timeScale = 1;

        if (!PlayerPrefs.HasKey("respawn"))
        {
            sceneLoad = 2;
        }
        else sceneLoad = PlayerPrefs.GetInt("respawn");

        StartCoroutine(LoadNextScene());
    }

    private void correccionLogicas() {
        Physics2D.IgnoreLayerCollision(0, 3, false);
        Physics2D.IgnoreLayerCollision(11, 3, false);
        //this.gameObject.tag = "Player";
        //this.gameObject.layer = 11;
    }

    IEnumerator LoadNextScene()
    {
        loadScenePanel.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneLoad);

        while (asyncLoad.isDone == false)
        {
            yield return null;
        }
    }
}
