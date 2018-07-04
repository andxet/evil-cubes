using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game namespaces
using EvilCubes.Core;

namespace EvilCubes.Appearance
{
    ///// <summary>
    /// Change the color of the object based on the life
    //// </summary>
    [RequireComponent(typeof(LifeComponent))]
    [RequireComponent(typeof(Material))]
    public class ColorLifeIndicator : MonoBehaviour
    {
        public Color finalColor = Color.white;

        LifeComponent mLifeComponent;
        Material mMaterial;
        Color mInitialColor;
        int mMaxLife;
        int mCurrentLife;

        /////////////////////////////////////////////
        void Start()
        {
            mLifeComponent = GetComponent<LifeComponent>();
            mMaterial = GetComponent<Renderer>().material;
            mInitialColor = mMaterial.color;
            mMaxLife = mLifeComponent.GetMaxLife();
            mCurrentLife = mLifeComponent.Life;
            UpdateColor();
        }

        /////////////////////////////////////////////
        void Update()
        {
            if (mCurrentLife != mLifeComponent.Life)
                UpdateColor();
        }

        /////////////////////////////////////////////
        void UpdateColor()
        {
            mCurrentLife = mLifeComponent.Life;
            float lifePercentage = (float) mCurrentLife / mMaxLife; 
            mMaterial.SetColor("_Color", Color.Lerp(finalColor, mInitialColor, lifePercentage));

        }
    }
}
