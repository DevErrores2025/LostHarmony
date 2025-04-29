using UnityEngine;

public class TrashProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;
    public Animator animator;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        Destroy(gameObject, lifetime); // Autodestruir tras X segundos
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           // other.GetComponent<PlayerHealth>().TakeDamage(damage);
           // --PlayerHealth no se encuentra en mi script, quitar comentario y poner variable adecuada
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // tecla o control de lanzamiento de Dama
        {
            animator.SetTrigger("StartAttack");
        }
    }
}
