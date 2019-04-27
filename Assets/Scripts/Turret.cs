using System;
using UnityEngine;

[RequireComponent((typeof(GameObjectPool)))]
public class Turret : MonoBehaviour
{
    public TurretVariant[] variants;
    public int variant = 0;

    public Boolean disabled = false;
    
    private TurretVariant turretVariant;

    public GameObject head;

    public GameObject projectileContainer;

    private float offsetLR = -1f;

    private float lastShot;

    public GameObject lockOnEnemy;

    private float desiredAngle;
    private float headAngle;

    private Vector3 aimLocation;

    private Boolean isAimed;

    private GameObjectPool projectilePool;

    // Start is called before the first frame update
    void Start()
    {
        projectilePool = GetComponent<GameObjectPool>();
    }

    public TurretVariant getVariant()
    {
        turretVariant = variants[variant];
        return turretVariant;
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled)
        {
            return;
        }
        turretVariant = variants[variant];
        projectilePool.prefab = turretVariant.projectile;
        this.name = turretVariant.displayName;

        if (lockOnEnemy == null)
        {
            lockOnEnemy = FindNewEnemy();
        }

        AimAtEnemy();
        ShootAtEnemy();
    }

    void ShootAtEnemy()
    {
        if (isAimed && lastShot <= 0)
        {
            lastShot = turretVariant.shootCooldown;
            var offset = Quaternion.Euler(0, 0, headAngle) * Vector3.left * turretVariant.offset * offsetLR;
            offsetLR *= -1;
            var obj = projectilePool.Get();
            obj.transform.position = transform.position + offset;
            obj.transform.rotation = Quaternion.Euler(0, 0, headAngle);
            obj.GetComponent<Projectile>().variant = turretVariant;
        }

        lastShot -= Time.deltaTime * 50;
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

    void AimAtEnemy()
    {
        isAimed = false;
        if (lockOnEnemy != null)
        {
            aimLocation = getIntersection(lockOnEnemy);
            if (aimLocation != Vector3.zero)
            {
                var dir = aimLocation - transform.position;
                //var dir = lockOnEnemy.transform.position - transform.position;
                desiredAngle = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                isAimed = RotateTo(desiredAngle, turretVariant.maxRotationPerTick);

                var headRenderer = head.GetComponentInChildren<SpriteRenderer>();
                headRenderer.sprite = turretVariant.turretHead;
                head.transform.rotation = Quaternion.Euler(0, 0, headAngle);    
            }
        }
    }

    private Boolean RotateTo(float newAngle, float maxRotationPerTick)
    {
        var delta = Time.deltaTime;

        newAngle = getAngleInLimits(newAngle);
        float rotation = headAngle;
        float leftRotation = getAngleInLimits(rotation - newAngle);
        float rightRotation = getAngleInLimits(newAngle - rotation);

        if (Mathf.Abs(rotation - newAngle) <= maxRotationPerTick * delta)
        {
            headAngle = newAngle;
        }
        else if (leftRotation < rightRotation)
        {
            headAngle = rotation - (maxRotationPerTick * delta);
        }
        else
        {
            headAngle = rotation + (maxRotationPerTick * delta);
        }

        headAngle = getAngleInLimits(headAngle);

        return Mathf.Abs(headAngle - newAngle) < 0.0001;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimLocation, 0.25f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, headAngle) * Vector3.up * 20);
        
        if (lockOnEnemy != null)
        {
            var enemy = lockOnEnemy.GetComponent<Enemy>();
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(lockOnEnemy.transform.position, lockOnEnemy.transform.position + lockOnEnemy.transform.rotation * Vector3.up * enemy.speed);    
        }
        
    }


    /**
     * Calculates the given angle to the range of 0-360
     * @param angle
     */
    private float getAngleInLimits(float angle)
    {
        if (angle < 0)
        {
            angle = 360 - (Mathf.Abs(angle) % 360);
        }
        else if (angle > 360)
        {
            angle %= 360;
        }

        return angle;
    }

    public Vector3 getIntersection(GameObject target)
    {
        return getIntersection(target, 0, 0);
    }

    private Vector3 getIntersection(GameObject target, int iteration, float i)
    {
        
        Vector3 ownPosition = transform.position;
        float bulletSpeed = turretVariant.speed;

        var targetEnemy = target.GetComponent<Enemy>();

        Vector3 targetPosNext = targetEnemy.getPosAt(iteration + 1);
        Vector3 targetPosition = targetEnemy.getPosAt(iteration);

        Vector3 targetVelocity = targetPosNext - targetPosition;


        float partOne = 2f * ((targetVelocity.x * targetVelocity.x) + (targetVelocity.y * targetVelocity.y) - (bulletSpeed * bulletSpeed));
        float partTwo = 2f * ((targetPosition.x * targetVelocity.x) + (targetPosition.y * targetVelocity.y) - (ownPosition.x * targetVelocity.x) - (ownPosition.y * targetVelocity.y) - (i * bulletSpeed * bulletSpeed));
        float partThree = (targetPosition.x * targetPosition.x) - (2f * targetPosition.x * ownPosition.x) + (targetPosition.y * targetPosition.y) - (2f * targetPosition.y * ownPosition.y) + (ownPosition.x * ownPosition.x) + (ownPosition.y * ownPosition.y) - (i * i * bulletSpeed * bulletSpeed);
        
        float x = (1f / partOne) * (-Mathf.Sqrt((partTwo * partTwo) - (2f * partOne * partThree)) - partTwo);


        Vector3 intersectionPos = targetVelocity * Mathf.Abs(x) + targetPosition;


        var dist = (intersectionPos - targetPosition).magnitude / targetVelocity.magnitude;
        
        if (dist > 5)
        {
            return Vector3.zero;
            //return getIntersection(target, iteration + 1, x + i);
        }

        return intersectionPos;
    }
}