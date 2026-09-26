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
        // Si Isaac está invulnerable, no recibe daño
        if (esInvulnerable)
        {
            return;
        }

        corazonesActuales -= cantidad;

        // Evitamos que la vida sea negativa
        if (corazonesActuales < 0)
        {
            corazonesActuales = 0;
        }

        Debug.Log("Isaac recibió daño. Corazones actuales: " + corazonesActuales);

        // Comenzamos la invulnerabilidad
        StartCoroutine(Invulnerabilidad());

        // Comprobamos si murió
        if (corazonesActuales <= 0)
        {
            Morir();
        }
    }


    private System.Collections.IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;

        Debug.Log("Isaac es invulnerable durante " + tiempoInvulnerabilidad + " segundos.");

        yield return new WaitForSeconds(tiempoInvulnerabilidad);

        esInvulnerable = false;

        Debug.Log("Isaac ya no es invulnerable.");
    }


    private void Morir()
    {
        Debug.Log("Isaac ha muerto.");
    }
}
