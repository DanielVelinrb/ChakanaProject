using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NubeToxica : MonoBehaviour
{
    [SerializeField] private float visibilidadNube = 0;
    [SerializeField] private float signoVisibilidad = 0;
    [SerializeField] private bool danioPlayerDisponible = true;
    [SerializeField] private bool mapinguariDerrotado = false;
    [SerializeField] private Hoyustus hoyustus;
    void Start()
    {
        hoyustus = GameObject.FindGameObjectWithTag("Player").GetComponent<Hoyustus>();
    }

    public void IsMapinguariDerrotado(bool mapinguariDerrotado){
        this.mapinguariDerrotado = mapinguariDerrotado;
    }


    private void Update()
    {
        if (visibilidadNube >= -0.2f && visibilidadNube <= 10)
        {
            if (mapinguariDerrotado)
            {
                if (visibilidadNube > 0)
                    signoVisibilidad = -1;
                else
                    Destroy(gameObject);
            }

            visibilidadNube += Time.deltaTime * signoVisibilidad;
            if (visibilidadNube >= 10)
            {
                if (danioPlayerDisponible)
                {
                    danioPlayerDisponible = false;
                    StartCoroutine(daniarPlayer());
                }
                visibilidadNube = 10;
            }
            else if (visibilidadNube > 8)
            {
                transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (visibilidadNube > 6)
            {
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (visibilidadNube > 4)
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (visibilidadNube > 2)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (visibilidadNube > 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                signoVisibilidad = 0;
            }
        }
    }

    private IEnumerator daniarPlayer() {
        hoyustus.recibirDanio(25);
        yield return new WaitForSeconds(1f);
        danioPlayerDisponible = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            signoVisibilidad = 1;        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            signoVisibilidad = -1;
        }
    }

}
