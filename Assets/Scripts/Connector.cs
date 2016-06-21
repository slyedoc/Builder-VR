using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Connector : MonoBehaviour {

    private ConnectionManager connectionManager;
    
    
    public void Start()
    {
        connectionManager = GetComponentInParent<ConnectionManager>();
    }
        
    private void OnTriggerEnter(Collider collider)
    {
        //ignore if not enabled
        if (!connectionManager.EnableSnap)
            return;

        Connector collidedItem = collider.GetComponent<Connector>();
        if (collidedItem )
        {
            connectionManager.HitConnector(this, collidedItem);
        }

        ConnectionJoint cj = collider.GetComponent<ConnectionJoint>();
        if ( cj )
        {
            connectionManager.HitJoint(this, cj);
        }
    }


}
