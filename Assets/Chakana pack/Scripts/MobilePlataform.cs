using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlataform : MonoBehaviour
{
    [SerializeField] private List<Transform> mP = new List<Transform>();
    [SerializeField] private float velocity;

    private Vector3 destination;
    private int lastPoint;
    private int simbol = 1;
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        destination = mP[0].position;
        lastPoint = 0;
    }

    private void LateUpdate()
    {
        tr.position = Vector3.MoveTowards(tr.position, destination, velocity * Time.deltaTime);

        CheckMP();
    }

    private void CheckMP()
    {
        if (tr.position == mP[lastPoint].position)
        {
            if (lastPoint == (mP.Count - 1)) simbol = -1;
            else if (lastPoint == 0) simbol = 1;

            lastPoint += simbol;

            destination = mP[lastPoint].position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
