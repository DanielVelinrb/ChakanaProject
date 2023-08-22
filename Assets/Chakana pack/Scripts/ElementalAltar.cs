using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElementalAltar : MonoBehaviour
{
    [SerializeField] string doorName;
    [SerializeField] string keyCode;
    [SerializeField] GameObject altarFX;
    [SerializeField] GameObject txtUse;
    [SerializeField] GameObject details;
    [SerializeField] TextMeshProUGUI detailTxt;
    [SerializeField] GameObject explodeFx;

    private bool isIn, isOn;
    private string altarName;

    private void Start()
    {
        altarName = doorName + keyCode;

        if (!PlayerPrefs.HasKey(altarName))
        {
            PlayerPrefs.SetInt(altarName, 0);
        }
        else if (PlayerPrefs.GetInt(altarName) == 1)
        {
            altarFX.SetActive(true);
            isOn = true;
        }
    }

    void Update()
    {
        if (isIn && Input.GetButtonDown("Interact"))
        {
            altarFX.SetActive(true);
            txtUse.SetActive(false);
            
            isOn = true;
            isIn = false;

            PlayerPrefs.SetInt(altarName, 1);

            int e = PlayerPrefs.GetInt(doorName) + 1;
            PlayerPrefs.SetInt(doorName, e);
            StartCoroutine(ShowDetails());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !isOn)
        {
            isIn = true;
            txtUse.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !isOn)
        {
            isIn = false;
            txtUse.SetActive(false);
        }
    }

    IEnumerator ShowDetails()
    {
        int e = 3 - PlayerPrefs.GetInt(doorName);

        if(e == 2)
        {
            detailTxt.text = e + " MORE" + " ALTARS";
        }
        else if(e == 1)
        {
            detailTxt.text = e + " MORE" + " ALTAR";
        }
        else
        {
            detailTxt.text = "THE DOOR HAS OPENED";
            GetComponent<AudioSource>().Play();
            Instantiate(explodeFx);
        }

        details.SetActive(true);

        yield return new WaitForSeconds(4.4f);

        details.SetActive(false);
    }
}
