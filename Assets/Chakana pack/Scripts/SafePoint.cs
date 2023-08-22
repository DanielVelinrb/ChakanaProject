using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafePoint : MonoBehaviour
{

    [SerializeField] GameObject fire;
    [SerializeField] Transform pivot;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject txt;
    [SerializeField] int spNum;
    bool isIn, isOn;

    Hoyustus player;

    void Start()
    {
        if (PlayerPrefs.GetInt("SP" + spNum) == 1)
        {
            fire.SetActive(true);
        }

        isOn = true;
    }

    void Update()
    {
        if(isIn && Input.GetButtonDown("Interact")&& isOn)
        {
            fire.SetActive(true);
            Destroy(Instantiate(particles, pivot), 2.5f);

            PlayerPrefs.SetInt("SP" + spNum, 1);
            PlayerPrefs.SetInt("respawn", SceneManager.GetActiveScene().buildIndex);

            isOn = false;
            StartCoroutine(Timer());

            player.CurarCompletamente();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            isIn = true;
            txt.SetActive(true);

            player = collision.gameObject.GetComponent<Hoyustus>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            isIn = false;
            txt.SetActive(false);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
        isOn = true;
    }
}
