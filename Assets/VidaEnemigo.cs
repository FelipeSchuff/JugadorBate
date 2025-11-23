using UnityEngine;
using System.Collections; 

public class VidaEnemigo : MonoBehaviour
{
    private Renderer miRender;
    private Color colorOriginal;

    void Start()
    {
        miRender = GetComponent<Renderer>();
        colorOriginal = miRender.material.color;
    }

    public void RecibirGolpe()
    {
        StartCoroutine(FlashRojo());
    }

    IEnumerator FlashRojo()
    {
        miRender.material.color = Color.red; 
        yield return new WaitForSeconds(0.3f); 
        miRender.material.color = colorOriginal; 
    }
}