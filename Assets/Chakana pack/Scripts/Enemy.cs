using UnityEngine; 


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

    protected virtual float orientacionDeteccionPlayer(float playerPositionX)
    {
        if (playerPositionX < transform.position.x) return -1;
        else if (playerPositionX > transform.position.x) return 1;

        return playerPositionX;
    }

}
