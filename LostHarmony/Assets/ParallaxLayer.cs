using UnityEngine;

public class ParallaxRepeater : MonoBehaviour
{
    public float parallaxMultiplier = 0.5f;
    private Transform cam;
    private Vector3 lastCamPosition;

    private float spriteWidth;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;

        // Obtiene el ancho real del sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void LateUpdate()
    {
        // Movimiento parallax
        Vector3 delta = cam.position - lastCamPosition;
        transform.position += new Vector3(delta.x * parallaxMultiplier, 0, 0);
        lastCamPosition = cam.position;

        // Repetición horizontal
        float distance = cam.position.x - transform.position.x;
        if (Mathf.Abs(distance) >= spriteWidth)
        {
            float offset = (distance % spriteWidth);
            transform.position = new Vector3(cam.position.x + offset, transform.position.y, transform.position.z);
        }
    }
}
