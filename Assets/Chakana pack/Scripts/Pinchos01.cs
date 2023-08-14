using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos01 : MonoBehaviour
{
    [SerializeField] List<Pinchos02> pinchos = new List<Pinchos02>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for(int i = 0; i < pinchos.Count; i++)
            {
                pinchos[i].NewPos(transform.position);
            }
        }
    }
}
