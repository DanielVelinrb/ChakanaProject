using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom_cin : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject player;

    private Rigidbody2D rb;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("inicio01"))
        {
            door.SetActive(true);
            rb = player.GetComponent<Rigidbody2D>();
            player.GetComponent<Hoyustus>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Destroy(gameObject);
        }
        else
        {
            rb = player.GetComponent<Rigidbody2D>();

            StartCoroutine(PlayScene());
        }
    }

    IEnumerator PlayScene()
    {
        PlayerPrefs.SetString("inicio01", "si");

        yield return new WaitForSeconds(2.35f);

        door.SetActive(true);
        GetComponent<AudioSource>().Play();
        player.GetComponent<Animator>().SetBool("Grounded", true);
        yield return new WaitForSeconds(1.2f);

        player.GetComponent<Hoyustus>().enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Destroy(gameObject);
    }
}
