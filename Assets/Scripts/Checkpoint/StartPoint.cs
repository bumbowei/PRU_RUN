using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public Animator anim => GetComponent<Animator>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
            anim.SetTrigger("activate");
    }
}
