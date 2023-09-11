using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoyustusHead : MonoBehaviour
{
    private const float V = 251f;

    // Start is called before the first frame update

    [SerializeField] public float life;
    [SerializeField] Animator anim;
    [SerializeField] RectTransform deadAwareness;

    Hoyustus player;

    private void Awake()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hoyustus>();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("primerDato"))
        {
            PlayerPrefs.SetFloat("vidaM", player.getVida());
            
            PlayerPrefs.SetInt("primerDato", 1);
        }

        anim = this.gameObject.GetComponent<Animator>();
        life = player.getVida();
    }

    // Update is called once per frame
    void Update()
    {
        life = player.getVida();

        if (life < V)
        {
        anim.SetBool("FlagDeathAwareness", true);
        deadAwareness.gameObject.SetActive(true);
        }
        else
        {
            anim.SetBool("FlagDeathAwareness", false);
            deadAwareness.gameObject.SetActive(false);
        }
    }
}
