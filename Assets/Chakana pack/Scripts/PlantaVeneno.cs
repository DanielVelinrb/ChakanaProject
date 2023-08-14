using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PlantaVeneno : MonoBehaviour
{
    //private Transform hoyustus;
    [SerializeField] private bool ataqueDisponible = false;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject bolaVeneno;
    [SerializeField] private float danio;

    void Start()
    {
        explosion = Resources.Load<GameObject>("Explosion");
        StartCoroutine(inicio());
    }

    void Update()
    {
        if (ataqueDisponible)
        {
            ataqueDisponible = false;
            StartCoroutine(Ataque());
        }
    }

    IEnumerator inicio() { 
        yield return new WaitForSeconds(1.4f);
        ataqueDisponible = true;
    }

    private IEnumerator Ataque(){
        ataqueDisponible = false;
        GameObject bolaVenenoGenerada = Instantiate(bolaVeneno, transform.position, Quaternion.identity);
        bolaVenenoGenerada.name += "Enemy";
        bolaVenenoGenerada.AddComponent<ProyectilMovUniforme>();
        bolaVeneno.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForEndOfFrame();
        bolaVenenoGenerada.GetComponent<ProyectilMovUniforme>().setDanio(danio);
        yield return new WaitForSeconds(2);
        ataqueDisponible = true;
    }

    public void setDanio(float danio) {
        this.danio = danio;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14)
        {
            Destroy(this.gameObject);
        }
    }
}
