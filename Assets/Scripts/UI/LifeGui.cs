using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EvilCubes.Core;

namespace EvilCubes.UI
{
    public class LifeGui : MonoBehaviour
    {
        [SerializeField]
        LifeComponent mLifeComponentToFollow;
        [SerializeField]
        GameObject mLifeUI;


        [SerializeField]
        Color mGood = Color.green;
        [SerializeField]
        Color mMiddle = Color.yellow;
        [SerializeField]
        Color mBad = Color.red;

        float maxLife;
        float previousLife;
        RectTransform mLifeTransform;
        Image mLifeImage;

        /////////////////////////////////////////////
        private void Awake()
        {
            if (mLifeComponentToFollow == null || mLifeUI == null)
            {
                Debug.LogWarning("LifeGui: Some component not found.");
                enabled = false;
                return;
            }
        }

        /////////////////////////////////////////////
        void Start()
        {
            maxLife = mLifeComponentToFollow.GetMaxLife();
            previousLife = maxLife;
            mLifeTransform = mLifeUI.GetComponent<RectTransform>();
            mLifeImage = mLifeUI.GetComponent<Image>();

            if (mLifeTransform == null || mLifeImage == null)
            {
                Debug.LogWarning("LifeGui: Some component initialization failed.");
                enabled = false;
                return;
            }
        }

        /////////////////////////////////////////////
        void Update()
        {
            float currentLife = mLifeComponentToFollow.Life;
            if (!Mathf.Approximately(previousLife, currentLife))
            {
                previousLife = currentLife;
                float normalizedLife = currentLife / maxLife;
                if (normalizedLife > 0.5f)
                    mLifeImage.color = mGood;
                else if (normalizedLife > 0.2)
                    mLifeImage.color = mMiddle;
                else
                    mLifeImage.color = mBad;
                if (normalizedLife < 0)
                    normalizedLife = 0;
                mLifeTransform.localScale = new Vector3(normalizedLife, mLifeTransform.localScale.y, mLifeTransform.localScale.z);
            }
        }
    }
}