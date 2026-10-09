
using UnityEngine;

public class Comun_Animations_Items : MonoBehaviour
{
    [Header("MOVIMIENTO VERTICAL")]
    [Tooltip("Velocidad del movimiento de arriba hacia abajo.")]
    public float velocidad = 2f;

    [Tooltip("Distancia máxima desde la posición inicial.")]
    public float distancia = 0.03f;

    private Vector3 posicionInicial;

    protected virtual void Start()
    {
        posicionInicial = transform.position;
    }

    protected virtual void Update()
    {
        float movimientoY =
            Mathf.Sin(Time.time * velocidad) * distancia;

        transform.position = new Vector3(
            posicionInicial.x,
            posicionInicial.y + movimientoY,
            posicionInicial.z
        );
    }
}
