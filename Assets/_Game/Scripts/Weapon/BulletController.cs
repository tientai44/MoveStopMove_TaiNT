using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum WeaponType
{
    Axe,
    //Stick,
    Knife,
    Boomerang,
    //Arrow,
    Candy0,
    //Uzi
};

public class BulletController : MonoBehaviour
{
    private Transform tf;
    [SerializeField] protected float rotateSpeed=10f;
    protected float timer=0;
    protected float timeExist = 1.5f;
    CharacterController character;
    CharacterController owner;
    //public string tagWeapon;
    public WeaponType tagWeapon;
    [SerializeField] float range;
    protected Rigidbody rbWeapon;
    public Transform TF
    {
        get
        {
            //tf ??= GetComponent<Transform>();
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    public Rigidbody RbWeapon
    {
        get
        {
            if (rbWeapon == null)
            {
                rbWeapon = GetComponent<Rigidbody>();
            }
            return rbWeapon;
        }
    }
    public float Timer { get => timer; set => timer = value; }

    private void Start()
    {
    }
    protected virtual void Update()
    {
        
        timer += Time.deltaTime;
        if (timer > timeExist)
        {
            //BulletPool.GetInstance().ReturnGameObject(this.gameObject);
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(),gameObject);
        }
    }
    public void ResetForce()
    {
        rbWeapon.velocity = Vector3.zero;
        rbWeapon.angularVelocity = Vector3.zero;
    }
    protected void OnTriggerEnter(Collider other)
    {
        //TODO: comp?etag + cach string
        if (other.CompareTag(Constant.TAG_OBSTACLE))
        {
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(), gameObject);
        }
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            //character = other.GetComponent<CharacterController>();
            //BulletPool.GetInstance().ReturnGameObject(this.gameObject);
            owner.UpPoint(1);
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(),gameObject);
            //TODO: cache getcomponent dictionary
            //other.GetComponent<PlayerController>().OnDeath();
            Cache.GetCharacter(other).OnDeath();
            SoundManager.GetInstance().PlayOneShot(SoundManager.GetInstance().killSound);


        }
        if(other.CompareTag(Constant.TAG_BOT))
        {
            owner.UpPoint(1);
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(), gameObject);
            other.GetComponent<BotController>().ChangeState(new DieState());
            SoundManager.GetInstance().PlayOneShot(SoundManager.GetInstance().killSound);
            if(owner is PlayerController)
            {
                SaveLoadManager.GetInstance().Data1.Coin += 1;
                SaveLoadManager.GetInstance().Save();
            }

        }
    }
    
    public void SetOwner(CharacterController character)
    {
        owner = character;
    }

    public void AddForce(float v1, float v2, float v3)
    {
        RbWeapon.AddForce(v1, v2, v3);
    }
}
