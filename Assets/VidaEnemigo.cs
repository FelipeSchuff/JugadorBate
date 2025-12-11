using UnityEngine;
using UnityEngine.AI;

public class VidaEnemigo : MonoBehaviour
{
    public int vidaMaxima = 100;
    private int vidaActual;
    
    private Animator animator;
    private EnemyAI scriptIA;
    private Collider miCollider;
    private NavMeshAgent agente;

    void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
        scriptIA = GetComponent<EnemyAI>();
        miCollider = GetComponent<CapsuleCollider>();
        agente = GetComponent<NavMeshAgent>();
    }

    public void RecibirGolpe() 
    {
        if (vidaActual <= 0) return;

        vidaActual -= 25;
        
        if (animator != null) animator.SetTrigger("Hurt"); 

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        if(animator != null) animator.SetTrigger("Die");

        if(scriptIA != null) scriptIA.enabled = false;
        if(agente != null) agente.enabled = false;

        if(miCollider != null) miCollider.enabled = false;

        Destroy(gameObject, 5f);
    }
}