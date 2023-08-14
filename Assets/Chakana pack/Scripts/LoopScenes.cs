using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopScenes : MonoBehaviour
{
    string escena;

    private LoopScenes instance;

    public LoopScenes Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        escena = SceneManager.GetActiveScene().name;
        if (escena != "14-Boss Room" && escena != "00- Main Menu 0")
        {
            Debug.Log("Awake() Aplica DontDestroyOnLoad(this.gameObject) al objeto; escena: " + escena);

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Awake() Aplica Destroy(this.gameObject) al objeto; escena: " + escena);
            Destroy(this.gameObject);
        }
        
        
        //Debug.Log("Aplica DontDestroyOnLoad(this.gameObject) al objeto; "+this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        escena = SceneManager.GetActiveScene().name;


        if (escena != "14-Boss Room" && escena != "00- Main Menu 0")
        {
            Debug.Log("Update() Aplica DontDestroyOnLoad(this.gameObject) al objeto; escena: " + escena);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Update() Aplica Destroy(this.gameObject) al objeto; escena: " + escena);
            Destroy(this.gameObject);
        }


        //Debug.Log("Aplica DontDestroyOnLoad(this.gameObject) al objeto; "+this.gameObject);
    }*/
}
