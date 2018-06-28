using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EvilCubes.Core
{
    public class CameraManager : MonoBehaviour
    {
        [System.Serializable]
        public class CameraEvent : UnityEvent<bool> {}

        [SerializeField]
        List<PlayerCamera> mAlternativeCameras = new List<PlayerCamera>();
        [SerializeField]
        PlayerCamera mRearCamera;
        [SerializeField]
        CameraEvent mOnCameraChangedIsFPS;

        int mActiveCamera = 0;
        InputManager mInput;

        /////////////////////////////////////////////
        void Start()
        {
            mInput = GameManager.GetInstance().GetInputManager();
            if (!mInput || mAlternativeCameras.Count == 0 || mRearCamera == null)
            {
                Debug.LogWarning("CameraManager: Error in manager configuration");
                enabled = false;
            }
            //Notify is it is a fps camera
            bool isFps = mAlternativeCameras[mActiveCamera].IsFirstPerson();
            mOnCameraChangedIsFPS.Invoke(isFps);
        }

        /////////////////////////////////////////////
        void Update()
        {
            if(mAlternativeCameras.Count > 1 && mInput.GetCommandState(InputManager.Command.CHANGE_CAMERA))
               ActivateCamera((mActiveCamera + 1) % mAlternativeCameras.Count);
        }

        /////////////////////////////////////////////
        public PlayerCamera GetCurrentActiveCamera()
        {
            if (mActiveCamera < mAlternativeCameras.Count)
                return mAlternativeCameras[mActiveCamera];
            else
                return null;
        }

        /////////////////////////////////////////////
        void ActivateCamera(int index)
        {
            if(index >= 0 && index < mAlternativeCameras.Count)
            {
                mAlternativeCameras[mActiveCamera].gameObject.SetActive(false);
                mActiveCamera = index;
                mAlternativeCameras[mActiveCamera].gameObject.SetActive(true);

                //Notify is it is a fps camera
                bool isFps = mAlternativeCameras[mActiveCamera].IsFirstPerson();
                mOnCameraChangedIsFPS.Invoke(isFps);
            }
        }
    }
}
