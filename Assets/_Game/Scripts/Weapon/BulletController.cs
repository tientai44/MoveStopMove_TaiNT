using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum WeaponType
{
    Axe,
    Stick,
    Knife,
    Boomerang,
    Arrow,
    Candy1,
    Uzi
};

public class BulletController : MonoBehaviour
{
    [SerializeField] protected float rotateSpeed=10f;
    protected Rigidbody rb;
    protected float timer=0;
    protected float timeExist = 1.5f;
    CharacterController character;
    CharacterController owner;
    //public string tagWeapon;
    public WeaponType tagWeapon;
    public float Timer { get => timer; set => timer = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //character = other.GetComponent<CharacterController>();
            //BulletPool.GetInstance().ReturnGameObject(this.gameObject);
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(),gameObject);
            other.GetComponent<PlayerController>().OnDeath();
            owner.UpPoint(1);
        }
        if(other.tag == "Bot")
        {
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(), gameObject);
            other.GetComponent<BotController>().ChangeState(new DieState());
            owner.UpPoint(1);
        }
    }
    public void SetOwner(CharacterController character)
    {
        owner = character;
    }
}
