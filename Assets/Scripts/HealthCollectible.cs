using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubtController controller = collision.GetComponent<RubtController>();
        if(controller != null)
        {
            if(controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
            
        }
        //Debug.Log(collision);
    }
}
