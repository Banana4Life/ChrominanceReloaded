using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public TurretVariant variant;
    
    // Start is called before the first frame update
    public float dieAfterSeconds = 5f;

    private void OnEnable()
    {
        Invoke(nameof(Die), dieAfterSeconds);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Die));
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * variant.speed * (transform.rotation * Vector3.up);
    }
}
