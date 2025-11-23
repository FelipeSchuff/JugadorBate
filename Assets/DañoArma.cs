using UnityEngine;

public class DañoArma : MonoBehaviour
{
    public AudioClip sonidoGolpe;
    private AudioSource audioSource;
    
    public bool puedeHacerDaño = false; 

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

   private void OnTriggerEnter(Collider other)
    {
        if (puedeHacerDaño == false) return;

        if (other.CompareTag("Enemigo"))
        {
            VidaEnemigo enemigo = other.GetComponent<VidaEnemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirGolpe();
                if (sonidoGolpe != null) audioSource.PlayOneShot(sonidoGolpe);
                puedeHacerDaño = false; 
            }
        }
    }
}