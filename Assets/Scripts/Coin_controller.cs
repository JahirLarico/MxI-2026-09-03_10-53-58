
using UnityEngine;

public class Coin_controller : Comun_Animations_Items
{
    [Header("VALOR DE LA MONEDA")]
    [SerializeField] private int cantidadMonedas = 1;

    private bool recogida = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogida) return;

        if (other.CompareTag("Player"))
        {
            recogida = true;

            if (Coin_counter_controller.instancia != null)
            {
                Coin_counter_controller.instancia.AgregarMonedas(cantidadMonedas);
                Destroy(gameObject);
            }
            else
            {
                recogida = false;
                Debug.LogWarning("No se encontró el contador de monedas.");
            }
        }
    }
}
