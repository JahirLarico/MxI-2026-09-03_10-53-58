using UnityEngine;

public class Isaac_healt : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int corazonesMaximos = 6;

    [SerializeField] private int corazonesActuales;

    [Header("Invulnerabilidad")]
    [SerializeField] private float tiempoInvulnerabilidad = 1f;

    private bool esInvulnerable = false;


    private void Start()
    {
        corazonesActuales = corazonesMaximos;
    }


    public void RecibirDanio(int cantidad)
    {
        if (esInvulnerable)
        {
            return;
        }

        corazonesActuales -= cantidad;

        if (corazonesActuales < 0)
        {
            corazonesActuales = 0;
        }


        StartCoroutine(Invulnerabilidad());

        if (corazonesActuales <= 0)
        {
            Morir();
        }
    }


    private System.Collections.IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;


        yield return new WaitForSeconds(tiempoInvulnerabilidad);

        esInvulnerable = false;

    }


    private void Morir()
    {
    }
}
