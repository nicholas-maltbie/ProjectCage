using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Character;

public class SpeedOnPath : MonoBehaviour
{
    public float originalSpeed;
    public float onPathSpeed;
    public float onPathPercentage = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = this.GetComponent<CharacterMovement>().runSpeed;
        onPathSpeed = originalSpeed * onPathPercentage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Path")
        {
            this.GetComponent<CharacterMovement>().runSpeed = onPathSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Path")
        {
            this.GetComponent<CharacterMovement>().runSpeed = originalSpeed;
        }
    }
}
