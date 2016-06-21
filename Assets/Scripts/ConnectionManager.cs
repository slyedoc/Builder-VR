using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ConnectionManager : MonoBehaviour
{
    private List<Connector> connections;

    public bool EnableSnap { get; set; }

    public void Start()
    {
        connections = GetComponentsInChildren<Connector>().ToList();
    }

    public void HitConnector(Connector sender, Connector target)
    {
        //disable both connectors
        sender.gameObject.SetActive(false);
        target.gameObject.SetActive(false);
        
        //create connector, and join connectors
        var joint = CreateJoint(target);
        if (joint)
        {
            //place connectors on self
            IHasConnection hc1 = GetComponent(typeof(IHasConnection)) as IHasConnection;
            hc1.ReplaceConnection(sender, joint);

            //replace on target
            IHasConnection hc2 = target.transform.parent.gameObject.GetComponent(typeof(IHasConnection)) as IHasConnection;
            hc2.ReplaceConnection(target, joint);

            joint.Snap(target);
            joint.Snap(sender);
        }

    }

    public void HitJoint(Connector sender, ConnectionJoint joint)
    {
        //disable our connector
        sender.gameObject.SetActive(false);

        //snap to our connector
        joint.Snap(sender);
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
