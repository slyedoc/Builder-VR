using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour {

    public bool Playing = false;

    public GameObject items;

    public void TogglePlaying()
    {
        Playing = !Playing;
        var plays = items.GetComponentsInChildren<IPlay>().ToList();
        if (Playing)
        {            
            foreach (var p in plays)
            {
                p.Play();
            }
        }
        else
        {
            foreach (var p in plays)
            {
                p.Stop();
            }
        }

    }
}
