using UnityEngine;
using System.Collections;


public class Enemy : CharactersBehaviour
{
    [SerializeField] protected GameObject deathFX;
    [SerializeField] protected float rangoVision;
    [SerializeField] protected float rangoAtaque;
    [SerializeField] protected float rangoPreparacion;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected Vector3 objetivo;
    [SerializeField] protected float speed;
    [SerializeField] protected Animator anim;

    protected virtual float OrientacionDeteccionPlayer(float playerPositionX)
    {
        if (playerPositionX < transform.position.x) return -1;
        else if (playerPositionX > transform.position.x) return 1;

        return playerPositionX;
    }

    protected bool CambioOrientacionDisponible(float flipRange) {
        return transform.position.x + flipRange < objetivo.x || transform.position.x - flipRange > objetivo.x;
    }

    protected void cambioColor(int color)
    {
        Color[] colores = { Color.white, Color.black, Color.blue, Color.yellow, Color.red };
        GetComponent<SpriteRenderer>().color = colores[color];
    }

    protected override IEnumerator cooldownRecibirDanio(int direccion, float fuerzaRecoil)
    {
        Recoil(direccion, fuerzaRecoil);
        if (vida <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        playable = true;
    }

    protected void PlayAnimation(string animation)
    {
        anim.Play(animation);
    }

}
