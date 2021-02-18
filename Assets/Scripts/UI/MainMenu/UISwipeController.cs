using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class UISwipeController : MonoBehaviour
    {

        [SerializeField] private UISettings settings;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                settings.StartPos = Input.mousePosition.y;
            }

            if (Input.GetMouseButtonUp(0))
            {
                settings.EndPos = Input.mousePosition.y;
                settings.CalculateDirection();
            }
        }

    }
}
