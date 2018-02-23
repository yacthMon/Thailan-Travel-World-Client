using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowFacebookDetail : MonoBehaviour {
	void Start () {
        Authentication auth = Authentication.Instance;
        if (auth.fbLogin) {            
            transform.GetChild(0).GetComponent<Text>().text = auth.fbName;
            transform.GetChild(1).GetComponent<Image>().sprite = auth.fbDisplay;
        } else {
            this.gameObject.SetActive(false);
        }        
    }

    private void FixedUpdate() {
        
    }
}
