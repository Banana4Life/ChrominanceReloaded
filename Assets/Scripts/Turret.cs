using System;
using UnityEngine;

[RequireComponent((typeof(GameObjectPool)))]
public class Turret : MonoBehaviour
{
    public TurretVariant[] variants;
    public ColorVariant[] colorVariants;
    public int variant = 0;
    public int colorVariant = 0;

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

    public Color color = Color.green;
    public Gradient gradient = new Gradient();

    private GameObjectPool projectilePool;

    private float enemyIntersectTime;
    
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
        UpdateColor();
        UpdateVariant();
        
        if (disabled)
        {
            return;
        }

        if (lockOnEnemy == null || !lockOnEnemy.activeSelf)
        {
            lockOnEnemy = FindNewEnemy();
        }

        AimAtEnemy();
        ShootAtEnemy();
    }
    
    void UpdateColor()
    {
        color = colorVariants[colorVariant].color;
        gradient = colorVariants[colorVariant].gradient;
    }
    
    void UpdateVariant()
    {
        turretVariant = variants[variant];
        projectilePool.prefab = turretVariant.projectile;
        name = turretVariant.displayName;
        
        var rend = head.GetComponentInChildren<SpriteRenderer>();
        var newSprite = turretVariant.getSprite(color);
        rend.sprite = newSprite;
    }

    void ShootAtEnemy()
    {
        if (isAimed && lastShot <= 0)
        {
            lastShot = turretVariant.shootCooldown;
            var offset = Vector3.left * turretVariant.offset * offsetLR;
            offsetLR *= -1;
            
            var pla = head.GetComponentInChildren<ParticleLauncher>();
            pla.Shoot(turretVariant, offset, gradient);
        }

        lastShot -= Time.deltaTime * 50;
    }

    GameObject FindNewEnemy()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.gameObject.activeSelf)
            {
                // TODO improve targeting systems
                return enemy.gameObject;
            }
        }

        return null;
    }

    void AimAtEnemy()
    {
        isAimed = false;
        if (lockOnEnemy != null)
        {
            aimLocation = getIntersection(lockOnEnemy.GetComponent<PathFollower>());
            if (aimLocation != Vector3.zero)
            {
                var dir = aimLocation - transform.position;
                //var dir = lockOnEnemy.transform.position - transform.position;
                desiredAngle = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                isAimed = RotateTo(desiredAngle, turretVariant.maxRotationPerTick);

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
            var enemy = lockOnEnemy.GetComponent<PathFollower>();
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(lockOnEnemy.transform.position, lockOnEnemy.transform.position + lockOnEnemy.transform.rotation * Vector3.up * enemy.speed);
            
            // Intersection Spheres
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, enemyIntersectTime * turretVariant.speed);
            Gizmos.DrawWireSphere(lockOnEnemy.transform.position, enemyIntersectTime * enemy.speed);
            
            // Enemy Velocity Spheres
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lockOnEnemy.transform.position, enemy.speed);
            Gizmos.DrawWireSphere(lockOnEnemy.transform.position, enemy.speed * 2);
            Gizmos.DrawWireSphere(lockOnEnemy.transform.position, enemy.speed * 3);
            
            // Last Enemy Source Node
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(tNode, 0.1f);
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

    public Vector3 getIntersection(PathFollower target)
    {
        return getIntersection(target, 0, 0);
    }

    private Vector3 getIntersection(PathFollower pathFollower, int iteration, float ti)
    {
        if (iteration > 20)
        {
            Debug.Log("MAXIT");
            return pathFollower.gameObject.transform.position;
        }
        
        Vector3 chaser = transform.position;
        float vc = turretVariant.speed;

        Vector3 target;
        Vector3 targetNode;

        if (iteration == 0)
        {
            target = pathFollower.gameObject.transform.position;
            targetNode = pathFollower.GetPosAt(0);
        }
        else
        {
            target = pathFollower.GetPosAt(iteration - 1);
            targetNode = pathFollower.GetPosAt(iteration);
        }
        
        //targetNode = target + pathFollower.gameObject.transform.rotation * Vector3.forward * pathFollower.speed;

        Vector3 vtRaw = (targetNode - target);
        Vector3 vt = vtRaw.normalized * pathFollower.speed;

        var dd = (chaser - target);
        var alpha = Vector3.Angle(dd, vt) * Mathf.Deg2Rad;
        
        var t = calcTime(vc, dd.magnitude, vt, alpha, ti);

        
        var tvt = vt * Mathf.Abs(t);
        
        if ( tvt.magnitude > vtRaw.magnitude)
        {
            return getIntersection(pathFollower, iteration + 1, (t / tvt.magnitude * vtRaw.magnitude) + ti);
        }
        
        finalIterationCount = iteration;
        tNode = target;

        enemyIntersectTime = t + ti;
        
        Vector3 intersectionPos = target + tvt;
        return intersectionPos;
    }
    
    public float finalIterationCount;
    public Vector3 tNode;

    private static float calcTime(float vc, float d, Vector3 vt, float alpha, float n)
    {
        // (t . vc)2 = (t . vt)2 + d2 - 2 . d . t . vt . cos(α) 
        
        // ((v-n) * c)^2 = ((v-n) * t)^2 + d^2 - 2 * d * (v-n) * t * cos(z) 
        var sqrVc = vc * vc;

        //a = vc^2 - vt^2
        var a = sqrVc - vt.sqrMagnitude;
        //b = 2 * d * vt * cos(α)
        var b = 2 * sqrVc * n + 2 * d * vt.magnitude * Mathf.Cos(alpha);
        //c = - d2
        var c = (sqrVc * n * n) - (d * d);
        
        if (b*b - 4 * a * c < 0)
        {
            Debug.Log("ZERO");
            return 0;
        }
        
        //t = (- b ± √(b^2 - 4 * a * c)) / (2 * a)
        var t1 = (+Mathf.Sqrt(b * b - 4 * a * c) -b) / (2 * a);
        var t2 = (-Mathf.Sqrt(b * b - 4 * a * c) -b) / (2 * a);
        return Mathf.Max(t1, t2);
    }
}