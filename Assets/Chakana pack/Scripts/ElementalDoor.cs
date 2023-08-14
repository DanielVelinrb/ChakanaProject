using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElementalDoor : MonoBehaviour
{
    [SerializeField] string doorName;
    [SerializeField] Transform openP, closeP;
    [SerializeField] TextMeshPro adTxt;

    private Vector3 destination;
    private bool isMove = false;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey(doorName))
        {
            PlayerPrefs.SetInt(doorName, 0);
            destination = closeP.position;
            isMove = true;
        }
        else if (PlayerPrefs.GetInt(doorName) < 3)
        {
            transform.position = closeP.position;
        }
        else if (PlayerPrefs.GetInt(doorName) == 3)
        {
            PlayerPrefs.SetInt(doorName, 4);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void LateUpdate()
    {
        if (isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, 4 * Time.deltaTime);

            if (transform.position == destination) isMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            int e = 3 - PlayerPrefs.GetInt(doorName);
            adTxt.text = "ACTIVATE " + e + " ALTARS";
            adTxt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            adTxt.gameObject.SetActive(false);
        }
    }
}
