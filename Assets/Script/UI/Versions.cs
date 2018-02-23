using UnityEngine.UI;
using UnityEngine;

public class Versions : MonoBehaviour {
	void Start () {
        GetComponent<Text>().text += Application.version;
	}
}
