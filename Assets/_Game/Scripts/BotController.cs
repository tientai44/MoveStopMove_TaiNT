using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BotController : CharacterController
{
    [SerializeField] Transform spawnPosTrans;
    [SerializeField] GameObject skin;
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
        OnInit();
        
    }
    public void SetRandomTargetFollow()
    {
        //TODO: cach string
        ChangeAnim("run");
        List<Transform> targets = new List<Transform>();
        BotController bot;
        //TODO: neu co the thi dung fot thay foreach
        foreach(Transform t in l_targetFollow)
        {
            if (t.gameObject.TryGetComponent<BotController>(out bot))
            {
                if (bot.CurrentState is not DieState && bot.gameObject.activeSelf==true)
                    targets.Add(t);
            }
            else
            {
                targets.Add(t);
            }
        }
        if (targets.Count > 0)
        {
            targetFollow = targets[Random.Range(0, targets.Count)];
            destination = targetFollow.position;
            agent.destination = destination;
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        foreach (Transform t in GameController.GetInstance().L_character)
        {
            if (!t.Equals(transform) && !l_targetFollow.Contains(t))
                l_targetFollow.Add(t);
        }
        weaponHold.SetActive(true);
        skin.SetActive(true);
        CharacterCollider.enabled = true;
        ChangeState(new IdleState());
    }
    // Update is called once per frame
    public void FollowTarget()
    {
        ChangeAnim("run");
        if(targetFollow ==null)
        {
            SetRandomTargetFollow();
            return;
        }
        BotController bot;
        if (targetFollow.TryGetComponent<BotController>(out bot))
        {
            if(bot.CurrentState is DieState)
            {
                SetRandomTargetFollow();
            }
        }
        if (Vector3.Distance(destination, targetFollow.position) > 1.0f)
        {
            destination = targetFollow.position;
            agent.destination = destination;
        }
    }
    protected override void Update()
    {
        //base.Update();
        //TODO: lam the nao de bot phai getcomponent o day
        if (targetAttack!=null&&targetAttack.GetComponent<CharacterController>() is BotController) 
        if(targetAttack.GetComponent<BotController>().IsDead)
        {
            l_AttackTarget.Remove(targetAttack);
        }
        if (targetAttack != null && targetAttack.GetComponent<CharacterController>() is PlayerController)
            if (targetAttack.GetComponent<PlayerController>().MyState is PlayerState.Death)
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
        skin.SetActive(false);
        if (!GameController.GetInstance().isSpawnEnemy())
        {
            GameObjectPools.GetInstance().ReturnToPool(CharacterType.Bot.ToString(), this.gameObject);
            return;
        }
        GameController.GetInstance().NumSpawn -= 1;
        OnInit();
        TF.position = GameController.GetInstance().GetRandomSpawnPos();
    }
    public override void OnDeath()
    {
        
        base.OnDeath();
        GameController.GetInstance().UpdateAliveText();

    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        destination = TF.position;
        agent.destination = destination;
    }

    public void ChangeState(IState newState)
    {
        //TODO: check null nang cao
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
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
