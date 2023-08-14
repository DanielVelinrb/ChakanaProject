using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] Transform door1, door2;
    [SerializeField] GameObject invokeFX;
    [SerializeField] GameObject treasure;

    bool isOn, isMove, isOnBattle, finishSpawn, treasureCollect;
    private Vector3 destination1, destination2;
    private Vector3 originalPos1, originalPos2;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("combat"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        destination1 = door1.position;
        originalPos1 = door1.position;
        destination1.y -= 4;

        destination2 = door2.position;
        originalPos2 = door2.position;
        destination2.y -= 4;
    }

    private void LateUpdate()
    {
        if (isMove)
        {
            if (isOnBattle)
            {
                door1.position = Vector3.MoveTowards(door1.position, destination1, 5 * Time.deltaTime);
                door2.position = Vector3.MoveTowards(door2.position, destination2, 5 * Time.deltaTime);

                if (transform.position == destination1) isMove = false;
            }
            else
            {
                door1.position = Vector3.MoveTowards(door1.position, originalPos1, 5 * Time.deltaTime);
                door2.position = Vector3.MoveTowards(door2.position, originalPos2, 5 * Time.deltaTime);

                if (transform.position == destination1) isMove = false;
            }
        }
    }

    private void Update()
    {
        if (finishSpawn)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                }
            }

            if(enemies.Count == 0)
            {
                finishSpawn = false;
                StartCoroutine(StopCombat());
            }
        }

        if (treasureCollect && !treasure.GetComponent<SpriteRenderer>().enabled)
        {
            isOnBattle = false;
            isMove = true;

            PlayerPrefs.SetInt("combat", 1);

            treasureCollect = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isOn)
            {
                isOnBattle = true;
                isMove = true;

                StartCoroutine(StartCombat());

                isOn = true;
            }
        }
    }

    IEnumerator StartCombat()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            yield return new WaitForSeconds(2f);
            Destroy(Instantiate(invokeFX, enemies[i].transform.position, Quaternion.identity), 2);
            yield return new WaitForSeconds(0.2f);
            enemies[i].SetActive(true);
        }

        yield return new WaitForSeconds(5);

        finishSpawn = true;
    }

    IEnumerator StopCombat()
    {
        yield return new WaitForSeconds(1f);

        treasure.SetActive(true);
        PlayerPrefs.SetInt("combat", 1);

        treasureCollect = true;
    }
}
