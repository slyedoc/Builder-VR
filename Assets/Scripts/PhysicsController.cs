using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


using VRTK;
public class PhysicsController : MonoBehaviour
{    
    private LevelManager levelMgr;

    void Start()
    {
        levelMgr = FindObjectOfType<LevelManager>();


        GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler( DoApplicationMenuClicked);
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripClicked);

        //GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerClickedEventHandler(DoTriggerAxisChanged);
        //GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerClickedEventHandler(DoTouchpadAxisChanged);
        //GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerClickedEventHandler(DoTriggerUnclicked);
        //GetComponent<VRTK_ControllerEvents>().TouchpadUntouched += new ControllerClickedEventHandler(DoTouchpadUntouched);

        //GetComponent<VRTK_ControllerEvents>().ApplicationMenuClicked += new ControllerClickedEventHandler(DoApplicationMenuClicked);
    }

    void DoApplicationMenuClicked(object sender, ControllerInteractionEventArgs e)
    {
        levelMgr.TogglePlaying();
    }

    void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
