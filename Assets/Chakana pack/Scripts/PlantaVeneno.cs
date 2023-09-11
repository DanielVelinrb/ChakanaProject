using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PlantaVeneno : MonoBehaviour
{
    [SerializeField] private Transform hoyustus;
    [SerializeField] private bool ataqueDisponible = false;
    [SerializeField] private bool atacando = false;
    [SerializeField] private GameObject bolaVeneno;
    [SerializeField] private float danio;
    [SerializeField] private float anguloCambioVista;
    [SerializeField] private Animator anim;
    [SerializeField] private int codigoAtaque;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        hoyustus = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(inicio());
    }

    void Update()
    {
        CalcularDireccionVista();
        anim.SetInteger("Codigo Ataque", codigoAtaque);
        if(!atacando)
        {
            Flip();
        }
        if (ataqueDisponible)
        {
            ataqueDisponible = false;
            atacando = false;
            StartCoroutine(Ataque());
        }
    }

    private void CalcularDireccionVista() {
        float angle = Vector3.Angle(hoyustus.position - transform.position, transform.right * -transform.localScale.x);
        if (hoyustus.position.y < transform.position.y && angle > anguloCambioVista)
        {
            codigoAtaque = 2;
        }
        else if (hoyustus.position.y > transform.position.y && angle > anguloCambioVista)
        {
            codigoAtaque = 0;
        }
        else if (angle < anguloCambioVista)
        {
            codigoAtaque = 1;
        }
    }

    private void Flip() { 
        if(hoyustus.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (hoyustus.position.x < transform.position.x)
        {
            transform.localScale = Vector3.one;         
        }
    }


    IEnumerator inicio() { 
        yield return new WaitForSeconds(1.4f);
        ataqueDisponible = true;
    }

    private IEnumerator Ataque(){
        ataqueDisponible = false;
        atacando = true;
        GameObject bolaVenenoGenerada = Instantiate(bolaVeneno, transform.position + Vector3.up, Quaternion.identity);
        bolaVenenoGenerada.SetActive(false);
        bolaVenenoGenerada.name += "Enemy";
        bolaVenenoGenerada.AddComponent<ProyectilMovUniforme>();
        bolaVenenoGenerada.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForEndOfFrame();
        bolaVenenoGenerada.GetComponent<ProyectilMovUniforme>().setDanio(danio);
        bolaVenenoGenerada.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        atacando = false;
        yield return new WaitForSeconds(2);
        ataqueDisponible = true;
    }

    public void setDanio(float danio) {
        this.danio = danio;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14 && collider.gameObject.CompareTag("Untagged"))
        {
            Destroy(this.gameObject);
        }
    }
}
