using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField] private int limitRadio = 3;
    [SerializeField] private float indiceExplosion = 6;
    [SerializeField] private float danioExplosion = 45;
    [SerializeField] private string tipoExplosion;

    void FixedUpdate()
    {
        this.gameObject.transform.localScale += Vector3.one * Time.deltaTime * indiceExplosion;
        if (transform.localScale.x >= limitRadio)
        {
            Destroy(this.gameObject);
        }
    }

    public void modificarValores(int limitRadio, float danioExplosion, float indiceExplosion) {
        this.indiceExplosion = indiceExplosion;
        this.danioExplosion = danioExplosion;
        this.limitRadio = limitRadio;
    }


    public void modificarValores(int limitRadio, float danioExplosion, float indiceExplosion, int layer, string tag, string tipoExplosion){
        this.tipoExplosion = tipoExplosion;
        this.transform.gameObject.layer = layer;
        this.transform.gameObject.tag = tag;
        this.indiceExplosion = indiceExplosion;
        this.danioExplosion = danioExplosion;
        this.limitRadio = limitRadio;
    }


    public float getDanioExplosion() {
        return danioExplosion;
    }

    public string getTipoExplosion() { 
        return tipoExplosion;
    }

}
