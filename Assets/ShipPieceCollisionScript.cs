using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPieceCollisionScript : MonoBehaviour
{
    public ShipPieceScript spS;

    private void OnCollisionEnter(Collision collision)
    {
        print("collided at: " + Time.deltaTime);
        if (collision.gameObject.CompareTag("asteroid"))
            spS.Detatch();
    }
}
