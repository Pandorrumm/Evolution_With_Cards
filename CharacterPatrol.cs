using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CharacterPatrol : MonoBehaviour
{
    [SerializeField] private bool patrolWaiting = false;
    [SerializeField] private float totalWaitTime = 3f;
    public List<WayPoint> patrolPoints = new List<WayPoint>();

    private NavMeshAgent navMeshAgent;
    private int currentPatrolIndex;
    public bool travelling = true;
    private bool waiting;
    private bool patrolForward;
    private float waitTimer;
    private Character character;
    public bool isMoveOnNavMesh = true;

    public float moveSpeed = 0;
    [SerializeField] private float rotateOffset = 0;
    private new Rigidbody rigidbody;
    [SerializeField] private float timeMoved = 0f;
    public float time;
    Vector2 dir;

    private void OnEnable()
    {
        SpawnerObjects.StartMoveCharacterCloneEvent += StartMove;
    }

    private void OnDisable()
    {
        SpawnerObjects.StartMoveCharacterCloneEvent -= StartMove;
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        character = GetComponent<Character>();
        rigidbody = GetComponent<Rigidbody>();

        
        time = timeMoved;
    }

    private void Update()
    {
        if (!character.isManualControl)
        {
            if (isMoveOnNavMesh)
            {
                if (travelling && navMeshAgent.remainingDistance <= 0.1f)
                {
                    travelling = false;

                    if (patrolWaiting)
                    {
                        waiting = true;
                        waitTimer = 0f;
                    }
                    else
                    {
                        ChangePatrolPoints();
                        MoveOnNavMesh();
                    }
                }

                if (waiting)
                {
                    waitTimer += Time.deltaTime;

                    if (waitTimer >= totalWaitTime)
                    {
                        waiting = false;

                        ChangePatrolPoints();
                        MoveOnNavMesh();
                    }
                }
            }
            else
            {
                if (travelling)
                {
                    MoveWithoutNavMesh();   

                     //Move(dir, moveSpeed);
                     //time -= Time.deltaTime;
                }
            }
        }
    }

    private void StartMove()
    {
        if (isMoveOnNavMesh)
        {
            if (navMeshAgent == null)
            {
                Debug.LogError("error " + gameObject.name);
            }
            else
            {
                if (patrolPoints != null && patrolPoints.Count >= 2)
                {
                    currentPatrolIndex = 0;
                    MoveOnNavMesh();
                }
                else
                {
                    Debug.Log("not points");
                }
            }
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                MoveWithoutNavMesh();
            }
        }
    }

    private void MoveWithoutNavMesh()
    {
        if (patrolPoints.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].transform.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].transform.position) < 0.1f)
            {
                travelling = false;
                ChangePatrolPoints();
            }
        }
    }

    private void MoveOnNavMesh()
    {
        if (patrolPoints.Count > 0)
        {           
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            //Debug.Log(patrolPoints[currentPatrolIndex].name);
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangePatrolPoints()
    {

        if (patrolPoints.Count > 0)
        {
             int randomIndex = Random.Range(0, patrolPoints.Count);
            currentPatrolIndex = (patrolPoints.Count - 1) - randomIndex;
            dir = new Vector2(patrolPoints[currentPatrolIndex].transform.position.x, -patrolPoints[currentPatrolIndex].transform.position.z);

        }

        if (!isMoveOnNavMesh)
        {
            travelling = true;
        }      
    }
}
