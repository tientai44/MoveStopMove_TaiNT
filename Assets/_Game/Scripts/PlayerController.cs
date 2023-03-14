using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

enum PlayerState
{
    Attacked,Attacking,Death,Run,Idle
}
public class PlayerController : CharacterController
{
    private Vector3 moveVector;
    [SerializeField]private FixedJoystick _joystick;
    PlayerState myState;

    internal PlayerState MyState { get => myState; set => myState = value; }

    //bool isAttack = false;
    //bool isAttacking = false;
    //bool isDeath = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _joystick = FindObjectOfType<FixedJoystick>();
        GameController.GetInstance().cameraFollow.SetTargetFollow(transform);
        OnInit();
    }
    void OnInit()
    {
        
        myState = PlayerState.Idle;
        ChangeAnim("idle");
    }

    // Update is called once per frame
    protected override void  Update()
    {
        if (myState is PlayerState.Attacked || myState is PlayerState.Death)
        {
            return;
        }
       
        Run();
        if (targetAttack != null && targetAttack.GetComponent<BotController>().CurrentState is DieState)
        {
            L_AttackTarget.Remove(targetAttack);
            if (l_AttackTarget.Count > 0)
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
        }
        if (l_AttackTarget.Count > 0  )
        {
            
            if (!l_AttackTarget.Contains(targetAttack))
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
        }
        
        if (l_AttackTarget.Contains(targetAttack) && timer>=delayAttack)
        {
            Attack();
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.J)){
            Attack();
        }
    }
    public override void Run()
    {
        base.Run();
        moveVector = Vector3.zero;
        moveVector.x = _joystick.Horizontal * _moveSpeed * Time.deltaTime;
        moveVector.z = _joystick.Vertical * _moveSpeed * Time.deltaTime;
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            StopAllCoroutines();
            isReadyAttack = true;
            myState = PlayerState.Run;
            timer = 0;
            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            ChangeAnim("run");
        }
        else if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
        {
            if (myState == PlayerState.Attacked || myState == PlayerState.Attacking)
            {

            }
            else
            {
                timer += Time.deltaTime;
                myState = PlayerState.Idle;
                ChangeAnim("idle");
            }
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + moveVector, 1f);
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackTime);
        //isAttack = false;
        //isAttacking = false;
        myState = PlayerState.Idle;
    }
    IEnumerator ActiveAttack()
    {
        yield return new WaitForSeconds(waitThrow);
        //isAttack = true;
        myState = PlayerState.Attacked;
    }
    public override void OnDeath()
    {
        if(myState is PlayerState.Death)
        {
            return;
        }
        myState = PlayerState.Death;
        base.OnDeath();
        SaveLoadManager.GetInstance().Data1.Coin+= point;
        SaveLoadManager.GetInstance().Data1.WeaponCurrent = currentWeapon.ToString();
        SaveLoadManager.GetInstance().Save();
        Debug.Log("Now Coin: "+SaveLoadManager.GetInstance().Data1.Coin);
        Debug.Log("Now Weapon: " + SaveLoadManager.GetInstance().Data1.WeaponCurrent);
    }
    public override void Attack()
    {
        if (!isReadyAttack)
        {
            return;
        }
        base.Attack();
        //isAttacking = true;
        myState = PlayerState.Attacking;
        StartCoroutine(ActiveAttack());
        StartCoroutine(ResetAttack());
    }
}
