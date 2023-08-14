using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemigos;
    [SerializeField] private Hoyustus player;
    void Start()
    {
    }

    void Update()
    {
        Debug.Log(enemigos[0].GetType());
    }

    public void SuscribirEnemigo(Enemy enemigo) { 
        enemigos.Add(enemigo);
    }

    //setPlayer
    //Metodo de ejecucion corrutina de control

}
