using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int sceneNum;
    [SerializeField] int scenePos;
    [SerializeField] int actualPos;
    [SerializeField] Transform player;
    Transform appearPos;

    private bool isActive = false;

    [SerializeField] private GameObject loadScenePanel;

    private void Awake()
    {
        appearPos = transform.GetChild(0);

        if (PlayerPrefs.HasKey("scenePos"))
        {
            if (PlayerPrefs.GetInt("scenePos") == actualPos)
            {
                player.position = appearPos.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.GetComponent<Hoyustus>().SavePlayerData();
            PlayerPrefs.SetInt("scenePos", scenePos);
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        player.gameObject.GetComponent<AudioSource>().enabled = false;
        loadScenePanel.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNum);

        while(asyncLoad.isDone == false)
        {
            yield return null;
        }
    }
}

