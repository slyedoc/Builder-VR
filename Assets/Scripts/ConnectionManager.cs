using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRTK;


public delegate void ConnectionManagerEventHandler(object sender);

public class ConnectionManager : MonoBehaviour
{
    //Pairing of connection and the joints that replaced them
    public List<Tuple<Connector, ConnectionJoint>> connections;
    public bool EnableSnap { get; set; }

    public event ConnectionManagerEventHandler ConnectionCreated;

    public virtual void OnConnectionCreated()
    {
        if (ConnectionCreated != null)
            ConnectionCreated(this);
    }

    public void Start()
    {
        connections = new List<Tuple<Connector, ConnectionJoint>>();
        foreach (var c in GetComponentsInChildren<Connector>().ToList())
        {
            connections.Add( new Tuple<Connector, ConnectionJoint>( c, null));
        }
    }

    public void HitConnector(Connector sender, Connector target)
    {

        //disable both connectors
        sender.gameObject.SetActive(false);
        target.gameObject.SetActive(false);

        //create connector, and join connectors
        var joint = CreateJoint(target);
        if (joint == null)
        {
            Debug.LogError("Error creating joint.");
            return;
        }

        //update pairing with new joint
        UpdateConnectorPairing(sender, joint);
            
        //replace on target
        ConnectionManager cm2 = target.transform.parent.gameObject.GetComponent<ConnectionManager>();
        cm2.UpdateConnectorPairing(target, joint);

        joint.Snap(target);
        joint.Snap(sender);

        OnConnectionCreated();
        cm2.OnConnectionCreated();
    }

    private void UpdateConnectorPairing(Connector child, ConnectionJoint target)
    {
        var conn = connections.Find(a => a.first == child);
        if (conn == null)
        {
            Debug.LogError("Could not find Connector in list.");
            return;
        }
        conn.second = target;
    }

    public void HitJoint(Connector sender, ConnectionJoint joint)
    {

        //disable our connector
        sender.gameObject.SetActive(false);

        //update pairing
        UpdateConnectorPairing(sender, joint);

        //snap to our connector
        joint.Snap(sender);

        OnConnectionCreated();
    }

    

    private ConnectionJoint CreateJoint(Connector item)
    {
        //create connection joint
        var path = "ConnectionJoint";
        var prefab = Resources.Load(path);
        if (prefab == null)
        {
            Debug.LogError("Couldn't load prefab: Assets/Resources/" + path);
            return null;
        }
        var obj = (GameObject)Instantiate(prefab, item.transform.position, item.transform.rotation);
        var cj = obj.GetComponent<ConnectionJoint>();
        cj.transform.parent = transform.parent;
        return cj;
    }

}
