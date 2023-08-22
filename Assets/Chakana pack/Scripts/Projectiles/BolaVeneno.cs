using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BolaVeneno : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float tiempoEliminacion = 5f;
    protected GameObject explosion;
    [SerializeField] private GameObject charco;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();;
        rb.Sleep();
    }

    private void Update()
    {
        tiempoEliminacion -= Time.deltaTime;
        if (tiempoEliminacion <= 0) {
            //HACER LA DIFERENCIACION CON EL LAYER SI TIENE UNA CAPA PLAYER O ENEMY
            //PLAYER
            if (this.gameObject.layer == 0) {
                Destroy(charco);
                Destroy(this.gameObject);
            }
            //ENEMY
            else if (this.gameObject.layer == 3) {
                //EXPLOSION
                Destroy(charco);
                Destroy(this.gameObject);
            }
        }
    }

    public void AniadirFuerza(float direccion, int layer) {
        transform.gameObject.layer = layer;
        gameObject.tag = "Veneno";
        rb.WakeUp();
        rb.AddForce(new Vector3(12f * -direccion, 18f, 0f), ForceMode2D.Impulse);
    }


    public void AniadirFuerza(float direccion, int layer, float velocityX, float velocityY, GameObject explosion)
    {
        transform.gameObject.layer = layer;
        gameObject.tag = "Veneno";
        rb.WakeUp();
        rb.AddForce(new Vector3(velocityX * -direccion, velocityY, 0f), ForceMode2D.Impulse);
        this.explosion = explosion;
        this.explosion.GetComponent<ExplosionBehaviour>().modificarValores(3, 15, 6, 12, "Veneno", "ExplosionEnemy");
    }


    private IEnumerator GenerarCharco(Vector3 position) {
        GetComponent<SpriteRenderer>().enabled = false;
        rb.velocity= Vector3.zero;
        rb.isKinematic = true;
        this.GetComponent<CircleCollider2D>().enabled = false;
        GameObject charcoGenerado = Instantiate(charco, transform.position, Quaternion.identity);
        charcoGenerado.name = "CharcoVenenoPlayer";
        //charcoGenerado.layer = 11;
        yield return new WaitForSeconds(5f);
        Destroy(charcoGenerado);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //ENEMY
        if ((collider.gameObject.tag == "Player" || collider.gameObject.layer == 6 || collider.gameObject.layer == 16 || collider.gameObject.layer == 17) && this.gameObject.layer == 3)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(charco);
            Destroy(this.gameObject);
        }
        else if (this.gameObject.layer == 11 && (collider.gameObject.layer == 6 || collider.gameObject.layer == 16 || collider.gameObject.layer == 17)) {
            tiempoEliminacion = 5;
            //GENERAR CHARCO
            StartCoroutine(GenerarCharco(transform.localPosition));
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //PLAYER
        if (collision.gameObject.layer == 6 || (collision.gameObject.layer == 17
            && collision.transform.position.y < transform.position.y))
        {
            tiempoEliminacion = 5;
            //GENERAR CHARCO
            StartCoroutine(GenerarCharco(transform.localPosition));

        }
        else if (collision.gameObject.layer == 3 || collision.gameObject.layer == 19) {
            //GENERAR BOLA DE VENENO DESCENDENTE
            tiempoEliminacion = 50f;
            this.GetComponent<CircleCollider2D>().isTrigger = true;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

    }


    public void setExplosion(GameObject explosion) {
        this.explosion = explosion;
    }
}
