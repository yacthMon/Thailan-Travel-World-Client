using UnityEngine;
using UnityEngine.UI;
public class connectCanvasToCamera : MonoBehaviour {
    public int sortingOrder;
    void Start () {
        Canvas canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.sortingOrder = sortingOrder;
    }
	
}
