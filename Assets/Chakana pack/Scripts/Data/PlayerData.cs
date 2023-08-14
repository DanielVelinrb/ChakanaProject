using UnityEngine;

[System.Serializable]
public class PlayerData{

    private float maxVida = 1000;
    private float vida;
    private int gold;
    private float ataque;
    //private Transform positionRevivir;
    private float cargaHabilidadCondor, cargaHabilidadSerpiente, cargaHabilidadLanza;
    private float curacion;
    public PlayerData(Hoyustus hoyustus) {
        vida =  hoyustus.getVida();
        ataque = hoyustus.getAtaque();
        gold = hoyustus.getGold();
    }

    public PlayerData(float vida, int gold, float condor, float serpiente, float lanza, float curacion, float ataque) { 
        this.vida = vida;
        this.ataque = ataque;
        this.gold = gold;
        this.cargaHabilidadCondor = condor;
        this.cargaHabilidadSerpiente = serpiente;
        this.cargaHabilidadLanza = lanza;
        this.curacion = curacion;
    }

    public float getVida(){
        return vida;
    }

    public int getGold(){
        return gold;
    }

    public float getAtaque(){
        return ataque;
    }

    public float getCondor()
    {
        return cargaHabilidadCondor;
    }

    public float getSerpiente()
    {
        return cargaHabilidadSerpiente;
    }

    public float getLanza()
    {
        return cargaHabilidadLanza;
    }

    public float getCuracion()
    {
        return curacion;
    }
}
