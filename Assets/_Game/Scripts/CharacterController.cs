using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Player,Bot, 
}
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
    [SerializeField] protected WeaponType currentWeapon;
    
    protected Material currentPantMaterial;
    private Collider characterCollider;
    protected CharacterController targetAttack;
    protected float delayAttack = 0.1f;
    protected float attackTime = 1f;
    protected float timer = 0;
    protected bool isReadyAttack=false;
    protected float waitThrow = 0.4f;
    protected int point=0;
    [SerializeField]private float rangeDetect;
    private float intialRadiusSightZone;
    [SerializeField] SphereCollider sightZone;
    [SerializeField] protected Transform weaponPos;
    [SerializeField] protected GameObject weaponHold;
    [SerializeField] protected Head currentHead;
    [SerializeField] protected GameObject headShow;
    [SerializeField] protected Shield currentShield;
    [SerializeField] protected GameObject shieldShow;
    [SerializeField]protected Transform headPos;
    [SerializeField] protected SkinnedMeshRenderer pantMeshRender;
    [SerializeField] protected Transform shieldPos;
    public List<CharacterController> l_AttackTarget = new List<CharacterController>();

    public List<CharacterController> L_AttackTarget { get => l_AttackTarget; set => l_AttackTarget = value; }
    public Collider CharacterCollider { get => characterCollider; set => characterCollider = value; }


    private Transform tf;
    private Transform weaponHoldTransform;
    private Transform sightZoneTransform;
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
    public Transform WeaponHoldTransform
    {
        get
        {
            if (weaponHoldTransform == null)
            {
                weaponHoldTransform = weaponHold.transform;
            }
            return weaponHoldTransform;
        }
    }
    public Transform SightZoneTransform
    {
        get
        {
            if(sightZoneTransform == null)
            {
                sightZoneTransform = sightZone.transform;
            }
            return sightZoneTransform;
        }
    }
    public bool IsDead = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterCollider = GetComponent<Collider>();
        intialRadiusSightZone = sightZone.radius;
        
    }

    // Update is called once per frame
    virtual protected void Update()
    {

    }
    public virtual void OnInit()
    {
        point = 0;
        ChangeEquipment(currentWeapon);
        weaponHold.SetActive(true);
        L_AttackTarget.Clear();
        IsDead = false;
    }
    public virtual void Run() { }
    public virtual void SetTargetDirect(Vector3 targetPos)
    {
        
        transform.LookAt(targetPos);
        
    }
    public virtual void OnDeath()
    {
        StopAllCoroutines();
        SoundManager2.GetInstance().PlaySound(Constant.DEATH_MUSIC_NAME);
        ChangeAnim(Constant.ANIM_DIE);
        IsDead = true;
    }
    public virtual void Attack() {
        
        SetTargetDirect(targetAttack.transform.position);
        ChangeAnim(Constant.ANIM_ATTACK);
        isReadyAttack = false;
        Vector3 direct = throwPoint.position - transform.position;
        StartCoroutine(IEThrow(direct));

    }
    public IEnumerator IEThrow(Vector3 direct)
    {
        yield return new WaitForSeconds(waitThrow);
        weaponHold.SetActive(false);
        //SoundManager.GetInstance().PlayOneShot(SoundManager.GetInstance().attackSound);
        SoundManager2.GetInstance().PlaySound("Nem vu khi");
        //GameObject bullet = BulletPool.GetInstance().GetGameObject(throwPoint.position);
        BulletController bullet = GameObjectPools.GetInstance().GetFromPool(currentWeapon.ToString(),throwPoint.position).GetComponent<BulletController>();
        bullet/*.GetComponent<BulletController>()*/.tagWeapon = currentWeapon;
        bullet.TF.rotation = transform.rotation;
        bullet/*.GetComponent<Rigidbody>()*/.AddForce(direct.x * force_Throw, 0, direct.z * force_Throw);
        bullet/*.GetComponent<BulletController>()*/.SetOwner(this);
        bullet.TF.localScale *= (1 + 0.1f * point);
        yield return new WaitForSeconds(attackTime*0.5f);
        weaponHold.SetActive(true);
    }
    public void ChangeAnim(string animName)
    {
        //doan nay a sai
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);

            currentAnimName = animName;
            
            anim.SetTrigger(currentAnimName);
        }
    }
    public void ChangeEquipment(WeaponType weapon)
    {
        SetWeapon(weapon);
    }
    public void SetWeapon(WeaponType weapon)
    {
        GameObjectPools.GetInstance().ReturnToPool(GameObjectPools.GetInstance().weaponHolds[currentWeapon].ToString(), weaponHold);
        this.currentWeapon = weapon; 
        this.weaponHold = GameObjectPools.GetInstance().GetFromPool(GameObjectPools.GetInstance().weaponHolds[weapon].ToString(), weaponPos.position);
        //TODO: cache transform
        this.weaponHold.transform.SetParent(weaponPos);
        this.weaponHold.transform.rotation = new Quaternion(0, 0, 0, 0);
        //this.WeaponHoldTransform.SetParent(weaponPos);
        SightZoneTransform.localScale =new Vector3(1f,1f,1f)*StaticData.RangeWeapon[weapon];
    }
    public void SetPant(Material material)
    {
        currentPantMaterial = material;
        pantMeshRender.material = material;
    }
    public void SetHead(Head head)
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentHead.ToString(), headShow);
        }
        catch
        {
            //Debug.Log("L?i Thu h?i Head");
        }
        this.currentHead = head;
        this.headShow = GameObjectPools.GetInstance().GetFromPool(currentHead.ToString(), headPos.position);
        headShow.transform.SetParent(headPos);
        headShow.transform.rotation = new Quaternion(0, 0, 0, 0);   
    }
    public void SetShield(Shield shield)
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentShield.ToString(), shieldShow);
        }
        catch
        {
            //Debug.Log("Can not Return Shield");
        }
        this.currentShield = shield;
        this.shieldShow = GameObjectPools.GetInstance().GetFromPool(currentShield.ToString(), shieldPos.position);
        shieldShow.transform.SetParent(shieldPos);
        shieldShow.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    public void SetFullSet(Set)
    {

    }
    public void UpPoint(int point)
    {
        this.point += point;
        if(this is PlayerController)
        {
            //TODO: vi phong nguyen tac dong goi
            GameController.GetInstance().cameraFollow.Offset += new Vector3(0,1,-1);
        }
        TF.localScale = Vector3.one * this.point * 0.1f + Vector3.one;
        
    }
}
