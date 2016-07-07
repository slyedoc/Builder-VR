using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


using VRTK;
public class BuilderController : MonoBehaviour
{    
    private LevelManager levelMgr;

    public GameObject rodPrefab;


    void Awake()
    {
        if (rodPrefab == null)
        {
            Debug.LogError("Rod Prefab not set!");
        }
    }

    void Start()
    {
        levelMgr = FindObjectOfType<LevelManager>();


        GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler( DoApplicationMenuClicked);
        //GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripClicked);
        //GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
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

    void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {

        if (!GetComponent<VRTK_ControllerEvents>().gripPressed)
        {
            var placedRoot = GameObject.FindGameObjectsWithTag("PlacedRoot");
            var rodClone = Instantiate(rodPrefab);
            rodClone.SetActive(true);
            rodClone.name = "rod clone";
            rodClone.transform.position = this.transform.position;
            rodClone.transform.rotation = this.transform.rotation; //take rotation from controller
            rodClone.transform.Rotate(90, 0, 0);   //rotate so its inline with controller. 
            rodClone.transform.parent = placedRoot[0].transform;
        }
    }
}
