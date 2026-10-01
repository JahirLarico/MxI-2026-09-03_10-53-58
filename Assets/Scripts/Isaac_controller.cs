using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Isaac_controller : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public int speed = 2;
    public int jumpForce = 200;

    private bool spriteRight = true;

    public bool inGroud = true;

    public Animator animator;

    [Header("Detección del suelo")]
    public Transform feetPos;
    public float checkRadius = 0.5f;
    public LayerMask whatIsUnder;

    private BoxCollider2D box;

    [Header("Muerte")]
    public static bool death = false;

    private bool sonidoMuerteReproducido = false;

    [Header("Vida")]
    [SerializeField] private int corazonesMaximos = 6;
    [SerializeField] private int corazonesActuales;

    [Header("Invulnerabilidad")]
    [SerializeField] private float tiempoInvulnerabilidad = 1f;

    private bool esInvulnerable = false;

    [Header("Rebote al pisar enemigos")]
    public float fuerzaRebote = 5f;

    [Header("DISPARO")]
    public GameObject prefabLagrima;
    public Transform puntoDisparo;

    public float cadenciaDisparo = 1f;
    public int danoLagrima = 10;

    private float temporizadorDisparo = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        corazonesActuales = corazonesMaximos;

        death = false;
        sonidoMuerteReproducido = false;
    }


    void FixedUpdate()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(
            HorizontalMove * speed,
            rb.linearVelocity.y
        );

        animator.SetFloat(
            "Speed",
            Math.Abs(HorizontalMove)
        );


        if (Input.GetKey(KeyCode.J))
        {
            animator.SetInteger("Shoot_d", -1);
            animator.SetBool("Shoot", true);

            IntentarDisparar(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            animator.SetInteger("Shoot_d", 1);
            animator.SetBool("Shoot", true);

            IntentarDisparar(Vector2.right);
        }
        else
        {
            animator.SetInteger("Shoot_d", 0);
            animator.SetBool("Shoot", false);
        }
    }


    void Update()
    {
        if (temporizadorDisparo > 0f)
        {
            temporizadorDisparo -= Time.deltaTime;
        }

        inGroud = Physics2D.OverlapCircle(
            feetPos.position,
            checkRadius,
            whatIsUnder
        );


        if (Input.GetKeyDown(KeyCode.Space) && inGroud)
        {
            Jump();
        }


        if (death)
        {
            if (!sonidoMuerteReproducido)
            {
                AudioSource sound = GetComponent<AudioSource>();

                if (sound != null)
                {
                    sound.Play();
                }

                sonidoMuerteReproducido = true;
            }
        }
    }


    void Jump()
    {
        rb.AddForceY(jumpForce);

        inGroud = false;
    }


    public void Rebotar()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null && !death)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaRebote
            );

            inGroud = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            inGroud = true;
        }
        if (collision.gameObject.CompareTag("Pipe"))
        {
            inGroud = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            inGroud = false;
        }
        if (collision.gameObject.CompareTag("Pipe"))
        {
            inGroud = false;
        }
    }


    public void RecibirDanio(int cantidad)
    {
        if (death)
        {
            return;
        }

        if (esInvulnerable)
        {
            return;
        }

        corazonesActuales -= cantidad;


        if (corazonesActuales < 0)
        {
            corazonesActuales = 0;
        }


        if (corazonesActuales <= 0)
        {
            Morir();
            return;
        }


        StartCoroutine(Invulnerabilidad());
    }


    private System.Collections.IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;


        yield return new WaitForSeconds(
            tiempoInvulnerabilidad
        );


        esInvulnerable = false;
    }



    private void Morir()
    {
        death = true;

        Debug.Log("Isaac ha muerto.");
    }


    public int ObtenerCorazonesActuales()
    {
        return corazonesActuales;
    }

    private void IntentarDisparar(Vector2 direccion)
    {
        if (temporizadorDisparo > 0f)
        {
            return;
        }

        Disparar(direccion);

        temporizadorDisparo = cadenciaDisparo;
    }

    private void Disparar(Vector2 direccion)
    {
        if (prefabLagrima == null)
        {
            Debug.LogWarning("No has asignado el prefab de la lágrima.");
            return;
        }

        if (puntoDisparo == null)
        {
            Debug.LogWarning("No has asignado el PuntoDisparo.");
            return;
        }


        GameObject nuevaLagrima = Instantiate(
            prefabLagrima,
            puntoDisparo.position,
            Quaternion.identity
        );


        Tear_controller lagrima =
            nuevaLagrima.GetComponent<Tear_controller>();


        if (lagrima != null)
        {
            lagrima.dano = danoLagrima;
            lagrima.Configurar(direccion);
        }
    }

}