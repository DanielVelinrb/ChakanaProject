using System.Collections; //*
using System.Collections.Generic; //*
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine; //*
using System.Text.RegularExpressions;
using System;
using static System.Random;

public class Enemies : CharactersBehaviour
{
    public enum TipoEnemigo
    {
        Chontacuro,
        ApallimayDaga,
        ApallimayArco,
        ApallimayEscudo,
        Tzantza,
        Boraro,
        Mapianguari
    }

    [Header("Enemigo")]
    public TipoEnemigo tipoEnemigo;

    [Header("Patrullaje")]
    public GameObject puntoA;
    public GameObject puntoB;

    [Header("Perseguir Jugador")]
    //private Hoyustus player;
    //private Rigidbody2D rb; //Definida en la clase CharactersBehaviour
    private Animator anim;
    private Transform puntoActual;
    //Para MoverPatrullaje
    //Para MoverLibre
    public float velocidad=2.0f;
    private int cuadros = 0;
    private Vector3 target;

    [Header("Comportamiento")]
    public bool persiguiendo = true, atacando = false, cambiandoPlataforma;
    public float rangoAtaqueCuerpo;
    public float tiempoDentroRango, tiempoFueraRango;
    public float minX, maxX;
    public float xObjetivo;
    public bool ataqueDisponible;
    private BoxCollider2D ataqueCuerpo, campoVision;
    private CapsuleCollider2D cuerpo;
    private float movementVelocity = 4;
    //private float movementVelocitySecondStage = 8;
    private float maxVida;
    private bool segundaEtapa = false;

    [SerializeField] private GameObject bolaVeneno;
    [SerializeField] private GameObject explosion;

    [SerializeField] private GameObject plataformaUno;
    [SerializeField] private GameObject plataformaDos;
    [SerializeField] private GameObject plataformaTres;
    [SerializeField] public int nuevaPlataforma;
    [SerializeField] public int plataformaActual;
    private GameObject charcoVeneno;


    // Start is called before the first frame update
    void Start()
    {
        //Cargar variables iniciales
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        //*
        //anim.SetFloat("XVelocity", rb.velocity.x);


        puntoActual = puntoB.transform;
        //Para MoverLibre
        NewTarget();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hoyustus>();



        //C�digo de Mapianguari
        //Physics2D.IgnoreLayerCollision(13, 15, true);
        charcoVeneno = new GameObject();
        charcoVeneno.SetActive(false);
        charcoVeneno.tag = "Veneno";
        charcoVeneno.AddComponent<BoxCollider2D>();
        charcoVeneno.GetComponent<BoxCollider2D>().isTrigger = true;
        charcoVeneno.GetComponent<BoxCollider2D>().size = new Vector2(10f, 1f);

        plataformaActual = 0;
        nuevaPlataforma = 0;

        //INICIALIZACION VARIABLES
        explosionInvulnerable = "ExplosionEnemy";
        vida = 20;// 200;
        maxVida = vida;
        ataqueMax = 20;
        ataque = ataqueMax;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        counterEstados = 0;
        layerObject = transform.gameObject.layer;
        ataqueDisponible = true;
        cuerpo = transform.GetComponent<CapsuleCollider2D>();
        campoVision = transform.GetChild(0).GetComponent<BoxCollider2D>();
        ataqueCuerpo = transform.GetChild(1).GetComponent<BoxCollider2D>();

        ataqueCuerpo.enabled = false;
        layerObject = transform.gameObject.layer;

        //SE DESACTIVAN LAS COLISIONES DEL CUERPO DEL BOSS CON EL DASHBODY DE HOYUSTUS Y SU CUERPO ESTANDAR
        Physics2D.IgnoreCollision(cuerpo, GameObject.Find("Hoyustus Solicitud Prefab").GetComponent<CapsuleCollider2D>());
        Physics2D.IgnoreCollision(cuerpo, GameObject.Find("Hoyustus Solicitud Prefab").transform.GetChild(0).GetComponent<BoxCollider2D>());


        //CARGA DE PREFABS
        bolaVeneno = Resources.Load<GameObject>("BolaVeneno");
        explosion = Resources.Load<GameObject>("Explosion");

    }

