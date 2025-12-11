using UnityEngine;
using UnityEngine.UI; 
using StarterAssets;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("Interfaz")]
    public Image imagenSangre;       
    public GameObject panelGameOver;

    public Image barraVidaVerde;


    private Animator animator;
    private ThirdPersonController controlMovimiento;
    private SistemaCombate sistemaCombate;
    private StarterAssetsInputs inputs;
    private CharacterController characterController;

    void Start()
    {
        vidaActual = vidaMaxima;
        

        animator = GetComponent<Animator>();
        controlMovimiento = GetComponent<ThirdPersonController>();
        sistemaCombate = GetComponent<SistemaCombate>();
        inputs = GetComponent<StarterAssetsInputs>();
        characterController = GetComponent<CharacterController>();


        if (imagenSangre != null) imagenSangre.canvasRenderer.SetAlpha(0f);
        if (panelGameOver != null) panelGameOver.SetActive(false);


        ActualizarBarraVida();
    }

    public void RecibirDaño(int cantidad)
    {
        if (vidaActual <= 0) return;

        vidaActual -= cantidad;


        if (imagenSangre != null)
        {
            imagenSangre.canvasRenderer.SetAlpha(0.5f);
            imagenSangre.CrossFadeAlpha(0f, 0.5f, false);
        }


        ActualizarBarraVida();

        if (animator != null) animator.SetTrigger("Hurt");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }


    void ActualizarBarraVida()
    {
        if (barraVidaVerde != null)
        {
            float porcentajeVida = (float)vidaActual / vidaMaxima;
            barraVidaVerde.fillAmount = porcentajeVida;
        }
    }

    void Morir()
    {
        Debug.Log("JUGADOR ELIMINADO");

        if (animator != null) animator.SetTrigger("Die");
        if (sistemaCombate != null) sistemaCombate.enabled = false; 
        if (controlMovimiento != null) controlMovimiento.enabled = false;
        if (inputs != null) inputs.enabled = false;
        if (characterController != null) characterController.enabled = false;
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            panelGameOver.transform.SetAsLastSibling();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}