using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        bool mFirstPerson;
        [SerializeField]
        float mMaxAngleDown = 70;
        [SerializeField]
        float mMaxAngleUp = 80;

        float mSensibility;
        InputManager mInput;

        /////////////////////////////////////////////
        public void Start()
        {
            mInput = GameManager.GetInstance().GetInputManager();
            mSensibility = GameManager.GetInstance().GetAppConfig().YTurnRotation;
            if(mInput == null)
            {
                Debug.LogError("PlayerCamera: failed to init");
                enabled = false;
                return;
            }
        }

        /////////////////////////////////////////////
        void Update()
        {
            if (Time.timeScale > 0)
            {
                float x_angle = -mInput.GetMouseState(InputManager.MouseState.Y_DELTA) * mSensibility;
                x_angle += transform.rotation.eulerAngles.x;
                if (x_angle < 180 && x_angle > mMaxAngleDown)//Facing down
                    x_angle = mMaxAngleDown;
                else if (x_angle >= 180 && x_angle < 360 - mMaxAngleUp)//Facing up
                    x_angle = 360 - mMaxAngleUp;

                transform.localRotation = Quaternion.Euler(x_angle, 0, 0);
            }
        }

        /////////////////////////////////////////////
        public bool IsFirstPerson()
        {
            return mFirstPerson;
        }
    }
}