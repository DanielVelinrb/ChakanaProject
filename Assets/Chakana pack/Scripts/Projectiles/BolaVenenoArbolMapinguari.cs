using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BolaVenenoArbolMapinguari : BolaVeneno
{
    private Vector3 hoyustus;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        hoyustus = GameObject.Find("Hoyustus Solicitud Prefab").transform.position + Vector3.up;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, hoyustus, 20 * Time.deltaTime);
        tiempoEliminacion -= Time.deltaTime;

        if (transform.position == hoyustus) {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (tiempoEliminacion <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    public void instanciarValores(GameObject explosion) {
        this.explosion = explosion;
        this.explosion.GetComponent<ExplosionBehaviour>().modificarValores(6, 25, 6, 12, "Veneno", "ExplosionEnemy");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 6 || collision.gameObject.layer == 16)
        {
            Debug.Log(collision.gameObject.name);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
