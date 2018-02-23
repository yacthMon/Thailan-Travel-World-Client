using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform[] backgrounds;     //array (list) of all back- and forgrounds to be parallaxed
    public float[] parallaxScales;  //the proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //how smooth the parallax is going to be, Must be above 0 otherwize the parallax will not work

    private Transform cam;  //reference to the camera's transform
    private Vector3 previousCamPos;     //the position of the camera in the previous frame
    public bool show;
    //called before Start(), using to assign references.
    void Awake() {
        //set up camera the reference
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start() {
        // store previous frame
        previousCamPos = cam.position;
        /*
        //declares the length of the array
        parallaxScales = new float[backgrounds.Length];

        //assigning coresponding parallaxScales
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }*/
    }

    // Update is called once per frame
    void Update() {
        //for each background
        for (int i = 0; i < backgrounds.Length; i++) {
            //the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            if (show)
                GameObject.Find("System").GetComponent<fps>().customText = "Parallax : " + parallax + " Parallax Scale : " + previousCamPos.x + ":" + cam.position.x;
            //set a target x position that is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position which is the backgrounds current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade batween current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            
            //backgrounds[i].position += Vector3.right * parallax;
        }
        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;

    }
}
