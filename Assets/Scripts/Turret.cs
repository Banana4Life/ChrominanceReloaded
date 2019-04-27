using UnityEngine;

public class Turret : MonoBehaviour
{

    public TurretVariant[] variants;
    public int variant = 0;

    private TurretVariant turretVariant;
    
    public GameObject head;

    public GameObject projectileContainer;
    
    private float offsetLR = -1f;

    private float lastShot;

    public GameObject lockOnEnemy;

    private float headAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turretVariant = variants[variant];

        if (lockOnEnemy == null)
        {
            lockOnEnemy = FindNewEnemy();
        }

        LookAtEnemy();
        ShootAtEnemy();
    }

    void ShootAtEnemy()
    {
        lastShot -= Time.deltaTime * 50;
        if (lastShot <= 0)
        {
            lastShot = turretVariant.shootCooldown;
            var offset = Quaternion.Euler(0, 0, headAngle) * Vector3.left * turretVariant.offset * offsetLR;
            offsetLR *= -1;
            var p = Instantiate(turretVariant.projectile, transform.position + offset, Quaternion.Euler(0, 0, headAngle), projectileContainer.transform);
            p.GetComponent<Projectile>().speed = turretVariant.speed;
        }
    }

    GameObject FindNewEnemy()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            //enemy.gameObject.transform.position
            return enemy.gameObject;
        }

        return null;
    }

    void LookAtEnemy()
    {
        if (lockOnEnemy != null)
        {
            var dir = lockOnEnemy.transform.position - transform.position;
            headAngle = - Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            
            var headRenderer = head.GetComponentInChildren<SpriteRenderer>();
                    headRenderer.sprite = turretVariant.turretHead;
                    head.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        }
    }

}
