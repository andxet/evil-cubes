using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        FirstPersonCamera mFirstPersonCamera;
        [SerializeField]
        ThirdPersonCamera mThirdPersonCamera;
        [SerializeField]
        Camera mRetroCamera;
        InputManager mInput;
        float mRotationSensibility = 1.0f;
        bool mInit = false;


        // Use this for initialization
        void Awake()
        {
            //Check only prefabs internal objects, the other objects will be injected by the GameManager
            if(mFirstPersonCamera == null || mThirdPersonCamera == null || mRetroCamera == null)
            {
                Debug.LogError("PlayerManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
        }

        public void Init(InputManager mgr, float rotationSensibility)
        {
            if (!enabled)
                return;
            mInput = mgr;
            mRotationSensibility = rotationSensibility;
            mFirstPersonCamera.Init(mInput, mRotationSensibility);
            mInit = true;
            //mTPC.Init(mInput, mRotationSensibility);
        }

        // Update is called once per frame
        void Update()
        {
            if (!mInit)
                return;
            if(mInput != null)
            {
                float mouse_x = mInput.GetMouseState(InputManager.MouseState.X_DELTA);
                transform.rotation *= Quaternion.AngleAxis(mouse_x * mRotationSensibility, new Vector3(0, 1, 0));
            }
        }
    }
}
