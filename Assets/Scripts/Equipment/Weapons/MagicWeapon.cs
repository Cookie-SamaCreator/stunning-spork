using UnityEngine;
using UnityEngine.Rendering;

public class MagicWeapon : MonoBehaviour, IWeapon
{
    //TODO implement a damage zone + animation
    [SerializeField] private Weapon weaponSO;
    [SerializeField] private GameObject magicAttackObject;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator animator;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
    }

    public Weapon GetWeaponInfo()
    {
        return weaponSO;
    }
    
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        Vector3 playerPosition = PlayerController.Instance.transform.position;

        float angle = Mathf.Atan2(mousePos.y - playerPosition.y, Mathf.Abs(mousePos.x - playerPosition.x)) * Mathf.Rad2Deg;
        bool isLeft = mousePos.x < playerScreenPoint.x;

        if (isLeft)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
