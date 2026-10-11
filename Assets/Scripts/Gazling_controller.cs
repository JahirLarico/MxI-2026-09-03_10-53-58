
using System.Collections;
using UnityEngine;
using TMPro;

public class Gazling_controller : MonoBehaviour
{
    [Header("MOVIMIENTO")]
    public float speed = 0.6f;
    public bool moveRight = true;

    [Header("VIDA")]
    public int vidaMaxima = 40;
    public int vidaActual;

    [Tooltip("Daño que recibe cuando Isaac cae encima.")]
    public int danoPorPisoton = 12;

    [Header("SEGUNDA FORMA")]
    [Tooltip("Sprite nuevo que se aplicará al cuerpo.")]
    public Sprite spriteSegundaForma;

    [Range(0f, 1f)]
    public float porcentajeSegundaForma = 0.50f;

    public float tiempoParaMatarSegundaForma = 2f;

    [Range(0f, 1f)]
    public float porcentajeAlRevivir = 0.75f;

    [Header("FORMAS DEL ENEMIGO")]
    public GameObject cabeza;

    [Tooltip("Arrastra aquí el objeto hijo llamado cuerpo.")]
    public GameObject cuerpoNormal;

    [Header("SONIDO")]
    public AudioSource audioSource;
    public AudioClip sonidoEnemigo;
    public float intervaloSonido = 5f;

    [Header("ANIMACIÓN")]
    public Animator animator;

    [Header("BARRA DE VIDA")]
    public RectTransform barraVida;
    public TextMeshProUGUI textoDanio;
    public float tiempoTextoDanio = 0.5f;
    public float anchoBarraVida = 40f;

    private bool isBated = false;
    private bool segundaForma = false;
    private bool puedeRecibirPisoton = true;

    private BoxCollider2D box;
    private Collider2D colliderEnemigo;

    private SpriteRenderer spriteRendererCuerpo;
    private Sprite spriteOriginal;

    private Vector2 originalSize;
    private Vector2 originalOffset;

    private Coroutine coroutineRevivir;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        colliderEnemigo = GetComponent<Collider2D>();

        if (box != null)
        {
            originalSize = box.size;
            originalOffset = box.offset;
        }

        // Buscar exclusivamente el SpriteRenderer del cuerpo.
        if (cuerpoNormal == null)
        {
            Transform cuerpoEncontrado = transform.Find("cuerpo");

            if (cuerpoEncontrado != null)
            {
                cuerpoNormal = cuerpoEncontrado.gameObject;
            }
        }

        if (cuerpoNormal != null)
        {
            spriteRendererCuerpo =
                cuerpoNormal.GetComponent<SpriteRenderer>();
        }

        if (spriteRendererCuerpo == null)
        {
            Debug.LogError(
                "Gazling: asigna el objeto cuerpo en Cuerpo Normal y verifica que tenga SpriteRenderer.",
                this
            );
        }
        else
        {
            spriteOriginal = spriteRendererCuerpo.sprite;
        }

        vidaActual = vidaMaxima;

        MostrarFormaNormal();
        ActualizarBarraVida();

