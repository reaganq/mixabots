using UnityEngine;
using System.Collections;

public class UI3DCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    GUIManager.Instance.Inventory3DCamera = this.camera;
	}
}
