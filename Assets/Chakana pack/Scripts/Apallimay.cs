using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apallimay : Enemy
{
    [SerializeField] protected Transform groundDetector;
    [SerializeField] protected Transform wallDetector;
    [SerializeField] protected float distanciaPlayer;
    [SerializeField] protected bool ataqueDisponible;

    protected bool Grounded()
    {
        return Physics2D.OverlapCircle(groundDetector.position - Vector3.right * transform.localScale.x, 0.2f, groundLayer);
    }

    protected Vector3 orientacionDeteccionPlayer(Vector3 playerPosition)
    {
        return (playerPosition - transform.position).normalized;
    }

}