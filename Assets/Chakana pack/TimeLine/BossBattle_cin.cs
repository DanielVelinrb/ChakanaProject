using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class BossBattle_cin : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject player;
    [SerializeField] AudioSource roca;
    [SerializeField] Mapianguari boss;

    void Start()
    {
        StartCoroutine(PlayScene());
    }

    void Update()
    {
        print(director.state);
    }

    IEnumerator PlayScene()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.51f);
        roca.Play();
        
        yield return new WaitForSeconds(0.6f);

        player.GetComponent<Hoyustus>().enabled = true;
        boss.enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
