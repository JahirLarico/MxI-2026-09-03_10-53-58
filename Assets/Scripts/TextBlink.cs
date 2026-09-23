using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextBlink : MonoBehaviour
{
    private TextMeshProUGUI texto;

    public float velocidad = 3f;

    private Vector3 escalaOriginal;

    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        float efecto = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;


        float alpha = Mathf.Lerp(0.6f, 1f, efecto);

        Color color = texto.color;
        color.a = alpha;

        texto.color = color;


        float escala = Mathf.Lerp(1f, 1.05f, efecto);

        transform.localScale = escalaOriginal * escala;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
