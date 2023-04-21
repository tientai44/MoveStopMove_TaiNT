using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

enum PlayerState
{
    Attacked,Attacking,Death,Run,Idle,Win
}
public class PlayerController : CharacterController
{
    private Vector3 moveVector;
    //[SerializeField]private FixedJoystick _joystick;
    private FloatingJoystick _joystick;
    [SerializeField]PlayerState myState;
    float timerDeath = 0f;
    [SerializeField]CombatText combatTextPrefab;
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
        //_joystick = FindObjectOfType<FixedJoystick>();
        //TODO: can than
        _joystick = FindObjectOfType<FloatingJoystick>();
        if (_joystick != null)
        {
            SightZoneTransform.gameObject.SetActive(true);
            _joystick.OnInit();
            //Debug.Log("Appear");
            appearSystem.Play();
        }
        else
        {
            SightZoneTransform.gameObject.SetActive(false);
        }
        GameController.GetInstance().cameraFollow.SetTargetFollow(this);
        timerDeath = 0;
        myState = PlayerState.Idle;
        ChangeAnim(Constant.ANIM_IDLE);
        ChangeEquipment(StaticData.WeaponEnum[SaveLoadManager.GetInstance().Data1.WeaponCurrent]);
        Equip();
        
    }

    public void Equip()
    {
        RemoveAllEquip();
        try
        {
            SetFullSet(StaticData.SetEnum[SaveLoadManager.GetInstance().Data1.SetCurrent]);
        }
        catch
        {
            Debug.Log("No Set");
        }
        int index = SaveLoadManager.GetInstance().Data1.IdPantMaterialCurrent;

        if (index > 0)
            SetPant(GameObjectPools.GetInstance().pantMaterials[index - 1]);
        else
        {
            SetPant(null);
        }
        try
        {
            SetHead(StaticData.HeadEnum[SaveLoadManager.GetInstance().Data1.HeadCurrent]);
        }
        catch
        {
            Debug.Log("No Head ");
        }
        try
        {
            SetShield(StaticData.ShieldEnum[SaveLoadManager.GetInstance().Data1.ShieldCurent]);
        }
        catch
        {
            Debug.Log("No Shield");
        }
    }
    // Update is called once per frame
    protected override void  Update()
    {
        if (_joystick == null || myState == PlayerState.Win )
        {
            return;
        }
        if ( myState is PlayerState.Death)
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
            (targetAttack as BotController).EnableCircleTarget();
        }
        if (!L_AttackTarget.Contains(targetAttack) && targetAttack != null)
        {
            (targetAttack as BotController).UnEnableCircleTarget();
        }
        Run();
        if (targetAttack != null && targetAttack.IsDead)
        {
            (targetAttack as BotController).UnEnableCircleTarget();
            L_AttackTarget.Remove(targetAttack);
            if (l_AttackTarget.Count > 0)
            {
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
                (targetAttack as BotController).EnableCircleTarget();
            }
        }
        if (l_AttackTarget.Count > 0  )
        {
            if (!l_AttackTarget.Contains(targetAttack))
            {
                if (targetAttack != null)
                {
                    (targetAttack as BotController).UnEnableCircleTarget();
                }
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
                (targetAttack as BotController).EnableCircleTarget();
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
            if (myState == PlayerState.Attacked || myState == PlayerState.Attacking || myState == PlayerState.Win)
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
        //if (myState != PlayerState.Win)
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
        StaticData.CoinGet = Point;
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

    public override void UpPoint(int point)
    {
        base.UpPoint(point);
        VibrateController.GetInstance().Vibrate(0.1f);
        if (this.Point % numBottoLevelUp == 0)
        {
            GameController.GetInstance().cameraFollow.Offset += new Vector3(0, 1, -1)*numBottoLevelUp;
            Instantiate(combatTextPrefab, TF.position + new Vector3(1f*TF.localScale.x, 1f * TF.localScale.y, 0), Quaternion.identity).OnInit(1+Point/(10));
        }
    }
    public void Win()
    {
        StaticData.CoinGet = Point;
        myState = PlayerState.Win;
        ChangeAnim(Constant.ANIM_DANCE);
        
        SoundManager2.GetInstance().PlaySound(Constant.WIN_MUSIC_NAME);
        StopAllCoroutines();
    }
}
