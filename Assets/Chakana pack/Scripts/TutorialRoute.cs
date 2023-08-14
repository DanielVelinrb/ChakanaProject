using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoute : MonoBehaviour
{
    [SerializeField] private GameObject tutoObj;
    [SerializeField] private int tutoNum;
    private HudManager hud;

    private bool isActive, scene01;
    private GameObject keyObj, joyObj;
    private bool joystick = false;
    private Transform player;

    void Start()
    {
        if(PlayerPrefs.HasKey("tutorial" + tutoNum))
        {
            Destroy(gameObject);
        }

        keyObj = tutoObj.transform.GetChild(0).gameObject;
        joyObj = tutoObj.transform.GetChild(1).gameObject;

        keyObj.SetActive(true);
        joyObj.SetActive(false);

        if (tutoNum == 8 || tutoNum == 9 || tutoNum == 10 || tutoNum == 11)
        {
            hud = GameObject.Find("HUDMenu").GetComponent<HudManager>();
        }
    }

    void Update()
    {
        TutoDetector();
        DetectSkills();

        if (Input.anyKeyDown)
        {
            if (tutoNum == 5 && !scene01)
            {
                StartCoroutine(StartTuto());
                scene01 = true;
            }

            if (joystick)
            {
                joystick = false;

                keyObj.SetActive(true);
                joyObj.SetActive(false);
            }
        }

        if (Input.GetButtonDown("JoystickButton") || Input.GetAxis("HorizontalJ") != 0f || Input.GetAxis("VerticalJ") != 0f)
        {
            if (tutoNum == 5 && !scene01)
            {
                StartCoroutine(StartTuto());
                scene01 = true;
            }

            if (!joystick)
            {
                joystick = true;

                keyObj.SetActive(false);
                joyObj.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.transform;

            if (tutoNum <= 7 && tutoNum != 5)
            {
                tutoObj.transform.position = player.position;
                tutoObj.SetActive(true);
                isActive = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tutoObj.SetActive(false);
            isActive = false;
        }
    }

    private IEnumerator StartTuto()
    {
        yield return new WaitForSeconds(0.3f);

        tutoObj.SetActive(true);
        isActive = true;
    }

    private void TutoDetector()
    {
        if (isActive)
        {
            if (tutoNum <= 7)
            {
                tutoObj.transform.position = player.position;
            }

            if(tutoNum == 1 && Input.GetAxis("Jump") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1); 
                Destroy(gameObject);
            }
            if (tutoNum == 2 && Input.GetAxis("Interact") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 3 && Input.GetAxis("Atacar") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 4 && Input.GetAxis("Dash") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 5 && Input.GetAxis("Horizontal") != 0)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 6 && Input.GetAxis("Interact") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 7 && Input.GetAxis("Interact") == 1)
            {
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 8 && Input.anyKeyDown)
            {
                Time.timeScale = 1;
                print("dfas");
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 9 && Input.anyKeyDown)
            {
                Time.timeScale = 1;
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 10 && Input.anyKeyDown)
            {
                Time.timeScale = 1;
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
            if (tutoNum == 11 && Input.anyKeyDown)
            {
                Time.timeScale = 1;
                tutoObj.SetActive(false);
                PlayerPrefs.SetInt("tutorial" + tutoNum, 1);
                Destroy(gameObject);
            }
        }
    }

    private void DetectSkills()
    {
        if(tutoNum == 8 || tutoNum == 9 || tutoNum == 10 || tutoNum == 11)
        {
            if (isActive) return;

            if(tutoNum == 8 && hud.GetCondor() >= 100)
            {
                Time.timeScale = 0;
                StartCoroutine(SkillsTuto());
            }
            else if (tutoNum == 9 && hud.GetSnake() >= 100)
            {
                Time.timeScale = 0;
                StartCoroutine(SkillsTuto());
            }
            else if (tutoNum == 10 && hud.GetWeapon() >= 100)
            {
                Time.timeScale = 0;
                StartCoroutine(SkillsTuto());
            }
            else if (tutoNum == 11 && hud.GetCuracion() >= 100)
            {
                Time.timeScale = 0;
                StartCoroutine(SkillsTuto());
            }
        }
    }

    IEnumerator SkillsTuto()
    {
        tutoObj.transform.GetChild(5).gameObject.SetActive(false);
        tutoObj.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        print("asfas");
        tutoObj.transform.GetChild(5).gameObject.SetActive(true);
        isActive = true;
    }
}
