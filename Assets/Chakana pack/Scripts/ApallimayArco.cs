using System.Collections;
using UnityEngine;


public class ApallimayArco : Apallimay
{
    [SerializeField] private float rangoAtaqueEspecial;
    [SerializeField] private float cooldownAtaqueEspecial;
    [SerializeField] private float cooldownDisparoFlechas;
    [SerializeField] private bool ataqueEspecialDisponible = true;
    [SerializeField] private GameObject flecha;
    [SerializeField] private bool atacando;
    [SerializeField] private Vector3 limit1;
    [SerializeField] private Vector3 limit2;
    [SerializeField] private bool jugadorDetectado;
    [SerializeField] private float direction = 1;
    [SerializeField] private float posY = 0;
    [SerializeField] private bool realizandoAtaqueEspecial = false;
    [SerializeField] private GameObject hoyustus;


    void Start()
    {
        explosionInvulnerable = "ExplosionEnemy";
        layerObject = transform.gameObject.layer;
        fuerzaRecoil = 2f;
        ataqueDisponible = true;
        rb = GetComponent<Rigidbody2D>();
        explosion = Resources.Load<GameObject>("Explosion");
        objetivo = limit2;
        limit1 = transform.GetChild(0).gameObject.transform.position;
        limit2 = transform.GetChild(1).gameObject.transform.position;
        posY = transform.position.y;
        groundDetector = transform.GetChild(3).gameObject.transform;
        vidaMax = vida;
        hoyustus = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {

        Muerte();
        if (Grounded()) {
            Flip();
            detectarPiso();
            if (!jugadorDetectado && playable)
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
        }
        else if (transform.position.x >= limit2.x)
        {
            objetivo = limit1;
        }
    }

    private void Flip()
    {
        if (transform.position.x < objetivo.x) direction = 1;
        else if (transform.position.x > objetivo.x) direction = -1;

        transform.localScale = new Vector3(direction, 1, 0);
    }


    private IEnumerator Ataque(Vector3 objetivoAtaque) {
        //ROTAR SPRITE
        GameObject flechaGenerada = Instantiate(flecha, transform.position, Quaternion.identity);//.name += "Enemy";
        flechaGenerada.transform.Rotate(new Vector3(0, 0f, Vector3.Angle(objetivoAtaque - transform.position, transform.right)));
        flechaGenerada.name += "Enemy";
        flechaGenerada.GetComponent<ProyectilMovUniforme>().setDanio(ataque);
        atacando = true;
        //TIEMPO DE ANIMACION/PREPARACION
        yield return new WaitForSeconds(0.4f);
        atacando = false;
        yield return new WaitForSeconds(cooldownDisparoFlechas);
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

            triggerElementos_1_1_1(collider);
            StartCoroutine(cooldownRecibirDanio(direccion, 1));
            if (collider.transform.parent != null)
            {
                collider.transform.parent.parent.GetComponent<Hoyustus>().cargaLanza();
                recibirDanio(collider.transform.parent.parent.GetComponent<Hoyustus>().getAtaque());
            }
            return;
        }
        else if (collider.gameObject.layer == 11)
        {
            distanciaPlayer = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawLine(transform.position, collider.transform.position, Color.red);
            if (!Physics2D.Raycast(transform.position, orientacionDeteccionPlayer(collider.transform.position), distanciaPlayer, wallLayer))
            {
                jugadorDetectado = true;
                rb.velocity = Vector2.zero;
            }
            else {
                jugadorDetectado = false;
            }
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
            distanciaPlayer = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawLine(transform.position, collider.transform.position, Color.red);
            
            if (!Physics2D.Raycast(transform.position, orientacionDeteccionPlayer(collider.transform.position), distanciaPlayer, wallLayer))
            {
                jugadorDetectado = true;
                rb.velocity = Vector3.zero;
                if (collider.transform.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }


                if (distanciaPlayer <= rangoAtaqueEspecial && ataqueEspecialDisponible)
                {
                    atacando = true;
                    realizandoAtaqueEspecial = true;
                    StartCoroutine(AtaqueEspecial());
                }
                else if (ataqueDisponible && !realizandoAtaqueEspecial)
                {
                    atacando = true;
                    ataqueDisponible = false;
                    StartCoroutine(Ataque(collider.transform.position));
                }
            }
            else
            {
                jugadorDetectado = false;
            }
        }
    }


    private IEnumerator AtaqueEspecial() {
        playable = false;
        realizandoAtaqueEspecial = true;
        ataqueEspecialDisponible = false;
        yield return new WaitForSeconds(1);
        explosion.GetComponent<ExplosionBehaviour>().modificarValores(15, 1, 15, 12, "Untagged", explosionInvulnerable);
        Instantiate(explosion, transform.position, Quaternion.identity);
        //SE ESPERA HASTA QUE SE GENERE ESTA EXPLOSION
        yield return new WaitForSeconds(0.4f);
        realizandoAtaqueEspecial = false;
        atacando = false;
        playable = true;
        yield return new WaitForSeconds(cooldownAtaqueEspecial);
        ataqueEspecialDisponible = true;
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
        {
            posY = transform.position.y;
            limit1 = transform.GetChild(0).gameObject.transform.position;
            limit2 = transform.GetChild(1).gameObject.transform.position;
            objetivo = limit2;

            if (limit1.x >= limit2.x) {
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
        if (collision.gameObject.layer == 11) jugadorDetectado = false;
    }
}