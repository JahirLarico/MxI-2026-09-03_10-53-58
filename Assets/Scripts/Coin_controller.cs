
using UnityEngine;

public class Coin_controller : Comun_Animations_Items
{
    [Header("VALOR DE LA MONEDA")]
    [SerializeField] private int cantidadMonedas = 1;

    [Header("SONIDO DE RECOLECCIÓN")]
    [SerializeField] private AudioClip sonidoMoneda;
    [SerializeField] private float volumenSonido = 1f;

    private bool recogida = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogida) return;

        if (other.CompareTag("Player"))
        {
            if (Coin_counter_controller.instancia != null)
            {
                recogida = true;

                Coin_counter_controller.instancia
                    .AgregarMonedas(cantidadMonedas);

                // Reproducir sonido al recoger la moneda
                if (sonidoMoneda != null)
                {
                    AudioSource.PlayClipAtPoint(
                        sonidoMoneda,
                        transform.position,
                        volumenSonido
                    );
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning(
                    "No se encontró el contador de monedas."
                );
            }
        }
    }
}
