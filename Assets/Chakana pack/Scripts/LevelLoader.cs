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
                player.localScale = transform.localScale;
            }
        }
    }

    private void Start()
    {
        int escena = SceneManager.GetActiveScene().buildIndex;
        AudioSource audio = null;

        if (GameObject.FindGameObjectWithTag("Music"))
        {
            audio = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        }
        else return;

        if (escena == 15)
        {
            audio.Stop();
        }
        else if(!audio.isPlaying)
        {
            audio.Play();
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