    // Update is called once per frame
    void Update()
    {
        //Diferenciar el comportamiento seg�n el tipo de Enemigo
        switch (tipoEnemigo)
        {
            case TipoEnemigo.Chontacuro:
                MoverPatrullaje();
                break;
            case TipoEnemigo.ApallimayDaga:
                MoverPerseguirJugador();
                //MoverPatrullaje();
                if (RangoVision())
                {
                    MoverPerseguirJugador();
                }
                break;
            case TipoEnemigo.ApallimayArco:
                MoverPatrullaje();
                if (RangoAtaque())
                {
                    Atacar();
                }
                break;
            case TipoEnemigo.ApallimayEscudo:
                MoverPatrullaje();
                if (RangoVision())
                {
                    MoverPerseguirJugador();
                }
                break;
            case TipoEnemigo.Tzantza:
                MoverLibre();
                if (RangoVision())
                {
                    MoverPerseguirJugador();
                    if (RangoCercaJugador())
                    {
                        Atacar();
                    }
                }
                break;
            case TipoEnemigo.Boraro:
                MoverPerseguirJugador();
                //Al inicio el personaje se encuentra est�tico
                if (RangoVision())
                {
                    MoverPerseguirJugador();
                    if (RangoCercaJugador())
                    {
                        Atacar();
                    }
                }
                break;
            case TipoEnemigo.Mapianguari:
                MoverPerseguirJugador();
                break;
        }
    }


    //***************************************************************************************************
    //FIXED UPDATE - Mapianguari
    //***************************************************************************************************
    private void FixedUpdate()
    {
        if (vida <= maxVida / 2)
        {
            movementVelocity = 8;
            segundaEtapa = true;
        }
        if (vida <= 0)
        {
            StartCoroutine(Muerte());
        }
    }

    //***************************************************************************************************
    //CORRUTINA DE MUERTE
    //***************************************************************************************************
    private IEnumerator Muerte()
    {
        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA CORRUTINA
        campoVision.enabled = false;
        xObjetivo = transform.position.x;
        //TIEMPO ANIMACION DEL Boss
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        Debug.Log("Se supone que desaparece");
    }



    protected void MoverPatrullaje()
    {
        Vector2 point = puntoActual.position - transform.position;
        if (puntoActual == puntoB.transform)
        {
            rb.velocity = new Vector2(velocidad, 0);
        }
        else
        {
            rb.velocity = new Vector2(-velocidad, 0);
        }
        //if (Vector2.Distance(transform.position, puntoActual.position) < 0.5f && puntoActual == puntoB.transform)
        if (Mathf.Abs(transform.position.x - puntoActual.position.x) < 0.5f && puntoActual == puntoB.transform)
        {
            flip();
            puntoActual = puntoA.transform;
        }
        //if (Vector2.Distance(transform.position, puntoActual.position) < 0.5f && puntoActual == puntoA.transform)
        if (Mathf.Abs(transform.position.x - puntoActual.position.x) < 0.5f && puntoActual == puntoA.transform)
        {
            flip();
            puntoActual = puntoB.transform;
        }
    }
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(puntoA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(puntoB.transform.position, 0.5f);
        Gizmos.DrawLine(puntoA.transform.position, puntoB.transform.position);
    }


