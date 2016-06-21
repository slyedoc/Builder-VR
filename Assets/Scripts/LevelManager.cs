using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour {

    public bool Playing = false;

    public GameObject items;
    private GameObject clones;

    public void TogglePlaying()
    {
        Playing = !Playing;
        if (Playing)
        { 
            Clone();            
        }
        else
        { 
            Reset();
        }
    }


    private void Reset()
    {
        //destory clones
        GameObject.DestroyImmediate(clones);
        items.SetActive(true);
    }

    private void Clone()
    {
        clones = GameObject.Instantiate(items);
        items.SetActive(false);

        var plays = clones.GetComponentsInChildren<IPlay>().ToList();
        foreach (var p in plays)
        {
            p.Play();
        }

        
    }
}
