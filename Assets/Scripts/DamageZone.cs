using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        RubtController controller = other.GetComponent<RubtController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
