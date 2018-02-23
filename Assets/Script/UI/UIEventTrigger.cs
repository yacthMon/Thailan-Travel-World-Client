using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventTrigger : MonoBehaviour {

	public void OnDrag() {
        Vector2 mousePos = Input.mousePosition;
        Debug.Log(mousePos);
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        Debug.Log("after : " +mousePos);
        GetComponent<RectTransform>().localPosition = mousePos;
    }
}
