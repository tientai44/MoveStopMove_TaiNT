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
    float timerDeath = 0f;
    internal PlayerState MyState { get => myState; set => myState = value; }

    //bool isAttack = false;
    //bool isAttacking = false;
    //bool isDeath = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        OnInit();
    }
    public override void OnInit()
    {
        base.OnInit();
        _joystick = FindObjectOfType<FixedJoystick>();
        if(_joystick !=null)
            _joystick.OnInit();
        GameController.GetInstance().cameraFollow.SetTargetFollow(transform);
        timerDeath = 0;
        myState = PlayerState.Idle;
        ChangeAnim(Constant.ANIM_IDLE);
        ChangeEquipment(StaticData.WeaponEnum[SaveLoadManager.GetInstance().Data1.WeaponCurrent]);
        int index = SaveLoadManager.GetInstance().Data1.IdPantMaterialCurrent;
        if(index>0)
            SetPant(GameObjectPools.GetInstance().pantMaterials[index-1]);
        else
        {
            SetPant(GameObjectPools.GetInstance().pantMaterials[0]);
        }
         
        SetHead(StaticData.HeadEnum[SaveLoadManager.GetInstance().Data1.HeadCurrent]);
    }

    // Update is called once per frame
    protected override void  Update()
    {
        if( myState is PlayerState.Death)
        {
            timerDeath +=Time.deltaTime;
            if (timerDeath > 2f)
            {
                GameObjectPools.GetInstance().ReturnToPool(CharacterType.Player.ToString(), this.gameObject);
                GameController.GetInstance().Lose();
            }
            return;
        }
        if (myState is PlayerState.Attacked )
        {
            return;
        }
        if (targetAttack != null)
        {
            targetAttack.GetComponent<BotController>().EnableCircleTarget();
        }
        if (!L_AttackTarget.Contains(targetAttack)&targetAttack!=null)
        {
            targetAttack.GetComponent<BotController>().UnEnableCircleTarget();
        }
        Run();
        if (targetAttack != null && targetAttack.GetComponent<CharacterController>().IsDead)
        {
            L_AttackTarget.Remove(targetAttack);
            if (l_AttackTarget.Count > 0)
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
        }
        if (l_AttackTarget.Count > 0  )
        {
            if (!l_AttackTarget.Contains(targetAttack))
            {
                
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
            }
        }
        
        if (l_AttackTarget.Contains(targetAttack) && timer>=delayAttack)
        {
            Attack();
            timer = 0;
        }
       
    }
    public override void Run()
    {
        if (_joystick == null)
        {
            return;
        }
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
            ChangeAnim(Constant.ANIM_RUN);
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
                ChangeAnim(Constant.ANIM_IDLE);
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

        SoundManager2.GetInstance().PlaySound(Constant.LOSE_MUSIC_NAME);
        base.OnDeath();
        
        
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
