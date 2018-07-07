using System.Collections;
using System.Collections.Generic;
using EvilCubes.Core;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    [SerializeField]
    LayerMask mLayerMask;
    [HideInInspector]
    public Vector3 HitPoint { get; private set; }

    CameraManager mCameraManager;


    /////////////////////////////////////////////
	void Start () {
        mCameraManager = GameManager.GetInstance().GetCameraManager();
        if(mCameraManager == null)
        {
            Debug.LogError("CrossHair: bad initialization.");
            enabled = false;
        }
	}
	
    /////////////////////////////////////////////
	void Update () {
        PlayerCamera cam = mCameraManager.GetCurrentActiveCamera();
        if(cam == null)
        {
            Debug.LogWarning("CrossHair: received bad camera.");
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, mLayerMask))
            HitPoint = hit.point;
        else
            HitPoint = cam.transform.forward * 100;
	}

    /////////////////////////////////////////////
    /*void OnDrawGizmos()
    {
        PlayerCamera cam = mCameraManager.GetCurrentActiveCamera();
        if (cam == null)
        {
            return;
        }
        RaycastHit hit;
        Gizmos.color = Color.blue;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, mLayerMask))
            Gizmos.DrawCube(hit.point, new Vector3(0.1f,0.1f,0.1f));
    }*/
}
