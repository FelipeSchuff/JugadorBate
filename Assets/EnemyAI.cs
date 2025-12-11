using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public int dañoGolpe = 10;

    [Header("Ajuste de Tiempo")]
    public float retardoImpacto = 1.07f; 
    public float tiempoEntreAtaques = 2.0f; 

    public float walkSpeed = 1.0f; 
    public float runSpeed = 3.0f;  
    public float wanderRadius = 8f;

    private NavMeshAgent agent;
    private Animator animator;
    private float wanderTimer;
    private float cooldownTimer = 0f;
    private bool estaAtacando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaAtacando) return; 


        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        bool canSeePlayer = false;

        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
        {
            if (hit.transform == player) canSeePlayer = true;
        }

        if (canSeePlayer)
        {
            EngageTarget();
        }
        else
        {
            Wander();
        }
    }

    void EngageTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {

            agent.isStopped = false;
            agent.speed = runSpeed; 
            agent.SetDestination(player.position);
            animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime); 
        }
        else
        {

            agent.isStopped = true;
            agent.velocity = Vector3.zero;


            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookPos);

            animator.SetFloat("Speed", 0f); 


            if (cooldownTimer <= 0)
            {
                StartCoroutine(EjecutarAtaque());
            }
        }
    }

    void Wander()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed; 
        animator.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);

        wanderTimer += Time.deltaTime;
        if (wanderTimer >= 4.0f || agent.remainingDistance < 0.8f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0;
        }
    }

    IEnumerator EjecutarAtaque()
    {
        estaAtacando = true;
        

        animator.Play("Attack", 0, 0f); 

        yield return new WaitForSeconds(retardoImpacto);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange + 1.0f) 
        {
            VidaJugador vida = player.GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDaño(dañoGolpe);
            }
        }


        yield return new WaitForSeconds(0.5f); 

        cooldownTimer = tiempoEntreAtaques; 
        estaAtacando = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}