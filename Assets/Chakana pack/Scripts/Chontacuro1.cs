using UnityEngine;


public class Chontacuro1 : Enemy
{
    [SerializeField] public float movementSpeed;
    [SerializeField] public float seguimientoSpeed;

    [SerializeField] private float direction = 1;
    [SerializeField] private bool siguiendo = false;

    [SerializeField] private Vector3 limit1, limit2;
    [SerializeField] private float posY;

    [SerializeField] Transform groundDetector;
    [SerializeField] Transform wallDetector;
    [SerializeField] AudioClip audioHurt;

    AudioSource charAudio;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        charAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        fuerzaRecoil = 1;
        explosionInvulnerable = "ExplosionEnemy";
        speed = movementSpeed;

        limit1 = transform.GetChild(0).gameObject.transform.position;
        limit2 = transform.GetChild(1).gameObject.transform.position;
        objetivo = limit1;
        layerObject = transform.gameObject.layer;
        posY = transform.localPosition.y;
    }

    private void Falling() {
        rb.velocity -= Vector2.up * Time.deltaTime * -Physics2D.gravity * 4.5f;
    }

    private void Muerte() {
        Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


    private void FixedUpdate()
    {
        if (playable)
        {
            DetectarPiso();

            if (Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, 0.2f, groundLayer)) {
                if (CambioOrientacionDisponible(0.8f) && siguiendo)
                    Flip();
                else if (CambioOrientacionDisponible(0.2f) && !siguiendo)
                    Flip();
            }
        }
        if (Physics2D.OverlapArea(wallDetector.position + Vector3.up * 0.5f + Vector3.right * transform.localScale.x * 0.2f,
            wallDetector.position + Vector3.down * 0.5f, wallLayer) && playable)
            DetectarPared();

        if (playable)
            Move();            

        if (rb.velocity.y < 0)
            Falling();

        if (vida <= 0)
            Muerte();
    }


    private void DetectarPared() {
        if (transform.localScale.x == -1)
        {
            limit1 = transform.position + Vector3.right * 0.1f;
            objetivo = limit1;
            direction = 1;
        }
        else
        {
            limit2 = transform.position - Vector3.right * 0.1f;
            objetivo = limit2;
            direction = -1;
        }
        Flip();

        if (siguiendo)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            speed = 0;
        }
        else
        {
            speed = movementSpeed;
        }
    }


    private void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.DrawLine(transform.position, collider.transform.position, Color.red);
            if (!DetectarPiso() && !Physics2D.Raycast(transform.position, transform.right * OrientacionDeteccionPlayer(collider.transform.position.x),
                Vector3.Distance(transform.position, collider.transform.position), wallLayer))
            {
                if (collider.gameObject.transform.position.x < transform.position.x)
                {
                    objetivo = limit1;
                }
                else { 
                    objetivo = limit2;
                }
                speed = 0;
            }
            else if (!Physics2D.Raycast(transform.position, transform.right * OrientacionDeteccionPlayer(collider.transform.position.x),
                Vector3.Distance(transform.position, collider.transform.position), wallLayer))
            {
                speed = seguimientoSpeed;
                siguiendo = true;
                objetivo = collider.transform.position;
            }
            else if (Physics2D.Raycast(transform.position, transform.right * OrientacionDeteccionPlayer(collider.transform.position.x),
                Vector3.Distance(transform.position, collider.transform.position), wallLayer)) {
                siguiendo = false;
                speed = movementSpeed;
            }
        }
    }

    public bool DetectarPiso(int option = 0) {
        if (!Physics2D.OverlapCircle(groundDetector.position, 0.2f, groundLayer)) {
            if (option == 0)
            {
                if (direction == -1)
                {
                    limit1 = transform.position + Vector3.right * 0.1f;
                }
                else if (direction == 1)
                {
                    limit2 = transform.position - Vector3.right * 0.1f;
                }
            }
            return false;
        }
        return true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player")) {
            siguiendo = false;
            speed = movementSpeed;
            if (transform.position.x >= collider.transform.position.x) {
                if (transform.position.x < limit1.x)
                {
                    objetivo = limit1;
                }
                else {
                    objetivo = limit2;
                }
            }
            else if (transform.position.x < collider.transform.position.x) {
                objetivo = limit2;
                if (transform.position.x > limit2.x)
                {
                    objetivo = limit2;
                }
                else
                {
                    objetivo = limit1;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 6 || collision.gameObject.layer == 17) && transform.position.y - 1 < posY)
        {
            speed = movementSpeed;
            posY = transform.position.y;
            limit1 = transform.position + Vector3.left * 5.5f;
            limit2 = transform.position + Vector3.right * 5.5f;
            if(!siguiendo)
                objetivo = limit2;

            if (limit1.x >= limit2.x)
            {
                Vector3 aux = limit1;
                limit1 = limit2;
                limit2 = aux;
            }
        }
    }


    protected override void Recoil(int direccion, float fuerzaRecoil)
    {
        playable = false; //EL OBJECT ESTARIA SIENDO ATACADO Y NO PODRIA ATACAR-MOVERSE COMO DE COSTUMBRE
        rb.AddForce(new Vector2(-direccion * 10, rb.gravityScale * 4), ForceMode2D.Impulse);
    }


    private void Move()
    {
        rb.velocity = new Vector2(direction * speed * (1 - afectacionViento), rb.velocity.y);

        if (!siguiendo) { 
            if (transform.position.x <= limit1.x) objetivo = limit2;
            else if (transform.position.x >= limit2.x) objetivo = limit1;
        }
    }

    private void Flip() {
        if (transform.position.x < objetivo.x) direction = 1;
        else if (transform.position.x > objetivo.x) direction = -1;

        transform.localScale = new Vector3(direction, 1, 0);
    }


    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.gameObject.layer == 14 && playable)
        {
            StartCoroutine(cooldownRecibirDanio((int)Mathf.Sign(collider.transform.position.x - transform.position.x), 1));
            if (collider.transform.parent != null) {
                collider.transform.parent.parent.GetComponent<Hoyustus>().cargaLanza();
                RecibirDanio(collider.transform.parent.parent.GetComponent<Hoyustus>().getAtaque());
            }
            charAudio.loop = false;
            charAudio.Stop();
            charAudio.clip = audioHurt;
            charAudio.Play();
        }

        if (collider.gameObject.CompareTag("Viento") && !collider.gameObject.name.Contains("Enemy"))
        {
            if (estadoViento) StopCoroutine("afectacionEstadoViento");
            else if (counterEstados > 0)
            {
                counterEstados += 1;
                this.CombinacionesElementales();
                return;
            }
            estadoViento = true;
            counterEstados = 1;
            StartCoroutine("afectacionEstadoViento");
        }
        else if (collider.gameObject.CompareTag("Fuego") && !collider.gameObject.name.Contains("Enemy"))
        {
            if (estadoFuego) StopCoroutine("afectacionEstadoFuego");
            else if (counterEstados > 0)
            {
                counterEstados += 10;
                CombinacionesElementales();
                return;
            }
            estadoFuego = true;
            counterEstados = 10;
            StartCoroutine("afectacionEstadoFuego");
        }
    }


    private void CombinacionesElementales()
    {
        if (counterEstados == 11)
        {
             if (combObj01 == null) combObj01 = Instantiate(combFX01, transform.position, Quaternion.identity, transform);

            estadoViento = false;
            afectacionViento = 0;
            counterEstados = 10;
            aumentoFuegoPotenciado = 3;
            ataque = ataqueMax * 0.75f;
            StopCoroutine("afectacionEstadoFuego");
            estadoFuego = true;
            StartCoroutine("afectacionEstadoFuego");
        }
    }
}