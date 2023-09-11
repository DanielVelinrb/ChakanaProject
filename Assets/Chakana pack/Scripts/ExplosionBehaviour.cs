using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField] private int radio = 3;
    [SerializeField] private float indiceExplosion = 6;
    [SerializeField] private float danioExplosion = 45;
    [SerializeField] private string tipoExplosion;
    [SerializeField] private bool isDynamic = true;
    [SerializeField] private float tiempoDestruccionEstatico = 0.4f;

    void FixedUpdate()
    {
        if (isDynamic)
        {
            this.gameObject.transform.localScale += Vector3.one * Time.deltaTime * indiceExplosion;
            if (transform.localScale.x >= radio)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void modificarValores(int limitRadio, float danioExplosion, float indiceExplosion, int layer, string tag, string tipoExplosion, bool tipo = true){
        this.tipoExplosion = tipoExplosion;
        transform.gameObject.layer = layer;
        transform.gameObject.tag = tag;
        this.indiceExplosion = indiceExplosion;
        this.danioExplosion = danioExplosion;
        this.radio = limitRadio;
        isDynamic = tipo;
        if (!isDynamic) {
            transform.localScale = new Vector3(limitRadio, limitRadio, 1);
            Destroy(this.gameObject.GetComponent<SpriteRenderer>(), 0.2f);
            Destroy(this.gameObject, tiempoDestruccionEstatico);
        }
    }


    public float getDanioExplosion() {
        return danioExplosion;
    }

    public string getTipoExplosion() { 
        return tipoExplosion;
    }

}
