using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDestroy : MonoBehaviour
{
    [SerializeField] private GameObject vfxDestroy;
    [SerializeField] private bool door, gold, heal;
    [SerializeField] private int doorNum;
    [SerializeField] GameObject goldObj;
    void Start()
    {
        if (PlayerPrefs.HasKey("puerta" + doorNum) && door)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 14)
        {
            if (door)
            {
                PlayerPrefs.SetInt("puerta" + doorNum, 1);
            }

            if (gold)
            {
                Instantiate(goldObj, transform.position, Quaternion.identity);

                Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, 3);

                foreach(Collider2D collider in objetos)
                {
                    Rigidbody2D rb2D = collider.GetComponent<Rigidbody2D>();
                    if(rb2D != null)
                    {
                        Vector2 direccion = collider.transform.position - transform.position;
                        float distancia = 1 + direccion.magnitude;
                        float fuerza = 200 / distancia;
                        rb2D.AddForce(direccion * fuerza);
                    }
                }
            }

            if (heal)
            {
                Vector3 newPos = transform.position;
                newPos.y += 0.5f;

                Instantiate(goldObj, newPos, Quaternion.identity);
            }

            Instantiate(vfxDestroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
