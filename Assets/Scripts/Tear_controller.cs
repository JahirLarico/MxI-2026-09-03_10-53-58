using Unity.VisualScripting;
using UnityEngine;

public class Tear_controller : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    public float velocidad = 10f;
    public int dano = 10;

    [Header("TIEMPO DE VIDA")]
    public float tiempoVida = 3f;

    private Vector2 direccion;


    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }


    private void Update()
    {
        transform.Translate(
            direccion * velocidad * Time.deltaTime
        );
    }


    public void Configurar(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemie"))
        {
            Gazling_controller enemigo =
                collision.GetComponentInParent<Gazling_controller>();

            if (enemigo != null)
            {
                enemigo.RecibirDanio(dano);
            }

        }
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);

        }
    }
}
