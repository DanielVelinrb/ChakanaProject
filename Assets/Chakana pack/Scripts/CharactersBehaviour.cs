using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersBehaviour : MonoBehaviour
{
    [Header("Atributos Basicos")]
    [SerializeField] protected int gold;
    [SerializeField] protected float vida;
    [SerializeField] protected float ataque;
    [SerializeField] protected float ataqueMax;
    [Space(5)]

    [Header("Estados Elementales")]
    [SerializeField] protected bool estadoViento;
    [SerializeField] protected bool estadoFuego;
    [SerializeField] protected bool estadoVeneno;
    [SerializeField] protected int counterEstados;
    [SerializeField] protected float afectacionViento;
    [SerializeField] protected float afectacionFuego = 15;
    [SerializeField] protected float afectacionVeneno = 0.05f;
    [SerializeField] protected int aumentoFuegoPotenciado = 1;
    [SerializeField] protected float aumentoDanioParalizacion = 1f;
    [Space(5)]

    [Header("Invulnerabilidad")]
    [SerializeField] protected bool invulnerable = false;
    [SerializeField] protected bool playable = true;
    [SerializeField] protected string explosionInvulnerable;

    [SerializeField] protected GameObject vientoFX;
    [SerializeField] protected GameObject fuegoFX;
    [SerializeField] protected GameObject venenoFX;
    [SerializeField] protected GameObject recieveDmgFX;
    [SerializeField] protected Material receiveDmgMat;

    [SerializeField] protected GameObject explosion;
    [SerializeField] protected GameObject combFX01;
    [SerializeField] protected GameObject combFX02;
    [SerializeField] protected GameObject combFX03;

    protected GameObject combObj01, combObj02, combObj03;
    protected Material playerMat = null;
    protected int layerObject;
    protected Rigidbody2D rb;
    protected bool paralizadoPorAtaque = false;
    public float fuerzaRecoil;
    private GameObject vientoObj, fuegoObj, venenoObj;
    protected float vidaMax = 0;



    //***************************************************************************************************
    //CORRUTINA DE DANIO E INVULNERABILIDAD (POSIBLE SEPARACION DE LA VULNERABILIDAD A METODO INDIVIDUAL)
    //***************************************************************************************************
    protected virtual IEnumerator cooldownRecibirDanio(int direccion, float fuerzaRecoil)
    {
        Recoil(direccion, fuerzaRecoil);
        if (vida <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.2f);
        //SE DETIENE EL RECOIL
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        //EL OBJECT PUEDE VOLVER A MOVERSE SIN ESTAR EN ESTE ESTADO DE "SER ATACADO"
        playable = true;
        //REVISAR SI SE DEBE PASAR EL VALOR DE 2 A LOS ENEMIGOS ANTES DE QUITAR LAS INVULNERABILIDADES O SI ESTA BIEN CON 0.5
        yield return new WaitForSeconds(0.5f);
        QuitarInvulnerabilidades(layerObject);
    }


    //***************************************************************************************************
    //LOGICA DE RECOIL
    //***************************************************************************************************
    protected virtual void Recoil(int direccion, float fuerzaRecoil)
    {
    }


    //***************************************************************************************************
    //FUNCION DE INVULNERABILIDAD A TODO DANIO
    //***************************************************************************************************
    protected void EstablecerInvulnerabilidades(int layerObject)
    {
        invulnerable = true;
        //Physics2D.IgnoreLayerCollision(3, layerObject, true);
        Physics2D.IgnoreLayerCollision(layerObject, 12, true);
        Physics2D.IgnoreLayerCollision(layerObject, 15, true);
    }


    //***************************************************************************************************
    //FUNCION DE INVULNERABILIDAD A TODO DANIO
    //***************************************************************************************************
    protected virtual void QuitarInvulnerabilidades(int layerObject)
    {
        invulnerable = false;
        //Physics2D.IgnoreLayerCollision(3, layerObject, false);
        //Physics2D.IgnoreLayerCollision(layerObject, 12, false);
        //Physics2D.IgnoreLayerCollision(layerObject, 15, false);
    }


    //***************************************************************************************************
    //DETECCION COLISIONES
    //***************************************************************************************************
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //LAYER EXPLOSION
        if ((collider.gameObject.layer == 12 && !invulnerable && collider.gameObject.GetComponent<ExplosionBehaviour>().getTipoExplosion() != explosionInvulnerable))
        {
            triggerElementos_1_1_1(collider);
            recibirDanio(collider.gameObject.GetComponent<ExplosionBehaviour>().getDanioExplosion());
            StartCoroutine(cooldownRecibirDanio((int)-Mathf.Sign(collider.transform.position.x - transform.position.x), 1));
            return;
        }

    }


    //***************************************************************************************************
    //CORRUTINA DE COOLDOWN INVULNERABILIDADES POR EXPLOSIONES
    //***************************************************************************************************
    protected IEnumerator cooldownInvulnerabilidadExplosiones()
    {
        yield return new WaitForSeconds(0.5f);
        QuitarInvulnerabilidades(layerObject);
    }


    //***************************************************************************************************
    //CORRUTINA DE ESTADO VIENTO
    //***************************************************************************************************
    protected IEnumerator afectacionEstadoViento()
    {
        if (vientoObj == null)
        {
            vientoObj = Instantiate(vientoFX, transform.position, Quaternion.identity);
            vientoObj.transform.parent = transform;
        }
        afectacionViento = 0.10f;
        print(gameObject.name);
        yield return new WaitForSeconds(10f);
        estadoViento = false;
        afectacionViento = 0;
        counterEstados = 0;
        if (vientoObj != null) Destroy(vientoObj);
    }


    //***************************************************************************************************
    //CORRUTINA DE ESTADO FUEGO
    //***************************************************************************************************
    protected IEnumerator afectacionEstadoFuego()
    {
        estadoFuego = true;
        if (transform.CompareTag("Player"))
        {
            Vector3 newPos = transform.position;
            newPos.y -= 0.75f;
            if (fuegoObj == null)
            {
                fuegoObj = Instantiate(fuegoFX, newPos, Quaternion.identity);
                fuegoObj.transform.parent = transform;
            }
        }
        else
        {
            if (fuegoObj == null)
            {
                fuegoObj = Instantiate(fuegoFX, transform.position, Quaternion.identity);
                fuegoObj.transform.parent = transform;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(2f);
            vida -= (afectacionFuego * aumentoFuegoPotenciado);
        }
        aumentoFuegoPotenciado = 1;
        estadoFuego = false;
        counterEstados = 0;
        ataque = this.ataqueMax;
        if (fuegoObj != null) Destroy(fuegoObj);
    }


    //***************************************************************************************************
    //CORRUTINA DE ESTADO VENENO
    //***************************************************************************************************
    protected IEnumerator afectacionEstadoVeneno()
    {
        if (venenoObj == null)
        {
            venenoObj = Instantiate(venenoFX, transform.position, venenoFX.transform.rotation);
            venenoObj.transform.parent = transform;
        }
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(2f);
            float dmg = vidaMax * afectacionVeneno;
            vida -= dmg;
            print(vidaMax);
        }
        estadoVeneno = false;
        counterEstados = 0;
        if (venenoObj != null) Destroy(venenoObj);
    }


    //***************************************************************************************************
    //RECIBIR DANIO ATAQUE ENEMIGO
    //***************************************************************************************************
    public void recibirDanio(float danio)
    {

        if (vidaMax == 0) vidaMax = vida;

        vida -= (danio * aumentoDanioParalizacion);

        StartCoroutine(RecibirDanioBrillo());
        Destroy(Instantiate(recieveDmgFX, transform.position, Quaternion.identity), 1);

        //DE SER TRUE SIGNIFICARIA QUE EL JUGADOR ESTA PARALIZADO VOLVIENDO A SUS VALORES REGULARES (ELIMINACION PARALISIS)
        if (paralizadoPorAtaque)
        {
            playable = true;
            aumentoDanioParalizacion = 1.0f;
            paralizadoPorAtaque = true;
        }
    }

    IEnumerator RecibirDanioBrillo()
    {
        if (playerMat == null) playerMat = GetComponent<SpriteRenderer>().material;
        GetComponent<SpriteRenderer>().material = receiveDmgMat;

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().material = playerMat;
    }

    public void setParalisis()
    {
        rb.velocity = Vector3.zero;
        playable = false;
        aumentoDanioParalizacion = 1.5f;
        paralizadoPorAtaque = true;
    }

    public void quitarParalisis()
    {
        playable = true;
        aumentoDanioParalizacion = 1f;
        paralizadoPorAtaque = false;
    }


    //***************************************************************************************************
    //GETTERS
    //***************************************************************************************************

    public float getVida()
    {
        return vida;
    }


    public float getAtaque()
    {
        return ataque;
    }


    public int getGold()
    {
        return gold;
    }


    protected void collisionElementos_1_1_1(Collision2D collider)
    {
        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VIENTO
        if (collider.gameObject.CompareTag("Viento"))
        {
            //REINICIO ESTADO VIENTO
            if (estadoViento)
            {
                StopCoroutine("afectacionEstadoViento");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 1;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;

            }

            //SE ESTABLECE EL ESTADO DE VIENTO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoViento = true;
            counterEstados = 1;
            StartCoroutine("afectacionEstadoViento");
        }

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO FUEGO
        else if (collider.gameObject.CompareTag("Fuego"))
        {
            //REINICIO ESTADO FUEGO
            if (estadoFuego)
            {
                StopCoroutine("afectacionEstadoFuego");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 10;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;
            }

            //SE ESTABLECE EL ESTADO DE FUEGO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoFuego = true;
            counterEstados = 10;
            StartCoroutine("afectacionEstadoFuego");
        }

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VENENO
        else if (collider.gameObject.CompareTag("Veneno"))
        {
            //REINICIO ESTADO VENENO
            if (estadoVeneno)
            {
                StopCoroutine("afectacionEstadoVeneno");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 100;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;
            }

            //SE ESTABLECE EL ESTADO DE VENENO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoVeneno = true;
            counterEstados = 100;
            StartCoroutine("afectacionEstadoVeneno");
        }

    }

    protected void triggerElementos_1_1_1(Collider2D collider)
    {
        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VIENTO
        if (collider.gameObject.CompareTag("Viento"))
        {
            //REINICIO ESTADO VIENTO
            if (estadoViento)
            {
                StopCoroutine("afectacionEstadoViento");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 1;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;

            }

            //SE ESTABLECE EL ESTADO DE VIENTO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoViento = true;
            counterEstados = 1;
            StartCoroutine("afectacionEstadoViento");
        }

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO FUEGO
        else if (collider.gameObject.CompareTag("Fuego"))
        {
            //REINICIO ESTADO FUEGO
            if (estadoFuego)
            {
                StopCoroutine("afectacionEstadoFuego");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 10;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;
            }

            //SE ESTABLECE EL ESTADO DE FUEGO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoFuego = true;
            counterEstados = 10;
            StartCoroutine("afectacionEstadoFuego");
        }

        //DETECCIONS DE TRIGGERS DE OBJETOS TAGUEADOS COMO VENENO
        else if (collider.gameObject.CompareTag("Veneno"))
        {
            //REINICIO ESTADO VENENO
            if (estadoVeneno)
            {
                StopCoroutine("afectacionEstadoVeneno");
            }
            //SE DISPARA AL TENER YA UN ESTADO ELEMENTAL ACTIVO
            else if (counterEstados > 0)
            {
                counterEstados += 100;
                if (venenoObj != null) Destroy(venenoObj);
                if (fuegoObj != null) Destroy(fuegoObj);
                if (vientoObj != null) Destroy(vientoObj);
                StartCoroutine("combinacionesElementales");
                return;
            }

            //SE ESTABLECE EL ESTADO DE VENENO Y SUS RESPECTIVOS COMO ACTIVOS
            estadoVeneno = true;
            counterEstados = 100;
            StartCoroutine("afectacionEstadoVeneno");
        }

    }

    protected virtual IEnumerator combinacionesElementales()
    {
        if (counterEstados == 11)
        {
            //VIENTO - FUEGO
            if (combObj01 == null) combObj01 = Instantiate(combFX01, transform.position, Quaternion.identity);
            estadoViento = false;
            afectacionViento = 0;
            counterEstados = 10;
            aumentoFuegoPotenciado = 3;
            ataque = ataqueMax * 0.75f;
            StopCoroutine("afectacionEstadoFuego");
            estadoFuego = true;
            StartCoroutine("afectacionEstadoFuego");
        }
        else if (counterEstados == 101)
        {
            //VENENO - VIENTO
            if (combObj02 == null) combObj02 = Instantiate(combFX02, transform.position, Quaternion.identity, transform);
            StopCoroutine("afectacionEstadoVeneno");
            StopCoroutine("afectacionEstadoViento");
            rb.velocity = Vector3.zero;
            counterEstados = 0;
            estadoVeneno = false;
            estadoViento = false;
            playable = false;
            aumentoDanioParalizacion = 1.5f;
            yield return new WaitForSeconds(2f);
            playable = true;
            aumentoDanioParalizacion = 1f;
            //StartCoroutine(setParalisis());

        }
        else if (counterEstados == 110)
        {
            //FUEGO - VENENO
            if (combObj03 == null) combObj03 = Instantiate(combFX03, transform.position, Quaternion.identity);
            StopCoroutine("afectacionEstadoVeneno");
            StopCoroutine("afectacionEstadoFuego");
            counterEstados = 0;
            explosion.GetComponent<ExplosionBehaviour>().modificarValores(3, 45, 6, 12, "Untagged", "ExplosionPlayer");
            Instantiate(explosion, transform.position, Quaternion.identity);
            estadoVeneno = false;
            estadoFuego = false;
        }
        yield return new WaitForEndOfFrame();
    }
}