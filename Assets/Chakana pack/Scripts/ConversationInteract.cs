using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ConversationInteract : MonoBehaviour
{
    [SerializeField] Hoyustus player;
    [SerializeField] DialogueSystemTrigger data;

    void Start()
    {
        if(PlayerPrefs.GetInt("ukukuM") == 4 && PlayerPrefs.GetInt("conv01") != 2) data.conversation = "Ukuku02";
        else if(PlayerPrefs.GetInt("conv01") == 1) data.conversation = "Ukuku03";
        else if(PlayerPrefs.GetInt("conv01") == 2) data.conversation = "Ukuku04";
    }

    public void StartConversation()
    {
        player.enabled = false;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<AudioSource>().Stop();

        player.GetComponent<Animator>().SetBool("Walking", false);
        player.GetComponent<Animator>().SetBool("Dashing", false);
        player.GetComponent<Animator>().SetBool("Atacando", false);
        player.GetComponent<Animator>().SetBool("Jumping", false);
        player.GetComponent<Animator>().SetBool("Grounded", true);

        if (!PlayerPrefs.HasKey("conv01"))
        {
            PlayerPrefs.SetInt("conv01", 1);
        }
        
        if (PlayerPrefs.GetInt("ukukuM") == 4)
        {
            PlayerPrefs.SetInt("conv01", 2);
        }
    }

    public void StopConversation()
    {
        player.enabled = true;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<AudioSource>().Stop();

        if (PlayerPrefs.GetInt("ukukuM") == 4 && PlayerPrefs.GetInt("conv01") != 2) data.conversation = "Ukuku02";
        else if (PlayerPrefs.GetInt("conv01") == 1) data.conversation = "Ukuku03";
        else if (PlayerPrefs.GetInt("conv01") == 2) data.conversation = "Ukuku04";
    }
}
