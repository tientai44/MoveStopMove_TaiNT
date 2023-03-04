using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _rotateSpeed;
    [SerializeField] protected float force_Throw;
    string currentAnimName;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] protected Transform throwPoint;
    [SerializeField] private string tagWeapon;
    private Collider characterCollider;
    protected CharacterController targetAttack;
    protected float delayAttack = 0.1f;
    protected float attackTime = 2f;
    protected float timer = 0;
    protected bool isReadyAttack=false;
    protected float waitThrow = 0.8f;

    public List<CharacterController> l_AttackTarget = new List<CharacterController>();

    public List<CharacterController> L_AttackTarget { get => l_AttackTarget; set => l_AttackTarget = value; }
    public Collider CharacterCollider { get => characterCollider; set => characterCollider = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        
    }
    public virtual void Run() { }
    public virtual void SetTargetDirect(Vector3 targetPos)
    {
        
        transform.LookAt(targetPos);
        
    }
    public virtual void OnDeath()
    {
        StopAllCoroutines();
        ChangeAnim("die");
    }
    public virtual void Attack() {
        
        SetTargetDirect(targetAttack.transform.position);
        ChangeAnim("attack");
        isReadyAttack = false;
        Vector3 direct = throwPoint.position - transform.position;
        StartCoroutine(Throw(direct));

    }
    public IEnumerator Throw(Vector3 direct)
    {
        yield return new WaitForSeconds(waitThrow);
        //GameObject bullet = BulletPool.GetInstance().GetGameObject(throwPoint.position);
        GameObject bullet = GameObjectPools.GetInstance().GetFromPool(tagWeapon,throwPoint.position);
        bullet.GetComponent<BulletController>().tagWeapon = tagWeapon;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Rigidbody>().AddForce(direct.x * force_Throw, 0, direct.z * force_Throw);
    }
    public void ChangeAnim(string animName)
    {
        
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);

            currentAnimName = animName;
            
            anim.SetTrigger(currentAnimName);
        }
    }
    
}
