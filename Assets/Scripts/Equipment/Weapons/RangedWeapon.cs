using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    //TODO Test this + implement animation and first projectile
    [SerializeField] private Weapon weaponSO;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator animator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire"); // Performance enhance

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Attack()
    {
        animator.SetTrigger(FIRE_HASH);
        //GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        //newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponSO.weaponRange);
    }

    public Weapon GetWeaponInfo()
    {
        return weaponSO;
    }
}