        if (intervaloSonido > 0f)
        {
            StartCoroutine(ReproducirSonido());
        }
    }

    private void Update()
    {
        if (isBated)
        {
            return;
        }

        float direccion = moveRight ? 1f : -1f;

        transform.Translate(
            direccion * 2f * Time.deltaTime * speed,
            0f,
            0f
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pipe") ||
            collision.CompareTag("Enemie"))
        {
            moveRight = !moveRight;
        }

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Isaac_controller isaac =
            collision.GetComponent<Isaac_controller>();

        if (isaac == null)
        {
            isaac =
                collision.GetComponentInParent<Isaac_controller>();
        }

        if (isaac == null)
        {
            return;
        }

        // Detectar si Isaac entra desde arriba.
        float parteInferiorIsaac = collision.bounds.min.y;
        float parteSuperiorEnemigo =
            colliderEnemigo != null
                ? colliderEnemigo.bounds.max.y
                : transform.position.y;

        if (parteInferiorIsaac >= parteSuperiorEnemigo - 0.15f)
        {
            isaac.Rebotar();
            Pisoton();
        }
        else
        {
            isaac.RecibirDanio(1);
        }
    }

    private void Pisoton()
    {
        if (!puedeRecibirPisoton || vidaActual <= 0)
        {
            return;
        }

        puedeRecibirPisoton = false;

        RecibirDanio(danoPorPisoton);

        StartCoroutine(ReactivarPisoton());
    }

    private IEnumerator ReactivarPisoton()
    {
        yield return new WaitForSeconds(0.15f);
        puedeRecibirPisoton = true;
    }

    public void RecibirDanio(int cantidad)
    {
        if (vidaActual <= 0 || cantidad <= 0)
        {
            return;
        }

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        MostrarDanio(cantidad);
        ActualizarBarraVida();

        if (vidaActual > 0)
        {
            ActivarAnimacionPisado();
            return;
        }

        if (!segundaForma)
        {
            ActivarSegundaForma();
        }
        else
        {
            MuerteDefinitiva();
        }
    }

    private void ActivarAnimacionPisado()
    {
        if (box == null)
        {
            return;
        }

        float alturaNueva = originalSize.y * 0.37f;

        box.size = new Vector2(
            originalSize.x,
            alturaNueva
        );

        box.offset = new Vector2(
            originalOffset.x,
            originalOffset.y -
            (originalSize.y - alturaNueva) / 2f
        );
    }

    private void ActivarSegundaForma()
    {
        segundaForma = true;
        isBated = true;
        speed = 0f;

        if (animator != null)
        {
            animator.SetBool("beated", true);
            animator.enabled = false;
        }

        vidaActual = Mathf.CeilToInt(
            vidaMaxima * porcentajeSegundaForma
        );

        ActualizarBarraVida();

        if (cabeza != null)
        {
            cabeza.SetActive(false);
        }

        // Cambiar solamente el sprite del cuerpo.
        if (spriteRendererCuerpo != null &&
            spriteSegundaForma != null)
        {
            spriteRendererCuerpo.sprite = spriteSegundaForma;
        }

        if (coroutineRevivir != null)
        {
            StopCoroutine(coroutineRevivir);
        }

        coroutineRevivir =
            StartCoroutine(TemporizadorSegundaForma());
    }

    private IEnumerator TemporizadorSegundaForma()
    {
        yield return new WaitForSeconds(
            tiempoParaMatarSegundaForma
        );

        if (vidaActual > 0)
        {
            Revivir();
        }
    }

    private void Revivir()
    {
        segundaForma = false;
        isBated = false;

        vidaActual = Mathf.CeilToInt(
            vidaMaxima * porcentajeAlRevivir
        );

        ActualizarBarraVida();

        // Restaurar el sprite original del cuerpo.
        if (spriteRendererCuerpo != null)
        {
            spriteRendererCuerpo.sprite = spriteOriginal;
        }

        if (cuerpoNormal != null)
        {
            cuerpoNormal.SetActive(true);
        }

        if (cabeza != null)
        {
            cabeza.SetActive(true);
        }

        if (box != null)
        {
            box.size = originalSize;
            box.offset = originalOffset;
        }

        speed = 0.6f;

        if (animator != null)
        {
            animator.enabled = true;
            animator.SetBool("beated", false);
        }

        puedeRecibirPisoton = true;
        coroutineRevivir = null;
    }

    private void MuerteDefinitiva()
    {
        Debug.Log("Gazling murió definitivamente.");
        Destroy(gameObject);
    }

    private void MostrarFormaNormal()
    {
        if (cuerpoNormal != null)
        {
            cuerpoNormal.SetActive(true);
        }

        if (cabeza != null)
        {
            cabeza.SetActive(true);
        }

        if (spriteRendererCuerpo != null)
        {
            spriteRendererCuerpo.sprite = spriteOriginal;
        }
    }

    private IEnumerator ReproducirSonido()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloSonido);

            if (sonidoEnemigo != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoEnemigo);
            }
        }
    }

    private void ActualizarBarraVida()
    {
        if (barraVida == null || vidaMaxima <= 0)
        {
            return;
        }

        float porcentaje = (float)vidaActual / vidaMaxima;

        barraVida.sizeDelta = new Vector2(
            anchoBarraVida * porcentaje,
            barraVida.sizeDelta.y
        );
    }

    private void MostrarDanio(int cantidad)
    {
        if (textoDanio == null)
        {
            return;
        }

        textoDanio.text = "-" + cantidad;
        textoDanio.gameObject.SetActive(true);

        CancelInvoke(nameof(OcultarDanio));
        Invoke(nameof(OcultarDanio), tiempoTextoDanio);
    }

    private void OcultarDanio()
    {
        if (textoDanio != null)
        {
            textoDanio.gameObject.SetActive(false);
        }
    }
}
