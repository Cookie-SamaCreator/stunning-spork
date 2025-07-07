using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Weapon weaponSO;
    private Transform attackSpawnPoint;
    private Transform slashSpawnPoint;
    private Animator animator;
    private GameObject slashAnimation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        attackSpawnPoint = PlayerController.Instance.GetAttackSpawnPoint();
        slashSpawnPoint = PlayerController.Instance.GetAttackAnimSpawnPoint();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public Weapon GetWeaponInfo()
    {
        return weaponSO;
    }
    
    public void Attack()
    {
        animator.SetTrigger("Attack");
        slashAnimation = Instantiate(slashAnimationPrefab, slashSpawnPoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
    }

    public void SwingUpFlipAnim()
    {
        slashAnimation.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnim()
    {
        slashAnimation.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        Vector3 playerPosition = PlayerController.Instance.transform.position;

        float angle = Mathf.Atan2(mousePos.y - playerPosition.y, Mathf.Abs(mousePos.x - playerPosition.x)) * Mathf.Rad2Deg;
        bool isLeft = mousePos.x < playerScreenPoint.x;

        float x = Mathf.Abs(attackSpawnPoint.localPosition.x);
        Vector3 newPos = attackSpawnPoint.localPosition;
        Quaternion newRot;

        if (isLeft)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            newPos.x = -x;
            newRot = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            newPos.x = x;
            newRot = Quaternion.Euler(0, 0, 0);
        }

        attackSpawnPoint.SetLocalPositionAndRotation(newPos, newRot);
    }
}
