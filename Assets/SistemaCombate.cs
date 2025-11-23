using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

public class SistemaCombate : MonoBehaviour
{
    [Header("Configuración")]
    public Animator animator;
    public GameObject armaEnMano;    
    public GameObject armaEnEspalda; 

    private bool enModoCombate = false;
    private StarterAssets.StarterAssetsInputs _input;

    void Start()
    {
        _input = GetComponentInParent<StarterAssets.StarterAssetsInputs>();

        if(armaEnMano != null) armaEnMano.SetActive(false);
        if(armaEnEspalda != null) armaEnEspalda.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            enModoCombate = !enModoCombate;
            animator.SetBool("ModoCombate", enModoCombate);

            if (armaEnMano != null && armaEnEspalda != null)
            {
                Invoke("CambiarArmaVisual", 1.0f); 
            }
        }

        if (enModoCombate && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Atacar");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Rodar");
        }
    }

    void CambiarArmaVisual()
    {
        if (enModoCombate)
        {
            armaEnMano.SetActive(true);
            armaEnEspalda.SetActive(false);
        }
        else
        {
            armaEnMano.SetActive(false);
            armaEnEspalda.SetActive(true);
        }
    }

    public void InicioGolpe()
    {
        if (armaEnMano != null)
        {
            var scriptDaño = armaEnMano.GetComponent<DañoArma>();
            if (scriptDaño != null) scriptDaño.puedeHacerDaño = true;
        }
    }

    public void FinGolpe()
    {
        if (armaEnMano != null)
        {
            var scriptDaño = armaEnMano.GetComponent<DañoArma>();
            if (scriptDaño != null) scriptDaño.puedeHacerDaño = false;
        }
    }
}