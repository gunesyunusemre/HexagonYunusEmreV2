using System.Collections.Generic;
using UnityEngine;

namespace CoreScripts
{
    [CreateAssetMenu(menuName = "Hexagon/Board/Cell Settings")]
    public class CellSettings : ScriptableObject
    {
        #region ID
        private byte _column;
        private byte _row;
        private byte _colorType;
        private int _bombCount;
        [SerializeField] private bool isBomb;
        #endregion

        #region Referance
        [SerializeField] private Transform _cornerParent;
        [SerializeField] private List<Transform> _corners;
        [SerializeField] private GameObject _selectedBackground;
        #endregion

        #region MyChildren
        private List<Transform> _myCorner;
        private GameObject _mySelectedBackground;
        #endregion
        
        #region SwipeVariables
        private Vector2 startPosition;
        private Vector2 endPosition;
        #endregion

        #region Capsul

        public byte Column
        {
            get => _column;
            set => _column = value;
        }
        public byte Row
        {
            get => _row;
            set => _row = value;
        }
        public byte ColorType
        {
            get => _colorType;
            set => _colorType = value;
        }
        public bool IsBomb => isBomb;
        public int BombCount
        {
            get => _bombCount;
            set => _bombCount = value;
        }
        public List<Transform> Corners => _corners;
        public List<Transform> MyCorner
        {
            get => _myCorner;
            set => _myCorner = value;
        }
        public Transform CornerParent => _cornerParent;
        public GameObject SelectedBackground => _selectedBackground;
        public GameObject MySelectedBackground
        {
            get => _mySelectedBackground;
            set => _mySelectedBackground = value;
        }


        public Vector2 StartPosition
        {
            get => startPosition;
            set => startPosition = value;
        }
        public Vector2 EndPosition
        {
            get => endPosition;
            set => endPosition = value;
        }

        #endregion
        
    }
}
