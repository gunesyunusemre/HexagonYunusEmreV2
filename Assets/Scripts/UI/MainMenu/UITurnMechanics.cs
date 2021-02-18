using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace UI.MainMenu
{
    public class UITurnMechanics : MonoBehaviour
    {
        //TODO: Mid point kullanmazsan sil.
        [SerializeField] private Transform _midPoint;
        [SerializeField] private Transform[] buttons;
        [SerializeField] private UISettings settings;

        #region Monobehaviour
        private void OnEnable()
        {
            UISettings.UITurnButtonsEventHandler += MakeTurn;
        }

        private void OnDisable()
        {
            UISettings.UITurnButtonsEventHandler -= MakeTurn;
        }
        #endregion
        

        private void MakeTurn(int dir)
        {
            StartCoroutine(Turn(dir));
        }

        IEnumerator Turn(int dir)
        {
            settings.CanPlay = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                settings.ButtonsPos[i] = buttons[i].position;
            }
            
            float counter = 0;
            while(counter < settings.Duration)
            {
                foreach (var button in buttons)
                {
                    button.RotateAround(_midPoint.position, new Vector3(0,0,dir), 
                        (60/settings.Duration)*Time.deltaTime);
                    
                    button.rotation=quaternion.identity;
                }
                counter += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i==5 && dir < 0)
                {
                    buttons[i].position=settings.ButtonsPos[0];
                }
                else if (i==0 && dir > 0)
                {
                    buttons[i].position=settings.ButtonsPos[5];
                }
                else
                {
                    buttons[i].position=settings.ButtonsPos[i-dir];
                }
            }

            settings.CanPlay = true;
        }
    }
}
