using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollect : MonoBehaviour
{
    [SerializeField] GameObject txtUse;
    [SerializeField] GameObject door;
    [SerializeField] Transform des;
    [SerializeField] GameObject tuto;
    [SerializeField] GameObject weaponPref;
    [SerializeField] GameObject timeLine;
    [SerializeField] GameObject weaponTxt;

    bool isMove, isActive, isOn;
    Vector3 destination;

    void Start()
    {
        if (PlayerPrefs.HasKey("arma"))
        {
            Destroy(timeLine);
            Destroy(gameObject);
        }

        destination = des.position;
    }

    private void Update()
    {
        if(Input.GetAxis("Interact") == 1 && isActive && !isOn)
        {
            isOn = true;
            txtUse.SetActive(false);
            PlayerPrefs.SetInt("arma", 1);
            isMove = true;

            Destroy(timeLine);
            Destroy(weaponPref);

            StartCoroutine(ShowText());

            tuto.SetActive(true);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isMove)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, destination, 7 * Time.deltaTime);

            if (door.transform.position == destination) isMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isOn)
        {
            isActive = true;
            txtUse.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isOn)
        {
            isActive = false;
            txtUse.SetActive(false);
        }
    }

    IEnumerator ShowText()
    {
        weaponTxt.SetActive(true);
        yield return new WaitForSeconds(2f);
        weaponTxt.SetActive(false);
    }
}
