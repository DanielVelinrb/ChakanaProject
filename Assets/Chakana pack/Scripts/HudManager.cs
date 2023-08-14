using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    Hoyustus player;
    [SerializeField] LiquidBar lifeBar;
    [SerializeField] LiquidBar manaBar;
    [SerializeField] LiquidBar condorBar;
    [SerializeField] LiquidBar snakeBar;
    [SerializeField] LiquidBar weaponBar;
    [SerializeField] Text goldTxt;

    float lifeMax;
    float life;
    float mana;
    float condor;
    float snake;
    float weapon;
    float maxValue;
    float gold;

    private void Awake()
    {
        maxValue = 100;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hoyustus>();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("primerDato"))
        {
            PlayerPrefs.SetFloat("vidaM", player.getVida());
            lifeMax = PlayerPrefs.GetFloat("vidaM");
            PlayerPrefs.SetInt("primerDato", 1);
        }
        else
        {
            lifeMax = PlayerPrefs.GetFloat("vidaM");
        }

        life = player.getVida();
        mana = player.getCargaCuracion();
        condor = player.getCargaHabilidadCondor();
        snake = player.getCargaHabilidadSerpiente();
        weapon = player.getCargaHabilidadLanza();
        gold = player.getGold();

        lifeBar.currentFillAmount = (life / lifeMax);
        lifeBar.targetFillAmount = (life / lifeMax);

        manaBar.currentFillAmount = (mana / maxValue);
        manaBar.targetFillAmount = (mana / maxValue);

        condorBar.currentFillAmount = (condor / maxValue);
        condorBar.targetFillAmount = (condor / maxValue);

        snakeBar.currentFillAmount = (snake / maxValue);
        snakeBar.targetFillAmount = (snake / maxValue);

        weaponBar.currentFillAmount = (weapon / maxValue);
        weaponBar.targetFillAmount = (weapon / maxValue);
        goldTxt.text = player.getGold().ToString();

        string[] joystickNames = Input.GetJoystickNames();

        if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
        {
            Debug.Log("Hay un mando conectado.");
        }
    }

    void Update()
    {
        UpdateData();

        //print(player.getCargaHabilidadSerpiente());
    }

    private void UpdateData()
    {
        if(player.getVida() != life)
        {
            life = player.getVida();

            lifeBar.targetFillAmount = (life / lifeMax);
        }

        if(player.getGold() != gold)
        {
            gold = player.getGold();

            goldTxt.text = player.getGold().ToString();
        }

        if (player.getCargaCuracion() != mana)
        {
            mana = player.getCargaCuracion();

            manaBar.targetFillAmount = (mana / maxValue);
        }

        if(player.getCargaHabilidadCondor() != condor)
        {
            condor = player.getCargaHabilidadCondor();

            condorBar.targetFillAmount = (condor / maxValue);
        }

        if (player.getCargaHabilidadSerpiente() != snake)
        {
            snake = player.getCargaHabilidadSerpiente();

            snakeBar.targetFillAmount = (snake / maxValue);
        }

        if (player.getCargaHabilidadLanza() != weapon)
        {
            weapon = player.getCargaHabilidadLanza();

            weaponBar.targetFillAmount = (weapon / maxValue);
        }
    }

    public float GetCondor()
    {
        return condor;
    }

    public float GetSnake()
    {
        return snake;
    }

    public float GetWeapon()
    {
        return weapon;
    }

    public float GetCuracion()
    {
        return mana;
    }
}
