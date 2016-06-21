using UnityEngine;
using System.Collections;

public class DestroyAtDepth : MonoBehaviour {

    public int depth = -1000;

	void FixedUpdate () {
        if (transform.position.y < -1000)
            Destroy(gameObject);
	}
}
