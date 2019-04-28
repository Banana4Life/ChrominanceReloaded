using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public Camera cam;
    private SpriteRenderer r;

    private void Awake()
    {
        r = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var fullSize = cam.orthographicSize * 2f;
        r.size = new Vector2(fullSize * Screen.width / Screen.height, fullSize);
    }
}
