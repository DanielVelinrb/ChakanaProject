using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaHoyustus : MonoBehaviour
{
    private int danio = 5;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(14, 12, true);
    }


    //***************************************************************************************************
    //DETECCION DE COLISIONES
    //***************************************************************************************************
    /*private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            try
            {
                collider.GetComponent<CharactersBehaviour>().recibirDanio(danio);
            }
            catch (Exception e) {
                Debug.Log("El enemy no posee el script CharactersBehaviour");
            }
        }
    }*/

    public int getDanioArma() {
        return danio;
    }

}
