using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Hoyustus : CharactersBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float walkSpeedGround = 9f;
    [SerializeField] float resistenciaAire = 0.3f;
    [SerializeField] float walkSpeed = 12f;
    [SerializeField] private bool isWalking = false;
    [Space(5)]

    [Header("Salto")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isSecondJump = false;
    [SerializeField] private float correctorSalto = 19;
    [SerializeField] private bool firstJump = true;
    [SerializeField] private bool secondJump = false;
    [SerializeField] private bool saltoEspecial = false;
    [SerializeField] private float extraSalto = 10;
    [Space(5)]

    [Header("Ground Checking")]
    [SerializeField] public float groundCheckRadius;
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask platformLayer;
    [Space(5)]

    [SerializeField] Animator anim;

    [Header("Audio")]
    AudioSource playerAudio;
    [SerializeField] AudioClip AudioWalking;
    [SerializeField] AudioClip AudioJump;
    [SerializeField] AudioClip AudioHurt;
    [SerializeField] AudioClip AudioSkill01;
    [SerializeField] AudioClip AudioSkill02;
    [SerializeField] AudioClip AudioSkill03;

    [SerializeField] AudioSource AudioStep1;
    [SerializeField] AudioSource AudioStep2;
    [SerializeField] AudioSource AudioStep3;
    [SerializeField] AudioSource AudioStep4;
    [SerializeField] AudioSource AudioStep5;
    [SerializeField] AudioSource AudioStep6;
    [SerializeField] AudioSource AudioStep7;
    [SerializeField] AudioSource AudioStep8;

    [SerializeField] AudioSource GameplayIntro;
    [SerializeField] AudioSource GameplayLoop;

    [SerializeField] ParticleSystem ParticleTestParticleTest = null;

    [SerializeField] private GameObject menuMuerte;

    //TESTING DE ESCENAS
    [SerializeField] private GameObject controladorTesting;
    [SerializeField] private GameObject pantallaCanvas;

    [Header("Variables Player")]
    [SerializeField] private float maxVida = 1000;
    [SerializeField] private float tiempoInvulnerabilidad = 2f;
    [SerializeField] private float timeAir = 1.2f;
    [SerializeField] private float currentTimeAir = 0f;
    [Space(5)]


    [Header("Estados Player")]
    [SerializeField] private int tocandoPared = 1;
    [Space(5)]


    [Header("Dash")]
    [SerializeField] private float timeDashCooldown = 0.6f;
    [SerializeField] private bool dashAvailable = true;
    [SerializeField] private bool isDashing = false;
    [Space(5)]


    [Header("Ataque")]
    [SerializeField] private bool atacando = false;
    [SerializeField] private int codigoAtaque = 0;
    [SerializeField] private float tiempoCooldownAtaque = 0.2f;
    [SerializeField] private bool ataqueAvailable = true;
    [SerializeField] private GameObject[] lanzas;
    [SerializeField] private float valorAtaqueNormal = 50;
    [SerializeField] private float valorAtaqueHabilidadCondor = 100;
    [SerializeField] private float valorAtaqueHabilidadLanza = 150;
    [Space(5)]


    [Header("Habilidades")]
    [SerializeField] float cargaHabilidadCondor;
    [SerializeField] float cargaHabilidadSerpiente;
    [SerializeField] float cargaHabilidadLanza;
    [SerializeField] float cargaCuracion;
    [SerializeField] float aumentoBarraSalto = 10;
    [SerializeField] float aumentoBarraDash = 15;
    [SerializeField] float aumentoBarraAtaque = 15;
    [SerializeField] float danioExplosionCombinacionFuego_Veneno = 35;
    [Space(5)]


    [Header("PREFABS")]
    [SerializeField] private GameObject bolaVeneno;
    [SerializeField] private Transform wallPoint;

    [SerializeField] private float botonCuracion = 0f;
    [SerializeField] private bool aplastarBotonCuracion = false;

    [SerializeField] private bool realizandoHabilidadLanza = false;

    [SerializeField] private bool curando = false;

    [SerializeField] private int SSTEPS = 60;
    [SerializeField] private int CSTEPS = 0;
    private float maxHabilidad_Curacion = 100f;

    float limitY = 0f;

    [SerializeField] GameObject dashVfx;

    [SerializeField] GameObject skillObj01;
    [SerializeField] GameObject skillObj02;
    [SerializeField] GameObject skillObj03;
    [SerializeField] GameObject skillObj04;


    public void isTocandoPared(int value)
    {
        tocandoPared = value;
    }

    public float getCargaHabilidadCondor()
    {
        return cargaHabilidadCondor;
    }

    public float getCargaHabilidadSerpiente()
    {
        return cargaHabilidadSerpiente;
    }

    public float getCargaHabilidadLanza()
    {
        return cargaHabilidadLanza;
    }

    public float getCargaCuracion()
    {
        return cargaCuracion;
    }


    public void setCargaCuracion(int e)
    {
        cargaCuracion += e;
    }

    public void CurarCompletamente()
    {
        vida = maxVida;
    }

    private void Awake()
    {

        LoadData();
    }


    private void LoadData()
    {
        if (!PlayerPrefs.HasKey("iniciado"))
        {
            PlayerPrefs.SetInt("iniciado", 1);
            SaveManager.SavePlayerData(GetComponent<Hoyustus>());
        }

        PlayerData playerData = SaveManager.LoadPlayerData();
        if (playerData != null)
        {
            gold = playerData.getGold();
            ataque = playerData.getAtaque();
            vida = playerData.getVida();
            if (playerData.getVida() <= 0) vida = maxVida;
            cargaHabilidadCondor = playerData.getCondor();
            cargaHabilidadSerpiente = playerData.getSerpiente();
            cargaHabilidadLanza = playerData.getLanza();
            cargaCuracion = playerData.getCuracion();
        }
        else
        {
            SaveManager.SavePlayerData(GetComponent<Hoyustus>());
        }
    }

    public void SavePlayerData()
    {
        SaveManager.SavePlayerData(vida, gold, cargaHabilidadCondor, cargaHabilidadSerpiente, cargaHabilidadLanza, cargaCuracion, ataque);
    }


    void Start()
    {
        //ESTABLECER FRAME RATE
        Application.targetFrameRate = 90;

        limitY = transform.position.y + 2;

        //IGNORAR COLISIONES A LO LARGO DE LA ESCENA --> DEBERIA IR EN UN GAMEMANAGER OBJECT
        Physics2D.IgnoreLayerCollision(11, 14, true);
        Physics2D.IgnoreLayerCollision(13, 12, true);
        Physics2D.IgnoreLayerCollision(13, 14, true);
        Physics2D.IgnoreLayerCollision(13, 15, true);

        //INICIALIZACION VARIABLES 
        invulnerable = false;
        explosionInvulnerable = "ExplosionPlayer";
        layerObject = this.gameObject.layer;
        aumentoDanioParalizacion = 1f;
        lanzas = new GameObject[transform.GetChild(this.transform.childCount - 1).childCount];
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        ataque = ataqueMax;
        ataqueMax = ataque;

        //INICIALIZACION DE LOS GAMEOBJECTS DE LAS LANZAS
        for (int i = 0; i < lanzas.Length; i++)
        {
            lanzas[i] = transform.GetChild(this.transform.childCount - 1).GetChild(i).gameObject;
        }

        playerAudio = GetComponent<AudioSource>();

        //Lee datos de memoria

        //CARGA DE PREFABS
        explosion = Resources.Load<GameObject>("Explosion");
        bolaVeneno = Resources.Load<GameObject>("BolaVeneno");


        SSTEPS = 65;

        //TESTING PARA CAMBIO DE NIVEL
        try {
            pantallaCanvas = GameObject.Find("-----CANVAS");
            Instantiate(controladorTesting, pantallaCanvas.transform.position,
            pantallaCanvas.transform.rotation).transform.SetParent(pantallaCanvas.transform);
        }
        catch (Exception) {
            Debug.Log("Corregir nombre del Objeto padre del canvas para las pruebas");
        }
    }


    void Update()
    {
        TocarPared();

        if (Mathf.Abs(rb.velocity.y) < 0.1f)
            Grounded();

        if (transform.parent != null)
        {
            limitY = transform.position.y + extraSalto;
        }

        if (botonCuracion >= 0.3f)
        {
            botonCuracion = 0f;
            aplastarBotonCuracion = false;
        }

        if (aplastarBotonCuracion && playable)
        {
            botonCuracion += Time.deltaTime;

            if (!curando && Input.GetButton("Jump") && cargaHabilidadCondor >= maxHabilidad_Curacion)
            {
                StartCoroutine("habilidadCondor");
                return;
            }
            if (!curando && Input.GetButton("Dash") && cargaHabilidadSerpiente >= maxHabilidad_Curacion)
            {
                StartCoroutine("habilidadSerpiente");
                return;
            }
            if (!curando && Input.GetButton("Atacar") && cargaHabilidadLanza >= maxHabilidad_Curacion)
            {
                StartCoroutine("habilidadLanza");
                return;
            }
        }

        if (Input.GetButtonDown("Activador_Habilidades") && playable)
        {
            aplastarBotonCuracion = true;
            //ACTIVACION DE LA CURACION
            if (cargaCuracion >= maxHabilidad_Curacion && aplastarBotonCuracion && botonCuracion > 0f && botonCuracion < 0.3f)
            {
                curando = true;
                cargaCuracion = 0;
                playable = false;
                aplastarBotonCuracion = false;
                botonCuracion = 0f;
                StartCoroutine("Curacion");
                return;
            }
            return;
        }

        if (playable)
        {
            Falling();
            AtaqueLanza();
            Dash();
        }
    }

    //***************************************************************************************************
    //DETECCION SUELO
    //***************************************************************************************************
    public bool Grounded()
    {

        if (Physics2D.OverlapCircle(groundTransform.position, groundCheckRadius, groundLayer) ||
            Physics2D.OverlapCircle(groundTransform.position, groundCheckRadius, platformLayer))
        {
            if (Physics2D.OverlapCircle(groundTransform.position, groundCheckRadius, platformLayer) && rb.velocity.y > 0.1f)
                return false;

            anim.SetBool("Grounded", true);
            firstJump = true;
            secondJump = false;
            walkSpeed = walkSpeedGround;
            CSTEPS = 0;
            //isJumping = false;
            isSecondJump = true;
            limitY = transform.position.y + extraSalto;
            return true;
        }
        else
        {
            anim.SetBool("Grounded", false);
            //isJumping = true;
            walkSpeed = walkSpeedGround * (1 - resistenciaAire);
            return false;

        }
    }


    private void jump()
    {
        if (Input.GetButtonUp("Jump") && CSTEPS < SSTEPS)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if (!atacando)
                anim.Play("Caer");

            if (firstJump)
            {
                secondJump = true;
                firstJump = false;
                isSecondJump = true;
            }
            else if (isSecondJump)
            {
                secondJump = false;
                isSecondJump = false;
            }
            isJumping = false;
            CSTEPS = 0;
            return;

        }

        if (firstJump && !isTouchingRoof() && CSTEPS < SSTEPS)
        {
            isSecondJump = false;
            if (Input.GetButtonUp("Jump") ||  CSTEPS >= SSTEPS || transform.position.y >= limitY || isTouchingRoof())
            {
                anim.Play("Caer");
                secondJump = true;
                firstJump = false;
                isJumping = false;
                CSTEPS = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                return;
            }

            if (Input.GetButtonDown("Jump") && Grounded())
            {
                playerAudio.Stop();
                playerAudio.loop = false;
                playerAudio.clip = AudioJump;
                playerAudio.Play();

                anim.Play("Saltar");
                isJumping = true;
                secondJump = false;
                rb.AddForce(new Vector2(0, 12f), ForceMode2D.Impulse);
                cargaHabilidadCondor += aumentoBarraSalto;
                CSTEPS++;
                limitY = transform.position.y + extraSalto;
            }
            else if (Input.GetButton("Jump") && isJumping && transform.position.y < limitY)
            {
                rb.AddForce(new Vector2(0, ((6f + 0.5f * correctorSalto * ((SSTEPS - CSTEPS) * (SSTEPS - CSTEPS)) / 42) / (SSTEPS - CSTEPS) / 40)), ForceMode2D.Impulse);
                //rb.AddForce(new Vector2(0, (SSTEPS - CSTEPS) /150f), ForceMode2D.Impulse);
                CSTEPS++;
            }

        }
        //DOBLE SALTO
        else if (secondJump && !isTouchingRoof() && CSTEPS < SSTEPS)
        {
            if (Input.GetButtonDown("Jump") && CSTEPS == 0)
            {
                playerAudio.loop = false;
                playerAudio.Stop();
                playerAudio.clip = AudioJump;
                playerAudio.Play();

                anim.Play("Doble Salto");
                CSTEPS = 1;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, -rb.velocity.y + 18), ForceMode2D.Impulse);
                isJumping = true;
                secondJump = true;
                limitY = transform.position.y + extraSalto;
                cargaHabilidadCondor += aumentoBarraSalto;
                isSecondJump = true;
            }
            else if (Input.GetButton("Jump") && isJumping && transform.position.y < limitY && isSecondJump)
            {
                CSTEPS++;
                rb.AddForce(new Vector2(0, 1.15f - (limitY - transform.position.y) / 10), ForceMode2D.Impulse);
            }


            if ((Input.GetButtonUp("Jump") || transform.position.y >= limitY || CSTEPS > SSTEPS || isTouchingRoof()) && isSecondJump)
            {
                if (!atacando)
                    anim.Play("Caer");
                CSTEPS = 0;
                isJumping = false;
                secondJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                return;
            }

        }
    }

    private void LateUpdate()
    {
        if (playable)
        {
            jump();
        }
    }


    void FixedUpdate()
    {
        if (playable && !atacando)
        {
            Walk();
        }

        if (vida <= 0)
        {
            StartCoroutine(Muerte());
        }

        anim.SetBool("Walking", rb.velocity.x != 0);
        anim.SetBool("Grounded", Grounded());
        anim.SetFloat("YVelocity", rb.velocity.y);
        anim.SetFloat("XVelocity", rb.velocity.x);
        anim.SetFloat("Vida", vida);
        anim.SetFloat("Ataque", ataque);
        anim.SetInteger("Gold", gold);
        anim.SetBool("Dashing", isDashing);
        anim.SetBool("Atacando", atacando);
        anim.SetInteger("CA", codigoAtaque);
        anim.SetBool("SecondJump", secondJump);
        anim.SetBool("Jumping", isJumping);

    }


    protected override void Recoil(int direccion, float fuerzaRecoil)
    {
        playable = false;

        if (isJumping)
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 2f;
            rb.AddForce(new Vector2(direccion * 2.6f * fuerzaRecoil, rb.gravityScale), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(direccion * 4 * fuerzaRecoil, rb.gravityScale * 2), ForceMode2D.Impulse);
        }

        Physics2D.IgnoreLayerCollision(3, layerObject, true);
        Physics2D.IgnoreLayerCollision(layerObject, 19 , true);
        EstablecerInvulnerabilidades(layerObject);
    }


    protected override sealed void QuitarInvulnerabilidades(int layerObject)
    {
        invulnerable = false;
        Physics2D.IgnoreLayerCollision(3, layerObject, false);
        Physics2D.IgnoreLayerCollision(layerObject, 12, false);
        Physics2D.IgnoreLayerCollision(layerObject, 15, false);
        Physics2D.IgnoreLayerCollision(layerObject, 19, false);

    }


    //***************************************************************************************************
    //CURACION DEL PLAYER
    //***************************************************************************************************
    private IEnumerator Curacion()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(Instantiate(skillObj04, transform.position, Quaternion.identity), 1);
        //CAMBIO A LA ANIMACION
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playable = true;
        vida += 350;
        curando = false;
    }


    //***************************************************************************************************
    //HABILIDAD CONDOR
    //***************************************************************************************************
    private IEnumerator habilidadCondor()
    {
        anim.SetInteger("Skill", 2);
        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA HABILIDAD
        cargaHabilidadCondor = 0f;
        playable = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cargaCuracion += 30;

        //SE MODIFICA EL GAMEOBJECT DEL PREFAB EXPLOSION Y SE LO INSTANCIA
        explosion.GetComponent<ExplosionBehaviour>().modificarValores(15, valorAtaqueHabilidadCondor, 15, 12, "Viento", explosionInvulnerable);
        GameObject extraExplosion = Instantiate(explosion, transform.position + Vector3.up * 1f, Quaternion.identity);
        extraExplosion.name += "Player";

        yield return new WaitForSeconds(0.05f);
        anim.SetInteger("Skill", 0);
        yield return new WaitForSeconds(0.35f);
        Destroy(Instantiate(skillObj02, transform.position, Quaternion.identity), 1.2f);
        //SE ESPERA HASTA QUE SE GENERE ESTA EXPLOSION
        yield return new WaitForSeconds(0.8f);
        //SE VUELVEN A ESTABLECER LOS VALORES DE JUEGO NORMAL
        playable = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    private IEnumerator habilidadSerpiente()
    {
        anim.SetInteger("Skill", 1);
        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA HABILIDAD
        playable = false;
        dashAvailable = false;
        cargaHabilidadSerpiente = 0f;
        cargaCuracion += 30;
        yield return new WaitForSeconds(0.05f);
        anim.SetInteger("Skill", 0);
        yield return new WaitForSeconds(0.25f);
        //SE GENERA OTRO OBJETO A PARTIR DEL PREFAB BOLAVENENO Y SE LO MODIFICA
        GameObject bolaVenenoGenerada = Instantiate(bolaVeneno, transform.position + Vector3.up, Quaternion.identity);
        yield return new WaitForEndOfFrame();
        bolaVenenoGenerada.GetComponent<BolaVeneno>().AniadirFuerza(-transform.localScale.x, 11);
        yield return new WaitForEndOfFrame();
        //SE VUELVEN A ESTABLECER LOS VALORES DE JUEGO NORMAL
        dashAvailable = true;
        playable = true;
        vidaMax = 1000;
    }


    private IEnumerator habilidadLanza()
    {
        anim.SetInteger("Skill", 3);

        Physics2D.IgnoreLayerCollision(3, layerObject, true);
        Physics2D.IgnoreLayerCollision(layerObject, 19, true);
        EstablecerInvulnerabilidades(layerObject);
        realizandoHabilidadLanza = true;
        playable = false;

        invulnerable = true;
        cargaCuracion += 30;

        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA HABILIDAD
        atacando = true;
        codigoAtaque = 3;
        cargaHabilidadLanza = 0f;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        yield return new WaitForSeconds(0.05f);
        anim.SetInteger("Skill", 0);
        Destroy(Instantiate(skillObj01, transform.position, Quaternion.identity, transform), 1f);
        //ACTIVACION Y MODIFICACION DE LA LANZA
        lanzas[0].tag = "Fuego";
        ataque = valorAtaqueHabilidadLanza;
        ataque = 150;
        lanzas[0].SetActive(true);


        IEnumerator movimientoHabilidadLanza()
        {
            rb.AddForce(new Vector2(transform.localScale.x * 30, 0), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);
            rb.velocity = Vector2.zero;
            realizandoHabilidadLanza = false;
            //isDashing = false;
        }
        StartCoroutine(movimientoHabilidadLanza());
        yield return new WaitUntil(() => (tocandoPared == 0 || !realizandoHabilidadLanza));

        anim.SetInteger("Skill", 0);

        atacando = false;
        codigoAtaque = 0;

        //SE VUELVEN A ESTABLECER LOS VALORES DE JUEGO NORMAL
        QuitarInvulnerabilidades(layerObject);
        realizandoHabilidadLanza = false;
        playable = true;
        rb.gravityScale = 2f;
        ataque = valorAtaqueNormal;
        ataque = ataqueMax;

        //DESACTIVACION Y MODIFICACION DE LA LANZA
        lanzas[0].SetActive(false);
        lanzas[0].tag = "Untagged";
        lanzas[0].layer = 14;
    }


    public void setPlayable(bool state)
    {
        playable = state;
    }

    public void setGold(int e)
    {
        gold += e;
    }

    public void setAumentoDanioParalizacion(float value)
    {
        aumentoDanioParalizacion = value;
    }


    //***************************************************************************************************
    //DETECCION DE COLISIONES
    //***************************************************************************************************
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //COLISIONES PARA OBJETOS TAGUEADOS COMO ENEMY
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 18 || collision.gameObject.layer == 19)
        {
            try
            {
                //DETECCION DE DEL CUERPO DEL ENEMIGO
                if (!invulnerable && collision.gameObject.transform.parent.name == "-----ENEMIES")
                {
                    //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VIENTO
                    invulnerable = true;
                    RecibirDanio(collision.gameObject.GetComponent<CharactersBehaviour>().getAtaque());
                    StartCoroutine(cooldownRecibirDanio((int)-Mathf.Sign(collision.transform.position.x - transform.position.x),
                        collision.gameObject.GetComponent<CharactersBehaviour>().fuerzaRecoil));
                    CollisionElementos_1_1_1(collision);
                    return;
                }

            }
            catch (Exception){}

            if (!invulnerable)
                CollisionElementos_1_1_1(collision);

        }
    }

    protected override sealed IEnumerator cooldownRecibirDanio(int direccion, float fuerzaRecoil)
    {
        Recoil(direccion, fuerzaRecoil);
        if (vida <= 0)
        {
            yield break;
        }

        playerAudio.loop = false;
        playerAudio.Stop();
        playerAudio.clip = AudioHurt;
        playerAudio.Play();

        //Aniadir el brillo (Mientras se lo tenga se lo simulara con el cambio de la tonalidad del sprite)
        yield return new WaitForSeconds(0.5f);
        if(!realizandoHabilidadLanza)
        {
            //SE DETIENE EL RECOIL
            rb.velocity = Vector2.zero;
            yield return new WaitForEndOfFrame();
            //EL OBJECT PUEDE VOLVER A MOVERSE SIN ESTAR EN ESTE ESTADO DE "SER ATACADO"
            playable = true;
        }
        yield return new WaitForSeconds(0.7f);
        QuitarInvulnerabilidades(layerObject);
        if(realizandoHabilidadLanza)
            invulnerable = true;
    }


    public bool IsInvulnerable() {
        return invulnerable;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 18 || collision.gameObject.layer == 19)
        {
            try
            {
                //DETECCION DEL CUERPO DEL ENEMIGO
                if (!invulnerable && collision.gameObject.transform.parent.name == "-----ENEMIES")
                {
                    RecibirDanio(collision.gameObject.GetComponent<CharactersBehaviour>().getAtaque());
                    StartCoroutine(cooldownRecibirDanio((int)-Mathf.Sign(collision.transform.position.x - transform.position.x),
                        collision.gameObject.GetComponent<CharactersBehaviour>().fuerzaRecoil));
                    CollisionElementos_1_1_1(collision);
                }

            }
            catch (Exception) { }
        }
    }
    //***************************************************************************************************
    //DETECCION DE TRIGGERS
    //***************************************************************************************************
    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO ENEMY
        if (collider.gameObject.layer == 3 || collider.gameObject.layer == 18 || collider.gameObject.layer == 19)
        {
            //direccion nos dara la orientacion de recoil al sufrir danio
            int direccion = 1;
            if (collider.transform.position.x > gameObject.transform.position.x)
            {
                direccion = -1;
            }
            else
            {
                direccion = 1;
            }

            try
            {
                //PROYECTILES
                if (collider.gameObject.transform.parent == null)
                {
                    TriggerElementos_1_1_1(collider);
                }
                //DETECCION DE OBJETOS HIJOS DEL ENEMIGO
                else if (!invulnerable && (collider.gameObject.transform.parent.parent.name == "-----ENEMIES" && (collider.gameObject.layer == 3 || collider.gameObject.layer == 19)))
                {
                    RecibirDanio(collider.gameObject.transform.parent.GetComponent<CharactersBehaviour>().getAtaque());
                    StartCoroutine(cooldownRecibirDanio(direccion, collider.gameObject.transform.parent.GetComponent<CharactersBehaviour>().fuerzaRecoil));
                    TriggerElementos_1_1_1(collider);
                }
                else if (collider.gameObject.transform.parent.parent.name == "-----ENEMIES" && collider.gameObject.layer == 18 && isDashing)
                {
                    Debug.Log("imposible dashear");
                    invulnerable = true;
                    RecibirDanio(collider.gameObject.transform.parent.GetComponent<CharactersBehaviour>().getAtaque());
                    StartCoroutine(cooldownRecibirDanio(direccion, collider.gameObject.transform.parent.GetComponent<CharactersBehaviour>().fuerzaRecoil));
                    TriggerElementos_1_1_1(collider);
                }
                return;

            }
            catch (Exception){}
        }
        if (!invulnerable && !collider.gameObject.name.Contains("Player"))
            TriggerElementos_1_1_1(collider);

    }


    private bool isTouchingRoof()
    {
        if (Physics2D.OverlapCircle(groundTransform.position + Vector3.up * 2.5f, groundCheckRadius, groundLayer))
        {
            return true;
        }
        return false;
    }


    private void TocarPared()
    {
        tocandoPared = (Physics2D.OverlapArea(wallPoint.position + Vector3.right * transform.localScale.x * 0.5f +
            Vector3.up * 1.25f, wallPoint.position + Vector3.right * transform.localScale.x * 0.1f - Vector3.up * 1.25f, wallLayer)) ? 0 : 1;

    }


    //***************************************************************************************************
    //CORRUTINA DE MUERTE
    //***************************************************************************************************
    private IEnumerator Muerte()
    {

        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA CORRUTINA
        playable = false;
        //Corregir los tiempos en relacion a la muerte por danio fisico y por estas afectaciones elementales
        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        this.gameObject.tag = "Untagged";
        this.gameObject.layer = 0;
        Physics2D.IgnoreLayerCollision(0, 3, true);
        //GUARDADO DE INFORMACION
        //gold = 100;
        //SaveManager.SavePlayerData(maxVida, gold, SceneManager.GetActiveScene().name);
        //Da inicio a la animacion
        //WaitForSeconds deberia tener el tiempo de la animacion para desplegar el menu
        yield return new WaitForSeconds(1f);
        //Desplegar el menu

        SavePlayerData();
        SaveManager.SavePlayerData(GetComponent<Hoyustus>());

        Time.timeScale = 0;
        menuMuerte.SetActive(true);
        //La correccion de las acciones tomadas al "revivir" se implementaran despues al contar con el resto de mecanicas implementadas
        //Es decir la posicion en el checkpoint, vida y gold
    }


    //***************************************************************************************************
    //MOVIMIENTO
    //***************************************************************************************************
    private void Walk()
    {
        float h = Input.GetAxis("Horizontal");

        if (h >= -0.10 && h <= 0.10)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isWalking = false;
            if (playerAudio.clip == AudioWalking) playerAudio.Stop();
            return;
        }
        else if (h < -0.10)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (h > 0.10)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        isWalking = true;
        if (isJumping)
        {
            if(playerAudio.clip == AudioWalking) playerAudio.Stop();
        }
        else if (!isJumping)
        {
            if (!playerAudio.isPlaying && Grounded())
            {
                playerAudio.loop = true;
                playerAudio.clip = AudioWalking;
                playerAudio.Play();
            }
        }

        rb.velocity = new Vector2(h * walkSpeed * (1 - afectacionViento) * tocandoPared, rb.velocity.y);
        if (rb.velocity == Vector2.zero) playerAudio.Stop();
    }


    //***************************************************************************************************
    //AUMENTO DE LA VELOCIDAD DE CAIDA DE LOS OBJETOS
    //***************************************************************************************************
    void Falling()
    {
        if (rb.velocity.y < 0) rb.velocity -= Vector2.up * Time.deltaTime * -Physics2D.gravity * 9f;
    }


    //***************************************************************************************************
    //Ataque Lanza
    //***************************************************************************************************
    private void AtaqueLanza()
    {

        if (ataqueAvailable && Input.GetButtonDown("Atacar") && playable)
        {
            atacando = true;
            //playable = false;
            int index = 0;  //SE REFIERE AL INDICE DE LOS HIJOS DEL OBJETO LANZA DE HOYUSTUS
            //VOLVERLAS VARIABLES GLOBALES
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v == 0)
            {
                //Aniadir el pequenio impulso de movimiento
                anim.Play("Lanza Lateral");
                codigoAtaque = 4;
            }
            else if (v != 0 && h == 0)
            {
                //VERIFIFICAR QUE SOLO FUNCIONE AL ESTAR EN EL AIRE Y AGREGAR LAS POSICIONES VERTICALES DE ATAQUE.
                if (v > 0)
                {
                    index = 1;
                    codigoAtaque = 5;
                }
                else if (v <= 0 && !Grounded())
                {
                    index = 2;
                    codigoAtaque = 6;
                }
                else if (v <= 0 && Grounded())
                {
                    anim.Play("Lanza Lateral");
                    codigoAtaque = 4;
                }
            }
            else if (v != 0 && h != 0)
            {
                //VERIFIFICAR QUE SOLO FUNCIONE AL ESTAR EN EL AIRE Y AGREGAR LAS POSICIONES VERTICALES DE ATAQUE.
                if (Math.Abs(v) > Math.Abs(h))
                {
                    if (v > 0)
                    {
                        index = 1;
                        codigoAtaque = 5;
                    }
                    else if (v <= 0 && !Grounded())
                    {
                        index = 2;
                        codigoAtaque = 6;
                    }
                    else if (v <= 0 && Grounded())
                    {
                        anim.Play("Lanza Lateral");
                        codigoAtaque = 4;
                    }
                }
                else
                {
                    //Aniadir el pequenio impulso de movimiento
                    //lanza.SetActive(true);
                    anim.Play("Lanza Lateral");
                    codigoAtaque = 4;
                }
            }

            ataqueAvailable = false;
            StartCoroutine(lanzaCooldown(index));
        }
    }


    //***************************************************************************************************
    //CooldownAtaque
    //***************************************************************************************************
    private IEnumerator lanzaCooldown(int index)
    {
        lanzas[index].SetActive(true);
        atacando = true;
        yield return new WaitForSeconds(0.2f);
        atacando = false;
        playable = true;
        codigoAtaque = 0;
        lanzas[index].SetActive(false);
        anim.Play("Idel");
        yield return new WaitForSeconds(tiempoCooldownAtaque);
        ataqueAvailable = true;
    }


    //***************************************************************************************************
    //DASH
    //***************************************************************************************************
    private void Dash()
    {
        if (dashAvailable && Input.GetButtonDown("Dash") && tocandoPared != 0)
        {
            transform.parent = null;
            invulnerable = true;
            playable = false;
            dashAvailable = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            StartCoroutine(dashCooldown());

        }
    }


    //***************************************************************************************************
    //COOLDOWN DASH
    //***************************************************************************************************
    private IEnumerator dashCooldown()
    {
        Destroy(Instantiate(dashVfx, transform.position, Quaternion.identity, transform), 0.5f);

        isDashing = true;
        Physics2D.IgnoreLayerCollision(3, layerObject, true);
        Physics2D.IgnoreLayerCollision(layerObject, 19, true);
        EstablecerInvulnerabilidades(layerObject);
        anim.Play("Dash");
        cargaHabilidadSerpiente += aumentoBarraDash;

        IEnumerator movimientoDash()
        {
            rb.AddForce(new Vector2(transform.localScale.x * 45, 0), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
            //rb.velocity = Vector2.zero;
            rb.gravityScale = 2;
            isDashing = false;
        }
        StartCoroutine(movimientoDash());
        yield return new WaitUntil(() => (tocandoPared == 0 || isDashing == false));
        rb.gravityScale = 2;
        isDashing = false;
        playable = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);
        QuitarInvulnerabilidades(layerObject);
        yield return new WaitForSeconds(timeDashCooldown);
        dashAvailable = true;
    }


    public void cargaLanza()
    {
        cargaHabilidadLanza += aumentoBarraAtaque;
    }


    public void ejecucionCorrutinaPrueba(int direccion, float fuerza) {
        if(!realizandoHabilidadLanza) 
            StartCoroutine(cooldownRecibirDanio(direccion, fuerza));
    }


    void PlayJumpAudio()
    {
        //AudioJump.Play();

    }
    void PlayFallAudio()
    {
        //AudioFall.Play();
    }

    public float getMaxVida()
    {
        return maxVida;
    }



    IEnumerator PlayGamePlayLoop()
    {
        yield return new WaitForSeconds(59.2f);
        GameplayIntro.Stop();
        GameplayLoop.Play();
    }


    void PlayAudioStep1()
    {
        AudioStep1.Play();
    }
    void PlayAudioStep2()
    {
        AudioStep2.Play();
    }
    void PlayAudioStep3()
    {
        AudioStep3.Play();
    }
    void PlayAudioStep4()
    {
        AudioStep4.Play();
    }
    void PlayAudioStep5()
    {
        AudioStep5.Play();
    }
    void PlayAudioStep6()
    {
        AudioStep6.Play();
    }
    void PlayAudioStep7()
    {
        AudioStep7.Play();
    }
    void PlayAudioStep8()
    {
        AudioStep8.Play();
    }
    public void PlayParticles()
    {
        ParticleTestParticleTest.Play();
    }

}