    protected void MoverPerseguirJugador()
    {
        if (nuevaPlataforma != plataformaActual)
        {
            plataformaActual = nuevaPlataforma;
            transform.position = new Vector3(transform.position.x, 5 + plataformaActual * 20, 0);
        }
        //MODIFICACION DE POSICION A SEGUIR AL PLAYER AL ESTAR EN LA MISMA PLATAFORMA
        if (xObjetivo >= minX && xObjetivo <= maxX && !atacando)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Vector3.right * xObjetivo, movementVelocity * (1 - afectacionViento) * Time.deltaTime);
        }

        /*transform.position = Vector3.MoveTowards(this.transform.position, Vector3.right * player.transform.position.x, velocidad * (1 - 0) * Time.deltaTime);

        //Cambio Orientacion
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xObjetivo > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }*/
    }

    

    //***************************************************************************************************
    //DETECCION DE TRIGGERS
    //***************************************************************************************************
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

            StartCoroutine(cooldownRecibirDanio(direccion, 1));
            if (collider.transform.parent != null)
            {
                collider.transform.parent.parent.GetComponent<Hoyustus>().cargaLanza();
                recibirDanio(collider.transform.parent.parent.GetComponent<Hoyustus>().getAtaque());
            }
        }

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VIENTO
        if (collider.gameObject.tag == "Viento")
        {
            if (estadoViento)
            {
                StopCoroutine("afectacionEstadoViento");
            }
            else if (counterEstados > 0)
            {
                counterEstados += 1;
                StartCoroutine("combinacionesElementales");
                return;

            }
            estadoViento = true;
            counterEstados = 1;
            StartCoroutine("afectacionEstadoViento");
        }
        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO FUEGO
        else if (collider.gameObject.tag == "Fuego")
        {
            if (estadoFuego)
            {
                StopCoroutine("afectacionEstadoFuego");
            }
            else if (counterEstados > 0)
            {
                counterEstados += 10;
                StartCoroutine("combinacionesElementales");
                return;

            }
            estadoFuego = true;
            counterEstados = 10;
            StartCoroutine("afectacionEstadoFuego");
        }
    }

    private IEnumerator combinacionesElementales()
    {
        if (counterEstados == 11)
        {
            //VIENTO - FUEGO
            estadoViento = false;
            afectacionViento = 0;
            counterEstados = 10;
            aumentoFuegoPotenciado = 3;
            ataque = ataqueMax * 0.75f;
            StopCoroutine("afectacionEstadoFuego");
            StartCoroutine("afectacionEstadoFuego");
        }
        yield return new WaitForEndOfFrame();
    }


    private void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            xObjetivo = collider.transform.position.x;


            //Cambio Orientacion
            if (xObjetivo < transform.position.x)
            {
                transform.localScale = new Vector3(-5, 5, 1);
            }
            else if (xObjetivo > transform.position.x)
            {
                transform.localScale = new Vector3(5, 5, 1);
            }

            float distanciaPlayer = Mathf.Abs(transform.position.x - collider.transform.position.x);

            if (distanciaPlayer <= 12)
            {
                tiempoFueraRango = 0;
                tiempoDentroRango += Time.deltaTime;
            }
            else
            {
                tiempoDentroRango = 0;
                tiempoFueraRango += Time.deltaTime;
            }

            if (ataqueDisponible && distanciaPlayer <= 12 && tiempoDentroRango < 5)
            {
                StartCoroutine(ataqueCuerpoCuerpo());
            }
            else if (ataqueDisponible && distanciaPlayer <= 12 && tiempoDentroRango > 5)
            {
                StartCoroutine(ataqueAturdimiento());
            }
            else if (ataqueDisponible && distanciaPlayer > 12 && tiempoFueraRango >= 10)
            {
                StartCoroutine(ataqueDistancia());
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //SIGNIFICARIA QUE EL PLAYER ESTA EN OTRA PLATAFORMA
            tiempoDentroRango = 0;
            tiempoFueraRango = 0;
            //cambio de plataforma
        }
    }


    //***************************************************************************************************
    //CORRUTINA DE ATAQUE DE ATURDIMIENTO
    //***************************************************************************************************
    private IEnumerator ataqueAturdimiento()
    {

        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA CORRUTINA
        atacando = true;
        ataqueDisponible = false;
        //GameObject Hoyustus = GameObject.
        //TIEMPO PARA LA ANIMACION
        Debug.Log("Preparando ataque inmovilizador");
        yield return new WaitForSeconds(1.5f);

        //GENERACION DEL CHARCO DE VENENO
        if (segundaEtapa)
        {
            GameObject charcoGenerado = Instantiate(charcoVeneno, transform.position + Vector3.down * 5f, Quaternion.identity);
            StartCoroutine(destruirCharco(charcoGenerado));
        }

        //SE EVALUA SI HOYUSTUS ESTA EN EL RANGO DEL ATAQUE
        if (Mathf.Abs(transform.position.x - GameObject.FindObjectOfType<Hoyustus>().GetComponent<Transform>().position.x) <= 15)
        {

            StartCoroutine(aturdirPlayer());
            Debug.Log("Te inmovilizo");
            yield return new WaitForSeconds(0.5f);
            tiempoDentroRango = 0;
        }

        //REINICIO DE VARIABLES RELACIONADAS A LA DETECCION Y EL ATAQUE
        tiempoDentroRango = 0;
        tiempoFueraRango = 0;
        ataqueDisponible = true;
        atacando = false;
    }


    private IEnumerator aturdirPlayer()
    {
        GameObject.FindObjectOfType<Hoyustus>().setParalisis();
        tiempoDentroRango = 0;
        yield return new WaitForSeconds(3f);
        GameObject.FindObjectOfType<Hoyustus>().quitarParalisis();
    }


    private IEnumerator destruirCharco(GameObject charco)
    {
        charco.SetActive(true);
        yield return new WaitForSeconds(4f);
        Destroy(charco);
    }


    //***************************************************************************************************
    //CORRUTINA DE ATAQUE CUERPO A CUERPO
    //***************************************************************************************************
    private IEnumerator ataqueCuerpoCuerpo()
    {

        //SE MODIFICAN ESTAS VARIABLES PARA NO INTERFERIR EL TIEMPO DE ACCION DE LA CORRUTINA
        ataqueDisponible = false;
        ataqueCuerpo.enabled = true;
        atacando = true;
        //EXTENDER UN POCO LA DIMENSION DEL BOXCOLLIDER
        //EL TIEMPO DEPENDERA DE LA ANIMACION
        yield return new WaitForSeconds(0.3f);
        ataqueCuerpo.enabled = false;

        //DASH TRAS ATAQUE EN LA SEGUNDA ETAPA
        if (segundaEtapa)
        {
            Debug.Log("EMBESTIDA");
            //transform.position = transform.position + Vector3.up * 0.1f;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(6f * -transform.localScale.x, 0f);
            yield return new WaitForSeconds(0.5f);
            rb.gravityScale = 5;
            rb.velocity = Vector2.zero;
        }
        atacando = false;
        yield return new WaitForSeconds(1.5f);
        ataqueDisponible = true;
    }

    //***************************************************************************************************
    //CORRUTINA DE ATAQUE A DISTANCIA
    //***************************************************************************************************
    private IEnumerator ataqueDistancia()
    {
        //REINICIO DE VARIABLES RELACIONADAS A LA DETECCION Y EL ATAQUE
        atacando = true;
        ataqueDisponible = false;
        Debug.Log("Listo para atacar a distancia");
        //CORREGIR POR EL TIEMPO DE LA ANIMACION
        yield return new WaitForSeconds(1f);
        if (!segundaEtapa)
        {
            float auxDisparo = -10f;
            for (int i = 0; i < 10; i++)
            {
                GameObject bolaVenenoGenerada = Instantiate(bolaVeneno, transform.position, Quaternion.identity);
                yield return new WaitForEndOfFrame();
                bolaVenenoGenerada.GetComponent<BolaVeneno>().aniadirFuerza(-transform.localScale.x, layerObject, 5, 20 + auxDisparo * 1.5f, explosion);
                auxDisparo++;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                System.Random aux = new System.Random();
                int direccion = aux.Next(0, 2);
                Debug.Log(direccion);
                if (direccion == 0)
                {
                    direccion = -1;
                }
                GameObject bolaVenenoGenerada = Instantiate(bolaVeneno, transform.position, Quaternion.identity);
                yield return new WaitForEndOfFrame();
                bolaVenenoGenerada.GetComponent<BolaVeneno>().aniadirFuerza(-transform.localScale.x * direccion, layerObject, 8, aux.Next(5, 22), explosion);
                yield return new WaitForSeconds(0.05f);
            }
        }


        //REINICIO DE VARIABLES RELACIONADAS A LA DETECCION Y EL ATAQUE
        atacando = false;
        ataqueDisponible = true;
        tiempoDentroRango = 0f;
        tiempoFueraRango = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //AL TOCAR UNA PLATAFORMA SE ESTABLECEN SUS LIMITES DE MOVIMIENTO EN X
        if (collision.gameObject.layer == 6)
        {
            minX = collision.transform.GetChild(0).gameObject.transform.position.x;
            maxX = collision.transform.GetChild(1).gameObject.transform.position.x;
        }
    }















    protected void MoverLibre()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, velocidad * Time.deltaTime);

        cuadros++;
        if (cuadros == 5)
        {
            NewTarget();
            cuadros = 0;
        }
    }
    void NewTarget()
    {
        float x = UnityEngine.Random.Range(-10.0f, 10.0f);
        float y = UnityEngine.Random.Range(-10.0f, 10.0f);

        target = new Vector3(x, y, 0);
    }

    protected bool RangoVision()
    {
        return false;
    }


    protected bool RangoAtaque()
    {
        return false;
    }

    protected bool RangoCercaJugador()
    {
        return false;
    }


    //El ataque depender� del tipo de Enemigo
    protected void Atacar()
    {
        switch (tipoEnemigo)
        {
            case TipoEnemigo.Chontacuro:
                break;
            case TipoEnemigo.ApallimayDaga:
                break;
            case TipoEnemigo.ApallimayArco:
                break;
            case TipoEnemigo.ApallimayEscudo:
                break;
            case TipoEnemigo.Tzantza:
                break;
            case TipoEnemigo.Boraro:
                break;
        }
    }
}