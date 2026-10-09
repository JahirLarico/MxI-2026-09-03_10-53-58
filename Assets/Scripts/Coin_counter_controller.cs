using TMPro;
using UnityEngine;

public class Coin_counter_controller : MonoBehaviour
{
    public static Coin_counter_controller instancia;

    [Header("MONEDAS")]
    [SerializeField] private int monedasActuales = 0;

    [Header("INTERFAZ")]
    [SerializeField] private TMP_Text textoMonedas;

    private void Awake()
    {
        instancia = this;
        ActualizarTexto();
    }

    public void AgregarMonedas(int cantidad)
    {
        monedasActuales += cantidad;

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = monedasActuales.ToString();
        }
    }
}
