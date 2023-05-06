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
    string nameCharacter ;
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
    private int point = 0;
    [SerializeField]private float rangeDetect;
    private float intialRadiusSightZone;
    [SerializeField] private int numBotToLevelUp = 3;
    [SerializeField] SphereCollider sightZone;
    [SerializeField] protected Transform weaponPos;
    [SerializeField] protected GameObject weaponHold;
    [SerializeField] protected HeadType currentHead;
    [SerializeField] protected GameObject headShow;
    [SerializeField] protected ShieldType currentShield;
    [SerializeField] protected GameObject shieldShow;
    [SerializeField]protected Transform headPos;
    [SerializeField] protected SkinnedMeshRenderer pantMeshRender;
    [SerializeField] protected Transform shieldPos;
    [SerializeField] protected Transform wingPos;
    [SerializeField] protected GameObject wingShow;
    [SerializeField] protected WingType currentWingType;
    [SerializeField] protected TailType currentTailType;
    [SerializeField] protected GameObject tailShow;
    [SerializeField] protected Transform tailPos;
    [SerializeField] protected SetType currentSetType;
    [SerializeField] protected ParticleSystem bloodSystem;
    [SerializeField] protected ParticleSystem levelUpSystem;
    [SerializeField] protected ParticleSystem appearSystem;
    [SerializeField] private SkinnedMeshRenderer colorSkin;
    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected Transform foot;
    [SerializeField] Transform posPoinTag;
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

    public ParticleSystem BloodSystem { get => bloodSystem; set => bloodSystem = value; }
    public int Point { get => point; set => point = value; }
    public SkinnedMeshRenderer ColorSkin { get => colorSkin; set => colorSkin = value; }
    public Transform Foot { get => foot; set => foot = value; }
    public string Name { get => nameCharacter; set => nameCharacter = value; }
    public int NumBotToLevelUp { get => numBotToLevelUp; set => numBotToLevelUp = value; }
    public Transform PosPoinTag { get => posPoinTag; set => posPoinTag = value; }

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
        
        SetTargetDirect(targetAttack.TF.position);
        ChangeAnim(Constant.ANIM_ATTACK);
        isReadyAttack = false;
        //Vector3 direct = throwPoint.position - TF.position;
        Vector3 direct = targetAttack.TF.position - throwPoint.position;
        float max = Mathf.Max(Mathf.Abs(direct.x),Mathf.Abs(direct.y),Mathf.Abs(direct.z));
        direct /= max;
        StartCoroutine(IEThrow(direct));

    }
    public IEnumerator IEThrow(Vector3 direct)
    {
        yield return new WaitForSeconds(waitThrow);
        if (currentWeapon is WeaponType.Uzi == false)
        {
            weaponHold.SetActive(false);
        }
        //SoundManager.GetInstance().PlayOneShot(SoundManager.GetInstance().attackSound);
        SoundManager2.GetInstance().PlaySound(Constant.ATTACK_MUSIC_NAME);
        //GameObject bullet = BulletPool.GetInstance().GetGameObject(throwPoint.position);
        BulletController bullet = GameObjectPools.GetInstance().GetFromPool(currentWeapon.ToString(),throwPoint.position).GetComponent<BulletController>();
        bullet.tagWeapon = currentWeapon;
        bullet.TF.rotation = transform.rotation;
        if (currentWeapon is WeaponType.Uzi)
        {
            bullet.AddForce(direct.x * force_Throw*10, direct.y * force_Throw*10, direct.z * force_Throw*10);
        }
        else 
            bullet.AddForce(direct.x * force_Throw, direct.y*force_Throw, direct.z * force_Throw);
        bullet.SetOwner(this);
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
        if (weaponHold != null)
        {
            GameObjectPools.GetInstance().ReturnToPool(GameObjectPools.GetInstance().weaponHolds[currentWeapon].ToString(), weaponHold);
        }
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
        if (material == null)
        {
            pantMeshRender.gameObject.SetActive(false);
        }
        else
        {
            pantMeshRender.gameObject.SetActive(true);
        }
        currentPantMaterial = material;
        pantMeshRender.material = material;
    }
    public void SetHead(HeadType head)
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentHead.ToString(), headShow);
            headShow = null;
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
    public void SetShield(ShieldType shield)
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentShield.ToString(), shieldShow);
            shieldShow = null;
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
    public void SetWing(WingType wing) {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentWingType.ToString(), wingShow);
            wingShow = null;
        }
        catch
        {
        }
        this.currentWingType = wing;
        this.wingShow = GameObjectPools.GetInstance().GetFromPool(currentWingType.ToString(), wingPos.position);
        wingShow.transform.SetParent(wingPos);
        //wingShow.transform.rotation = Quaternion.identity;
        wingShow.transform.rotation = new Quaternion(0, 0, 0, 0);

    }
    public void SetTail(TailType tail)
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentTailType.ToString(), tailShow);
            tailShow = null;
        }
        catch
        {
        }
        this.currentTailType = tail;
        this.tailShow = GameObjectPools.GetInstance().GetFromPool(currentTailType.ToString(), tailPos.position);
        tailShow.transform.SetParent(tailPos);
        tailShow.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    public void SetFullSet(SetType set)
    {
        RemoveAllEquip();
        currentSetType = set;
        Equipment infor = GameObjectPools.GetInstance().SetValue[set];
        try
        {
            SetPant(GameObjectPools.GetInstance().pantMaterials[infor.IdPant - 1]);
        }
        catch
        {
            SetPant(null);
        }
        if (infor.HeadName != null)
        {
            SetHead(StaticData.HeadEnum[infor.HeadName]);
        }
        if (infor.WingName != null)
        {
            SetWing(StaticData.WingEnum[infor.WingName]);
        }
        if (infor.TailName != null)
        {
            SetTail(StaticData.TailEnum[infor.TailName]);
        }
        if (infor.ShieldName != null)
        {
            SetShield(StaticData.ShieldEnum[infor.ShieldName]);
        }
        if(infor.IdColor >0)
        {
            SetColorSkin(GameObjectPools.GetInstance().characterMaterial[infor.IdColor - 1]);
        }
    }
    public void SetColorSkin( Material material)
    {
        colorSkin.material = material;
    }
    public void RemoveAllEquip()
    {
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentTailType.ToString(), tailShow);
            tailShow = null;
        }
        catch
        {
        }
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentWingType.ToString(), wingShow);
            wingShow = null;
        }
        catch
        {
        }
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentShield.ToString(), shieldShow);
            shieldShow = null;
        }
        catch
        {
            //Debug.Log("Can not Return Shield");
        }
        try
        {
            GameObjectPools.GetInstance().ReturnToPool(currentHead.ToString(), headShow);
            headShow = null;
        }
        catch
        {
            //Debug.Log("L?i Thu h?i Head");
        }
        colorSkin.material = defaultMaterial;
    }
    public virtual void UpPoint(int point)
    {
        this.point += point;
        //if(this is PlayerController)
        //{
        //    //TODO: vi phong nguyen tac dong goi
        //    GameController.GetInstance().cameraFollow.Offset += new Vector3(0,1,-1);
        //}
        if (this.point % numBotToLevelUp == 0)
        {
            TF.localScale = Vector3.one * this.point * 0.1f + Vector3.one;
            levelUpSystem.Play();
            //GameObject effect= GameObjectPools.GetInstance().GetFromPool("LevelUp", foot.position);
            //effect.transform.SetParent(this.TF);
            //effect.transform.localScale *= TF.localScale.x;
            //StartCoroutine(IEBackEffect(2f, effect, "LevelUp"));
        }
        
    }
    IEnumerator IEBackEffect(float time,GameObject go,string tag)
    {
        yield return new WaitForSeconds(time);
        GameObjectPools.GetInstance().ReturnToPool(tag, go);
    }
    
}
