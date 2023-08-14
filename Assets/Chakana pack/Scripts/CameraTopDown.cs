using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopDown : MonoBehaviour
{
    private float vertical, horizontal;
    private Vector3 destination;
    private bool isMove;

    [SerializeField] private GameObject cinemaC, cinemaC2;
    [SerializeField] private float velocity;

    void Update()
    {
        vertical = Input.GetAxis("Vertical2");
        horizontal = Input.GetAxis("Horizontal2");

        DoUCamera(vertical, horizontal);
    }

    private void DoUCamera(float a, float b)
    {
        if(b <= 0.2 && b >= -0.2 && !isMove)
        {
            if(a >= 0.5)
            {
                if (!cinemaC2.activeSelf)
                {
                    cinemaC2.SetActive(true);
                    cinemaC2.transform.position = cinemaC.transform.position;
                }

                destination = cinemaC.transform.position;
                destination.y += 8;

                cinemaC2.transform.position = Vector3.MoveTowards(cinemaC2.transform.position, destination, velocity * Time.deltaTime);

            }
            else if(a <= -0.5)
            {
                if (!cinemaC2.activeSelf)
                {
                    cinemaC2.SetActive(true);
                    cinemaC2.transform.position = cinemaC.transform.position;
                }

                destination = cinemaC.transform.position;
                destination.y -= 8;

                cinemaC2.transform.position = Vector3.MoveTowards(cinemaC2.transform.position, destination, velocity * Time.deltaTime);
            }
            else
            {
                if (cinemaC2.activeSelf) cinemaC2.SetActive(false);
                StartCoroutine(EnableCamera());
                isMove = true;
            }
        }
        else
        {
            if (cinemaC2.activeSelf) cinemaC2.SetActive(false);
            StartCoroutine(EnableCamera());
            isMove = true;
        }
    }

    IEnumerator EnableCamera()
    {
        yield return new WaitForSeconds(1.75f);
        isMove = false;
    }
}
