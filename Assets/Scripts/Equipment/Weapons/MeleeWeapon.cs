using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private Weapon weaponSO;
    //Transform weaponCollider;

    private Animator animator;
    private GameObject slashAnimation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //weaponCollider = PlayerController.Instance.GetAttackSpawnPoint();
        // éviter d'uliser Find et plutot utiliser comme pour weaponCollider
        // string reference c'est pas top
        //slashSpawnPoint = PlayerController.Instance.GetAttackSpawnPoint();
        slashSpawnPoint = GameObject.Find("SlashAnimationSpawnPoint").transform;
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
        //weaponCollider.gameObject.SetActive(true);
        slashAnimation = Instantiate(slashAnimationPrefab, slashSpawnPoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
    }

    //public void DoneAttackingAnim()
    //{
    //    weaponCollider.gameObject.SetActive(false);
    //}

    public void SwingUpFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

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
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            //weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            //weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
