using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public AudioClip collectedClip;
    
   
void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {

            {
            	controller.boosting = true;
                controller.speed = controller.speedBoost;
                
                
                Destroy(gameObject);
            	controller.PlaySound(collectedClip);
            }
        }

    
        
    }
}

