using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    public Transform[] backgrounds; // 2 fondos hijos
    private float viewZone = 10f;
    private int leftIndex;
    private int rightIndex;
    private Transform cam;
    private float backgroundSize;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        backgroundSize = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        leftIndex = 0;
        rightIndex = 1;
    }

    void Update()
    {
        // Parallax movement SOLO en los fondos (no en el padre)
        float deltaX = cam.position.x - lastCamPos.x;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(deltaX * parallaxSpeed, 0, 0);
        }

        lastCamPos = cam.position;

        // Scroll logic
        if (cam.position.x < backgrounds[leftIndex].position.x + viewZone)
            ScrollLeft();

        if (cam.position.x > backgrounds[rightIndex].position.x - viewZone)
            ScrollRight();
    }

    private void ScrollLeft()
    {
        // Mueve el fondo de la derecha al lado izquierdo
        backgrounds[rightIndex].position = new Vector3(
            backgrounds[leftIndex].position.x - backgroundSize,
            backgrounds[leftIndex].position.y,
            backgrounds[leftIndex].position.z
        );

        // Cambia los índices
        int temp = rightIndex;
        rightIndex = leftIndex;
        leftIndex = temp;
    }

    private void ScrollRight()
    {
        // Mueve el fondo de la izquierda al lado derecho
        backgrounds[leftIndex].position = new Vector3(
            backgrounds[rightIndex].position.x + backgroundSize,
            backgrounds[rightIndex].position.y,
            backgrounds[rightIndex].position.z
        );

        // Cambia los índices
        int temp = leftIndex;
        leftIndex = rightIndex;
        rightIndex = temp;
    }
}
