using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OjoMapinguari : MonoBehaviour
{
    [SerializeField] Transform hoyustus;
    void Start()
    {
        hoyustus = GameObject.Find("Hoyustus Solicitud Prefab").transform;
    }

    void Update()
    {
        Vector3 direction = hoyustus.position + Vector3.down - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), 8 * Time.deltaTime);
        transform.localScale = new Vector3(transform.parent.localScale.x, 1, 1);
    }
}
