using System.Collections;
using UnityEngine;

public class Gazling_controller : MonoBehaviour
{
    [Header("MOVIMIENTO")]
    public float speed = 0.6f;
    public bool moveRight = true;

    [Header("VIDA")]
    public int vidaMaxima = 120;
    public int vidaActual;

    [Tooltip("Daño que recibe cuando Isaac cae encima.")]
    public int danoPorPisoton = 12;

    [Header("SEGUNDA FORMA")]
    [Tooltip("Porcentaje de vida que tendrá la segunda forma.")]
    [Range(0f, 1f)]
    public float porcentajeSegundaForma = 0.50f;

    [Tooltip("Tiempo que tiene Isaac para matar la segunda forma.")]
    public float tiempoParaMatarSegundaForma = 2f;

    [Tooltip("Porcentaje de vida al volver a la forma normal.")]
    [Range(0f, 1f)]
    public float porcentajeAlRevivir = 0.75f;

    [Header("FORMAS DEL ENEMIGO")]
    public GameObject cabeza;
    public GameObject cuerpoNormal;
    public GameObject cuerpoSegundaForma;

    [Header("SONIDO")]
    public AudioSource audioSource;
    public AudioClip sonidoEnemigo;
    public float intervaloSonido = 5f;

    [Header("ANIMACIÓN")]
    public Animator animator;

    private bool isBated = false;
    private bool segundaForma = false;
    private bool puedeRecibirPisoton = true;

    private BoxCollider2D box;

    private Vector2 originalSize;
    private Vector2 originalOffset;

    private Coroutine coroutineRevivir;


    private void Start()
    {
        box = GetComponent<BoxCollider2D>();

        originalSize = box.size;
        originalOffset = box.offset;

        vidaActual = vidaMaxima;

        MostrarFormaNormal();

        StartCoroutine(ReproducirSonido());
    }


    private void Update()
    {
        if (isBated)
        {
            return;
        }

        if (moveRight)
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe") ||
            collision.gameObject.CompareTag("Enemie"))
        {
            if (moveRight)
            {
                moveRight = false;
            }
            else
            {
                moveRight = true;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                Pisoton();

            }
            else
            {
                Isaac_controller.death = true;
            }
        }
    }


    private void Pisoton()
    {
        if (!puedeRecibirPisoton)
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
        vidaActual -= cantidad;

        if (vidaActual < 0)
        {
            vidaActual = 0;
        }

        Debug.Log("Daño recibido " + cantidad +" vida actual => " + vidaActual);


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
        //isBated = true;

        //speed = 0;

        if (animator != null)
        {
            //animator.SetBool("beated", true);
        }

        float visualY = originalSize.y * 0.37f;

        box.size = new Vector2(
            originalSize.x,
            visualY
        );

        box.offset = new Vector2(
            originalOffset.x,
            originalOffset.y -
            (originalSize.y - visualY) / 2f
        );
    }


    private void ActivarSegundaForma()
    {
        animator.SetBool("beated", true);
        segundaForma = true;
        isBated = true;

        speed = 0;


        vidaActual = Mathf.CeilToInt(
            vidaMaxima * porcentajeSegundaForma
        );

        if (cabeza != null)
        {
            cabeza.SetActive(false);
        }

        if (cuerpoNormal != null)
        {
            cuerpoNormal.SetActive(false);
        }

        if (cuerpoSegundaForma != null)
        {
            cuerpoSegundaForma.SetActive(true);
        }

        if (animator != null)
        {
            animator.SetBool("beated", true);
        }


        if (coroutineRevivir != null)
        {
            StopCoroutine(coroutineRevivir);
        }

        coroutineRevivir = StartCoroutine(
            TemporizadorSegundaForma()
        );
    }


    private IEnumerator TemporizadorSegundaForma()
    {
        Debug.Log("Tiempo para revivir " + tiempoParaMatarSegundaForma );

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

        if (cuerpoSegundaForma != null)
        {
            cuerpoSegundaForma.SetActive(false);
        }

        if (cuerpoNormal != null)
        {
            cuerpoNormal.SetActive(true);
        }

        if (cabeza != null)
        {
            cabeza.SetActive(true);
        }


        box.size = originalSize;
        box.offset = originalOffset;

        speed = 0.6f;


        if (animator != null)
        {
            animator.SetBool("beated", false);
        }

        puedeRecibirPisoton = true;
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

        if (cuerpoSegundaForma != null)
        {
            cuerpoSegundaForma.SetActive(false);
        }

        if (cabeza != null)
        {
            cabeza.SetActive(true);
        }
    }


    private IEnumerator ReproducirSonido()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                intervaloSonido
            );

            if (sonidoEnemigo != null &&
                audioSource != null)
            {
                audioSource.PlayOneShot(
                    sonidoEnemigo
                );
            }
        }
    }
}