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
    [SerializeField] protected float rotateSpeed=10f;
    protected Rigidbody rb;
    protected float timer=0;
    protected float timeExist = 1.5f;
    CharacterController character;
    CharacterController owner;
    //public string tagWeapon;
    public WeaponType tagWeapon;
    [SerializeField] float range;
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
        if (other.tag == "Obstacle")
        {
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(), gameObject);
        }
        if (other.tag == "Player")
        {
            //character = other.GetComponent<CharacterController>();
            //BulletPool.GetInstance().ReturnGameObject(this.gameObject);
            owner.UpPoint(1);
            GameObjectPools.GetInstance().ReturnToPool(tagWeapon.ToString(),gameObject);
            other.GetComponent<PlayerController>().OnDeath();
            SoundManager.GetInstance().PlayOneShot(SoundManager.GetInstance().killSound);


        }
        if(other.tag == "Bot")
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
}
