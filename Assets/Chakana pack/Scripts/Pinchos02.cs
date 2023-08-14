using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos02 : MonoBehaviour
{
    Vector3 pos;
    bool isActive = false;
    [SerializeField] GameObject exVFX;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isActive)
        {
            isActive = true;
            StartCoroutine(DelayDmg(collision.gameObject.GetComponent<Hoyustus>()));
        }
    }

    public void NewPos(Vector3 e)
    {
        if (isActive) return;
        pos = e;
    }

    IEnumerator DelayDmg(Hoyustus player)
    {
        player.recibirDanio(200);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        GetComponent<AudioSource>().Play();

        Instantiate(exVFX, transform.position, Quaternion.identity);

        if(player.getVida() <= 0)
        {
            //Instantiate(exVFX, transform.position, Quaternion.identity);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        }
        else
        {
            //Instantiate(exVFX, transform.position, Quaternion.identity);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 12, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.44f);

            rb.velocity = Vector2.zero;
            pos.z = player.transform.position.z;
            player.transform.position = pos;
            isActive = false;
        }
    }
}
