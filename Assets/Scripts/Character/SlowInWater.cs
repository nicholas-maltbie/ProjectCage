using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Character;

public class SlowInWater : MonoBehaviour
{
    
    public float originalSpeed;
    public float inWaterSpeed;
    public float inWaterPercentage = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = this.GetComponent<CharacterMovement>().runSpeed;
        inWaterSpeed = originalSpeed * inWaterPercentage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Water")
        {
            this.GetComponent<CharacterMovement>().runSpeed = inWaterSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Water")
        {
            this.GetComponent<CharacterMovement>().runSpeed = originalSpeed;
        }
    }

}
