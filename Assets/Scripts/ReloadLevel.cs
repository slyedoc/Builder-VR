using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReloadLevel : MonoBehaviour {

	// Update is called once per frame
	public void Reload () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
