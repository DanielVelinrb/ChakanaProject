using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Guardado");
            Hoyustus data = collider.gameObject.GetComponent<Hoyustus>();
            float vida = data.getVida();
            int gold = data.getGold();
            string scene = SceneManager.GetActiveScene().name;
            Transform position = data.gameObject.transform;
            Debug.Log(position.transform.position);
            //SaveManager.SavePlayerData(vida, gold, scene, position.position.x, position.position.y, position.position.z);
        }
    }
}
