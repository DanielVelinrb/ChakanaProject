using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldObj : MonoBehaviour
{
    [SerializeField] int goldType;
    [SerializeField] int goldTypeFisic;
    [SerializeField] bool heal;

    int amount;
    void Start()
    {
        if (goldType == 0)
        {
            if(PlayerPrefs.HasKey("goldfisic" + goldTypeFisic))
            {
                Destroy(gameObject);
            }

            amount = 10;
        }
        else
        {
            int e = Random.Range(1, 11);

            if (e > 6)
            {
                Destroy(gameObject);
            }
        }

        if (goldType == 1) amount = 3;
        else if (goldType == 2) amount = 5;
        else if (goldType == 3) amount = 7;
        else if (goldType == 4) amount = 1;
        else if (goldType == 5) amount = 4;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !heal)
        {
            if(goldType == 0)
            {
                PlayerPrefs.SetInt("gold" + goldTypeFisic, 1);
            }

            collision.GetComponent<Hoyustus>().setGold(amount);
            Destroy(gameObject);
        }
        else if(collision.tag == "Player" && heal)
        {
            collision.GetComponent<Hoyustus>().setCargaCuracion(50);
            Destroy(gameObject);
        }
    }
}
