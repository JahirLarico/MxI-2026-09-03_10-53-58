using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class End_controller : Comun_Animations_Items
{
    [Header("CAMBIO DE SPRITE DE ISAAC")]
    public Sprite spriteNuevo;

    private bool recogido = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recogido) return;

        if (collision.CompareTag("Player"))
        {
            Isaac_controller isaac =
                collision.GetComponent<Isaac_controller>();

            if (isaac == null)
            {
                isaac = collision.GetComponentInParent<Isaac_controller>();
            }

            if (isaac != null && spriteNuevo != null)
            {
                recogido = true;

                isaac.CambiarSprite(spriteNuevo);


                Destroy(gameObject);

            }
        }
    }
}