using UnityEngine;

public class ManagerPeleaMapinguari : MonoBehaviour
{
    [SerializeField] private PlataformaMapinguari[] plataformas = new PlataformaMapinguari[4];
    [SerializeField] private NubeToxica[] nubesToxicas = new NubeToxica[4];
    [SerializeField] private GameObject bossLifeBar;


    public void EliminarLogicaPlataformas() { 
        foreach(PlataformaMapinguari p in plataformas){ 
            Destroy(p);
        }

        foreach (NubeToxica n in nubesToxicas)
        {
            n.IsMapinguariDerrotado(true);
        }
        Destroy(bossLifeBar);
    }

    void Update()
    {
       if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.K))
        {
            EliminarLogicaPlataformas();
            GameObject.Find("Mapinguari").GetComponent<Mapianguari>().recibirDanio(3000);
        } 
    }
}
