using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    //TODO Test this + implement animation and first projectile
    [SerializeField] private Weapon weaponSO;
    [SerializeField] private Transform projectileSpawnPoint;

    private Animator animator;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack"); // Performance enhance

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
        //GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        //newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponSO.weaponRange);
    }

    public Weapon GetWeaponInfo()
    {
        return weaponSO;
    }

    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }
}
