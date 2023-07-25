using Cinemachine.Utility;
using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ApallimayDaga : CharactersBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 objetivo;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private bool ataqueDisponible;
    //[SerializeField] private GameObject explosion;
    [SerializeField] private bool atacando;
    [SerializeField] private Vector3 limit1;
    [SerializeField] private Vector3 limit2;
    [SerializeField] private float direction = 1;
    [SerializeField] private float detectionTime = 0;
    [SerializeField] private float posY = 0;
    [SerializeField] Transform groundDetector;
    [SerializeField] LayerMask groundLayer;
    //[SerializeField] LayerMask wallLayer;
    [SerializeField] private CapsuleCollider2D daga;
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float cooldownAtaque;


    [SerializeField] private bool prueba = false;
    [SerializeField] GameObject deathFX;
    /*[SerializeField] private GameObject combFX01;
    [SerializeField] private GameObject combFX02;
    [SerializeField] private GameObject combFX03;

    private GameObject combObj01, combObj02, combObj03;*/


    void Start()
    {
        explosionInvulnerable = "ExplosionEnemy";
        layerObject = transform.gameObject.layer;
        fuerzaRecoil = 2f;
        ataqueDisponible = true;
        rb = GetComponent<Rigidbody2D>();
        explosion = Resources.Load<GameObject>("Explosion");
        daga = transform.GetChild(2).gameObject.GetComponent<CapsuleCollider2D>();
        limit1 = transform.GetChild(0).gameObject.transform.position;
        limit2 = transform.GetChild(1).gameObject.transform.position;
        objetivo = limit2;
        posY = transform.position.y;
        groundDetector = transform.GetChild(4).gameObject.transform;
        vidaMax = vida;
    }


    void Update()
    {
        if (playable) {
            //Falling();
        }
        //Debug.DrawLine(transform.position, transform.position + Vector3.right * 15 * direction, Color.red, 0.2f);
        Muerte();
        //Flip();

        // if (transform.position.x < limit1.x || transform.position.x > limit2.x)
        //{
        if (!atacando) {
            Flip();
        }
        //}
        detectarPiso();


        if (playable && !atacando && Grounded())
        {
            Move();
        }
    }

    private void Muerte()
    {
        if (vida <= 0)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void Move() {

        rb.velocity = new Vector2(direction * speed * (1 - afectacionViento), rb.velocity.y);

        if (transform.position.x <= limit1.x)
        {
            objetivo = limit2;
            //Flip();
        }
        else if (transform.position.x >= limit2.x)
        {
            objetivo = limit1;
            //Flip();
        }
    }

    private void Flip()
    {
        if (transform.position.x < objetivo.x)
        {
            direction = 1;
        }
        else if (transform.position.x > objetivo.x)
        {
            direction = -1;
        }
        transform.localScale = new Vector3(direction, 1, 0);
    }


    private IEnumerator Ataque() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        playable = false;
        yield return new WaitForSeconds(0.4f);
        atacando = true;
        rb.AddForce(new Vector2(direction * 12f, 0f), ForceMode2D.Impulse);
        daga.enabled = true;
        yield return new WaitForSeconds(0.4f);
        atacando = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        daga.enabled = false;
        yield return new WaitForSeconds(0.2f);
        playable = true;
        detectionTime = 0;
    }


    protected override void Recoil(int direccion, float fuerzaRecoil)
    {
        playable = false; //EL OBJECT ESTARIA SIENDO ATACADO Y NO PODRIA ATACAR-MOVERSE COMO DE COSTUMBRE
        rb.AddForce(new Vector2(direccion * 7, rb.gravityScale * 4), ForceMode2D.Impulse);
        //EstablecerInvulnerabilidades(layerObject);
    }


    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.gameObject.layer == 14)
        {
            int direccion = 1;
            if (collider.transform.position.x > gameObject.transform.position.x)
            {
                direccion = -1;
            }
            else
            {
                direccion = 1;
            }

            //if (prueba) {
              //  rb.AddForce(new Vector2(direccion * 20, 0f), ForceMode2D.Impulse);
                //Recoil(direccion, 1);
            //}
            triggerElementos_1_1_1(collider);
            StartCoroutine(cooldownRecibirDanio(direccion, 1));
            if (collider.transform.parent != null)
            {
                detectionTime = 0;
                collider.transform.parent.parent.GetComponent<Hoyustus>().cargaLanza();
                recibirDanio(collider.transform.parent.parent.GetComponent<Hoyustus>().getAtaque());
            }
            return;
        }
        else if (collider.gameObject.layer == 11)
        {
            //jugadorDetectado = true;
            rb.velocity = Vector2.zero;
            return;
        }

        if (!collider.name.Contains("Enemy") && collider.gameObject.layer != 3 && collider.gameObject.layer != 18)
        {
            triggerElementos_1_1_1(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.layer == 11)
        {
            objetivo = collider.transform.position;

            if (Vector3.Distance(transform.position, collider.transform.position) <= rangoDeteccion)
            {
                detectionTime += Time.deltaTime;
            }

            if (detectionTime >= cooldownAtaque && !atacando && playable) {
                detectionTime = 0;
                StartCoroutine(Ataque());
            }
        }
    }


    private bool Grounded() {
        if (Physics2D.OverlapCircle(groundDetector.position + Vector3.right * direction, 0.1f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    private void Falling()
    {
        rb.velocity -= Vector2.up * Time.deltaTime * -Physics2D.gravity * 4.5f;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            if (objetivo == limit1)
            {
                limit1 = transform.position + Vector3.right * 0.5f;
                objetivo = limit1;
                direction = 1;
            }
            else if (objetivo == limit2)
            {
                limit2 = transform.position - Vector3.right * 0.5f;
                objetivo = limit2;
                direction = -1;
            }

        }

        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 17)
        {//&& transform.position.y <= posY -1.5f) {
            posY = transform.position.y;
            limit1 = transform.GetChild(0).gameObject.transform.position;
            limit2 = transform.GetChild(1).gameObject.transform.position;
            objetivo = limit2;

            if (limit1.x >= limit2.x)
            {
                Vector3 aux = limit1;
                limit1 = limit2;
                limit2 = aux;
            }
        }

        if (!collision.gameObject.name.Contains("Enemy"))
        {
            collisionElementos_1_1_1(collision);
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            //rb.bodyType = RigidbodyType2D.Static;
            //prueba = true;
                 
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            //rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }

    }*/

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            //prueba = false;
            detectionTime = 0;
            posY = transform.position.y;
            limit1 = transform.GetChild(0).gameObject.transform.position;
            limit2 = transform.GetChild(1).gameObject.transform.position;
            objetivo = limit2;

            if (limit1.x >= limit2.x)
            {
                Vector3 aux = limit1;
                limit1 = limit2;
                limit2 = aux;
            }
        }
    }

    public bool detectarPiso()
    {
        if (!Physics2D.OverlapCircle(groundDetector.position, 0.2f, groundLayer))
        {
            if (direction == -1)
            {
                limit1 = transform.position + Vector3.right * 0.1f;
            }
            else if (direction == 1)
            {
                limit2 = transform.position - Vector3.right * 0.1f;
            }
            return false;
        }
        return true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            //jugadorDetectado = false;
            detectionTime = 0;
            if (transform.position.x > limit2.x) {
                objetivo = limit1;
            }
            if (transform.position.x < limit1.x)
            {
                objetivo = limit2;
            }
        }
    }

}
