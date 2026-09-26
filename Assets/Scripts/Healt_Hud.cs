using UnityEngine;
using UnityEngine.UI;

public class Healt_Hud : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Isaac_controller isaac;

    [Header("Corazones")]
    [SerializeField] private Image[] corazones;

    [Header("Sprites")]
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;


    private void Start()
    {
        ActualizarHUD();
    }


    private void Update()
    {
        ActualizarHUD();
    }


    private void ActualizarHUD()
    {
        int vidaActual = isaac.ObtenerCorazonesActuales();

        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
            {
                corazones[i].sprite = corazonLleno;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }
}
