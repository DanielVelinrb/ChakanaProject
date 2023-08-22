using System.Collections;
using UnityEngine;

public class ApallimayEscudo : Apallimay
{
    [SerializeField] private bool atacando;
    [SerializeField] private bool siguiendo;
    [SerializeField] private Vector3 limit1;
    [SerializeField] private Vector3 limit2;
    [SerializeField] private float direction = 1;
    [SerializeField] private float posY = 0;
    [SerializeField] private BoxCollider2D daga;
    [SerializeField] private float cooldownAtaque;
    [SerializeField] private float t1;
    [SerializeField] private float t2;
    [SerializeField] private GameObject escudo;


    void Start()
    {
        explosionInvulnerable = "ExplosionEnemy";
        layerObject = transform.gameObject.layer;
        fuerzaRecoil = 2f;
        ataqueDisponible = true;
        rb = GetComponent<Rigidbody2D>();
        explosion = Resources.Load<GameObject>("Explosion");
        daga = transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>();
        limit1 = transform.GetChild(0).gameObject.transform.position;
        limit2 = transform.GetChild(1).gameObject.transform.position;
        objetivo = limit2;
        posY = transform.position.y;
        groundDetector = transform.GetChild(4).gameObject.transform;
        escudo = transform.GetChild(5).gameObject;
        vidaMax = vida;
        rangoVision += 1;
        rangoPreparacion += 1;
        rangoAtaque += 1;
        escudo.SetActive(false);
    }


    void Update()
    {
        Muerte();
        if (Physics2D.OverlapArea(wallDetector.position + Vector3.up * 0.5f + Vector3.right * transform.localScale.x * 0.3f,
            wallDetector.position + Vector3.down * 0.5f, wallLayer) && playable)
            DetectarPared();
        if (Grounded() && !atacando)
        {
            DetectarPiso(distanciaPlayer, siguiendo);
            if (CambioOrientacionDisponible(0.6f) && siguiendo)
                Flip();
            else if (CambioOrientacionDisponible(0.1f) && !siguiendo)
                Flip();
            if (playable)
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

    private void Move()
    {
        rb.velocity = new Vector2(direction * speed * (1 - afectacionViento), rb.velocity.y);

        if (transform.position.x <= limit1.x) objetivo = limit2;
        else if (transform.position.x >= limit2.x) objetivo = limit1;
    }

    private void Flip()
    {
        if (transform.position.x < objetivo.x)
        {
            direction = 1;
            objetivo = limit2;
        }
        else if (transform.position.x > objetivo.x)
        {
            direction = -1;
            objetivo = limit1;
        }
        transform.localScale = new Vector3(direction, 1, 0);
    }


    private IEnumerator Ataque(float direccionAtaque)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        playable = false;
        ataqueDisponible = false;
        //PREPARACION
        escudo.SetActive(false);
        yield return new WaitForSeconds(t1);
        atacando = true;
        //rb.AddForce(new Vector2(direccionAtaque * 12f, 0f), ForceMode2D.Impulse);
        daga.enabled = true;
        //ATAQUE
        yield return new WaitForSeconds(0.4f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        daga.enabled = false;
        //DESCANSO DEL ATAQUE
        yield return new WaitForSeconds(t2);
        escudo.SetActive(true);
        playable = true;
        atacando = false;
        //ATAQUE DISPONIBLE NUEVAMENTE
        yield return new WaitForSeconds(cooldownAtaque);
        ataqueDisponible = true;
    }

    protected override void Recoil(int direccion, float fuerzaRecoil)
    {
        playable = false; //EL OBJECT ESTARIA SIENDO ATACADO Y NO PODRIA ATACAR-MOVERSE COMO DE COSTUMBRE
        rb.AddForce(new Vector2(direccion * 2, rb.gravityScale * 2), ForceMode2D.Impulse);
    }


    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.gameObject.layer == 14 && !escudo.activeInHierarchy)
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

            TriggerElementos_1_1_1(collider);
            StartCoroutine(cooldownRecibirDanio(direccion, 1));
            if (collider.transform.parent != null)
            {
                collider.transform.parent.parent.GetComponent<Hoyustus>().cargaLanza();
                RecibirDanio(collider.transform.parent.parent.GetComponent<Hoyustus>().getAtaque());
            }
            return;
        }
        else if (collider.gameObject.layer == 11)
        {
            //rb.velocity = Vector2.zero;
            return;
        }

        if (!collider.name.Contains("Enemy") && collider.gameObject.layer != 3 && collider.gameObject.layer != 18)
        {
            TriggerElementos_1_1_1(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.layer == 11)
        {
            float orientacionDeteccion = OrientacionDeteccionPlayer(collider.transform.position.x);
            distanciaPlayer = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawLine(transform.position, collider.transform.position, Color.red);
            if (!Physics2D.Raycast(transform.position, transform.right * orientacionDeteccion, distanciaPlayer, wallLayer) && /*detectarPiso(distanciaPlayer, true) &&*/ Grounded())
            {
                if(playable)
                    escudo.SetActive(true);
                objetivo = collider.transform.position;
                siguiendo = true;
                if (distanciaPlayer <= rangoPreparacion && ataqueDisponible && !atacando && playable)
                {
                    StartCoroutine(Ataque(direction));
                }
            }
            else {
                escudo.SetActive(false);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((collision.gameObject.layer == 6 || collision.gameObject.layer == 17) && transform.position.y - 3 < posY)
        {
            posY = transform.position.y;
            limit1 = transform.position + Vector3.left * 5.5f;
            limit2 = transform.position + Vector3.right * 5.5f;
            objetivo = limit2;

            if (limit1.x >= limit2.x)
            {
                Vector3 aux = limit1;
                limit1 = limit2;
                limit2 = aux;
            }
        }
        else if (!collision.gameObject.name.Contains("Enemy"))
        {
            CollisionElementos_1_1_1(collision);
        }
    }

    public bool DetectarPiso(float option = 0, bool detectandoPlayer = false)
    {
        if (!Physics2D.OverlapCircle(groundDetector.position, 0.2f, groundLayer))
        {
            speed = 0;
            if (!detectandoPlayer)
            {
                if (direction == -1)
                {
                    limit1 = transform.position + Vector3.right * 0.1f;
                    limit2 = limit1 + Vector3.right * 11;
                }
                else if (direction == 1)
                {
                    limit2 = transform.position - Vector3.right * 0.1f;
                    limit1 = limit2 - Vector3.right * 11;
                }
                transform.localScale = new Vector3(-transform.localScale.x, 1, 0);
            }
            return false;
        }
        speed = 4;
        return true;
    }


    public void DetectarPared()
    {

        if (transform.localScale.x == -1)
        {
            limit1 = transform.position + Vector3.right * 0.5f;
            objetivo = limit1;
            limit2 = limit1 + Vector3.right * 11;
            direction = 1;
        }
        else
        {
            limit2 = transform.position - Vector3.right * 0.5f;
            objetivo = limit2;
            limit1 = limit2 - Vector3.right * 11;
            direction = -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            escudo.SetActive(false);
            distanciaPlayer = 0;
            siguiendo = false;
            speed = 4;
            if (transform.position.x > limit2.x)
            {
                objetivo = limit1;
            }
            else if (transform.position.x < limit1.x)
            {
                objetivo = limit2;
            }
        }
    }
}
