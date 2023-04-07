using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BotController : CharacterController
{
    [SerializeField] Transform spawnPosTrans;
    [SerializeField] GameObject skin;
    [SerializeField] GameObject circleTarget;
    Vector3 spawnPos;
    private NavMeshAgent agent;
    private Vector3 destination;
    public CharacterController targetFollow;
    private IState currentState;

    private List<CharacterController> l_targetFollow = new List<CharacterController>();

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
        ChangeAnim(Constant.ANIM_RUN);
        List<CharacterController> targets = new List<CharacterController>();
        //TODO: neu co the thi dung fot thay foreach
        for(int i = 0; i < l_targetFollow.Count; i++)
        {
            if (!l_targetFollow[i].IsDead && l_targetFollow[i].gameObject.activeSelf)
            {
                targets.Add(l_targetFollow[i]);
            }
        }
        if (targets.Count > 0)
        {
            targetFollow = targets[Random.Range(0, targets.Count)];
            destination = targetFollow.TF.position;
            agent.destination = destination;
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        foreach (CharacterController t in GameController.GetInstance().L_character)
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
        ChangeAnim(Constant.ANIM_RUN);
        if(targetFollow ==null)
        {
            SetRandomTargetFollow();
            return;
        }
        if (targetFollow.GetComponent<CharacterController>().IsDead)
        {
            SetRandomTargetFollow();
        }
        if (Vector3.Distance(destination, targetFollow.TF.position) > 1.0f)
        {
            destination = targetFollow.TF.position;
            agent.destination = destination;
        }
    }
    protected override void Update()
    {
        //base.Update();
        //TODO: lam the nao de bot phai getcomponent o day
        if (targetAttack != null)
        {
            CharacterController charTarget = targetAttack.GetComponent<CharacterController>();
            if (charTarget.IsDead)
            {
                l_AttackTarget.Remove(targetAttack);
            }
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
        ChangeAnim(Constant.ANIM_IDLE);
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
    public void EnableCircleTarget()
    {
        circleTarget.SetActive(true);
    }
    public void UnEnableCircleTarget()
    {
        circleTarget.SetActive(false);
    }
}
