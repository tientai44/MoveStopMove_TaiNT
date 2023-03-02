using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BotController : CharacterController
{
    [SerializeField] Transform spawnPosTrans;
    Vector3 spawnPos;
    private NavMeshAgent agent;
    private Vector3 destination;
    public Transform targetFollow;
    private IState currentState;
    private List<Transform> l_targetFollow = new List<Transform>();

    public IState CurrentState { get => currentState; set => currentState = value; }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        spawnPos = spawnPosTrans.position;
        foreach (Transform t in GameController.GetInstance().L_character)
        {
            if(!t.Equals(transform))
                l_targetFollow.Add(t);
        }
        OnInit();
        
    }
    public void SetRandomTargetFollow()
    {
        ChangeAnim("run");
        targetFollow = l_targetFollow[Random.Range(0,l_targetFollow.Count)];
        destination = targetFollow.position;
        agent.destination = destination;
    }
    public void OnInit()
    {
        CharacterCollider.enabled = true;
        ChangeState(new IdleState());
    }
    // Update is called once per frame
    public void FollowTarget()
    {
        ChangeAnim("run");
        if (Vector3.Distance(destination, targetFollow.position) > 1.0f)
        {
            destination = targetFollow.position;
            agent.destination = destination;
        }
    }
    protected override void Update()
    {
        //base.Update();
        if (targetAttack!=null&&targetAttack.GetComponent<CharacterController>() is BotController) 
        if(targetAttack.GetComponent<BotController>().CurrentState is DieState)
        {
            l_AttackTarget.Remove(targetAttack);
        }   
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }
    public void DeSpawn()
    {
        OnInit();

        transform.position = spawnPos;
    }
    public override void OnDeath()
    {
        base.OnDeath();
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        destination = transform.position;
        agent.destination = destination;
    }
    public void ChangeState(IState newState)
    {

        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public bool IsHaveTargetInRange()
    {
        if(L_AttackTarget.Count > 0)
        {
            targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
            return true;
        }
        return false;
        
    }
}
