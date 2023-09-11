using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilMovUniforme : MonoBehaviour
{
    private Vector3 hoyustus;
    private Rigidbody2D rb;
    private float tiempoEliminacion = 5f;
    private float danio;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        hoyustus = GameObject.Find("Hoyustus Solicitud Prefab").transform.position;
        aniadirFuerza();
    }

    void Update()
    {
        tiempoEliminacion -= Time.deltaTime;

        if (tiempoEliminacion <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void aniadirFuerza() {
        Vector2 direction = hoyustus - transform.position;
        float distance = Vector2.Distance(transform.position, hoyustus);
        rb.velocity = direction.normalized * 20;
    }

    public void setDanio(float danio) {
        this.danio = danio;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 6 || collision.gameObject.layer == 16)
        {
            if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<Hoyustus>().IsInvulnerable()) {
                collision.gameObject.GetComponent<Hoyustus>().RecibirDanio(danio);
                collision.gameObject.GetComponent<Hoyustus>().danioExterno((transform.position.x <= collision.transform.position.x) ? 1 : -1, 2f);

            }
            Destroy(gameObject);
        }
    }
}
