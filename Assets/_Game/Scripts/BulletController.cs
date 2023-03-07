using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] protected float rotateSpeed=10f;
    protected Rigidbody rb;
    protected float timer=0;
    protected float timeExist = 3f;
    CharacterController character;
    public string tagWeapon;
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
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon,gameObject);
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
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon,gameObject);
            other.GetComponent<PlayerController>().OnDeath();
            //if(character is BotController)
            //{
            //    other.GetComponent<BotController>().ChangeState(new DieState());
            //}
        }
        if(other.tag == "Bot")
        {
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon, gameObject);
            other.GetComponent<BotController>().ChangeState(new DieState());

        }
    }
    
}
