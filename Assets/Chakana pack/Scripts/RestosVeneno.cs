using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestosVeneno : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || (collision.gameObject.layer == 17
            && collision.transform.position.y < transform.position.y)) { 
            //GENERAR CHARCO
        }
    }
}
