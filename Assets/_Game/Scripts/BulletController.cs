using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float rotateSpeed=5f;
    private Rigidbody rb;
    private float timer=0;
    private float timeExist = 3f;
    CharacterController character;

    public float Timer { get => timer; set => timer = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeExist)
        {
            BulletPool.GetInstance().ReturnGameObject(this.gameObject);
        }
        transform.Rotate(rotateSpeed, 0, 0);
    }
    public void ResetForce()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            character = other.GetComponent<CharacterController>();
            BulletPool.GetInstance().ReturnGameObject(this.gameObject);
            if(character is BotController)
            {
                other.GetComponent<BotController>().ChangeState(new DieState());
            }
        }
    }
    
}
