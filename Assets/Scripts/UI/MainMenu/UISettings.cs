using System.Collections.Generic;
using UnityEngine;

namespace UI.MainMenu
{
    [CreateAssetMenu(menuName = "Hexagon/UI/Main Menu Swipe Settings")]
    public class UISettings : ScriptableObject
    {
        [SerializeField] private float swipeTolarance = 100;
        [SerializeField] private float duration=1;
        private float _startPos;
        private float _endPos;
        [SerializeField] private Vector3[] buttonsPos;
        
        private byte _selectedCounter = 0;
        private bool _canPlay = true;
        
        #region Capsul

        public float StartPos
        {
            get => _startPos;
            set => _startPos = value;
        }
        public float EndPos
        {
            get => _endPos;
            set => _endPos = value;
        }
        public float SwipeTolarance => swipeTolarance;
        public float Duration => duration;

        public Vector3[] ButtonsPos
        {
            get => buttonsPos;
            set => buttonsPos = value;
        }

        public byte SelectedCounter
        {
            get => _selectedCounter;
            set => _selectedCounter = value;
        }

        public bool CanPlay
        {
            get => _canPlay;
            set => _canPlay = value;
        }

        #endregion

        #region Send Direction
        public void CalculateDirection()
        {
            if (!_canPlay)
                return;

            float dis = _endPos - _startPos;
            if (dis>swipeTolarance)
            {
                if (_selectedCounter == 5) { _selectedCounter = 0; }
                else { _selectedCounter++; }

                UITurnButtonsEventHandler?.Invoke(1);
            }
            else if (dis<-swipeTolarance)
            {
                if (_selectedCounter == 0) { _selectedCounter = 5; }
                else { _selectedCounter--; }
                
                UITurnButtonsEventHandler?.Invoke(-1);
            }
            else
            {
                //Debug.Log("Select"+_selectedCounter);
                UISelectEventHandler?.Invoke(_selectedCounter);
            }
        }
        public delegate void UISelectCase(int counter);
        public static event UISelectCase UISelectEventHandler;
        
        public delegate void UITurnButtons(int dir);
        public static event UITurnButtons UITurnButtonsEventHandler;
        #endregion
        
        
        
    }
}
