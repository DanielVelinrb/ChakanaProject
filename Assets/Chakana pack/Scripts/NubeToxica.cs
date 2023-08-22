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
    [SerializeField] private int etapas = 7;
    [SerializeField] private float tiempoEtapa = 2f;
    void Start()
    {
        hoyustus = GameObject.FindGameObjectWithTag("Player").GetComponent<Hoyustus>();
    }

    public void IsMapinguariDerrotado(bool mapinguariDerrotado){
        this.mapinguariDerrotado = mapinguariDerrotado;
    }


    private void Update()
    {
        if (visibilidadNube >= -0.2f && visibilidadNube <= etapas * tiempoEtapa)
        {
            if (mapinguariDerrotado)
            {
                if (visibilidadNube > 0)
                    signoVisibilidad = -1;
                else
                    Destroy(gameObject);
            }

            visibilidadNube += Time.deltaTime * signoVisibilidad;
            if (visibilidadNube >= etapas * tiempoEtapa)
            {
                if (danioPlayerDisponible)
                {
                    danioPlayerDisponible = false;
                    StartCoroutine(DaniarPlayer());
                }
                visibilidadNube = 14;
            }
            else if (visibilidadNube > (etapas - 1) * tiempoEtapa)
            {
                transform.GetChild((etapas - 1)).gameObject.SetActive(true);
            }
            else if (visibilidadNube <= (etapas - 1) * tiempoEtapa && visibilidadNube > 0)
            {
                transform.GetChild((int)(visibilidadNube/ tiempoEtapa)).gameObject.SetActive(true);
                transform.GetChild((int)(visibilidadNube / tiempoEtapa) + 1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                signoVisibilidad = 0;
                visibilidadNube = 0;
            }
        }
    }

    private IEnumerator DaniarPlayer() {
        hoyustus.RecibirDanio(25);
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
