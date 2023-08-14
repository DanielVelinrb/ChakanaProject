using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongoLum : MonoBehaviour
{
    [SerializeField] float aTime;
    [SerializeField] float bTime;
    [SerializeField] float sTime;
    [SerializeField] List<Color> colors = new List<Color>();

    SpriteRenderer hongo;
    bool isStart = false;
    bool way = false;

    void Start()
    {
        hongo = GetComponent<SpriteRenderer>();

        StartCoroutine(HongoTimer());
    }

    IEnumerator HongoTimer()
    {
        if (!isStart)
        {
            isStart = true;
            yield return new WaitForSeconds(sTime);
        }

        if (!way)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                hongo.color = colors[i];

                yield return new WaitForSeconds(aTime);
            }

            way = true;

            GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitForSeconds(bTime/2);

            StartCoroutine(HongoTimer());
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;

            for (int i = (colors.Count - 1); i >= 0; i--)
            {
                hongo.color = colors[i];

                yield return new WaitForSeconds(aTime);
            }

            way = false;

            yield return new WaitForSeconds((bTime));

            StartCoroutine(HongoTimer());
        }
    }
}
