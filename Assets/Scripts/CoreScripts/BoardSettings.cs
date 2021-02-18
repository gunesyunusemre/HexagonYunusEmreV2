using System.Collections.Generic;
using UnityEngine;

namespace CoreScripts
{
    [CreateAssetMenu(menuName = "Hexagon/Board/Board Settings")]
    public class BoardSettings : ScriptableObject
    {
        #region Prepare Game
        [Header("Prepare Game")]
        [SerializeField] private byte column = 8;
        [SerializeField] private byte row = 9;
        [SerializeField] private int bombScore = 1000;
        [SerializeField] private List<string> keys;
        #endregion
        
        #region MidGame
        [Header("Mid Game")]
        [Tooltip("this variable is required for the wait time between matches.")]
        public float delayBetweenMatches = 1f;
        [Tooltip("This variable required for select domain")]
        [SerializeField] [Range(0,1)]private float domainRadius=0;
        #endregion
        
        #region Core Variable
        private List<Cell> selectedHex = new List<Cell>();
        private Cell[,] cells;
        private GameObject selectedDomain;
        #endregion
        
        #region Booleans
        private bool isSelect=false;
        private bool canPlay=true;
        private bool isGameOver=false;

        public bool IsSelect
        {
            get => isSelect;
            set => isSelect = value;
        }
        public bool CanPlay
        {
            get => canPlay;
            set => canPlay = value;
        }
        public bool IsGameOver
        {
            get => isGameOver;
            set => isGameOver = value;
        }
        #endregion
        
        #region Others
        //Capsule Method************
        public byte Column { get { return column; } }
        public byte Row { get { return row; } }
    
        public Cell[,] Cells
        {
            get => cells;
            set => cells=value;
        }
    
        public GameObject SelectedDomain
        {
            get => selectedDomain;
            set => selectedDomain= value;
        }
        public List<Cell> SelectedHex
        {
            get => selectedHex;
            set => selectedHex = value;
        }
        public List<string> Keys
        {
            get => keys;
            set => keys = value;
        }
        public float DomainRadius{ get => domainRadius;}
        //**************************
    
        public byte turnCounter = 0;//This variable for Turn count-1  

        //*****Ground Vectors*************
        private Vector3[] groundsPos;

        public Vector3[] GroundPos
        {
            get => groundsPos;
            set => groundsPos = value;
        }
        //*******************************
    

        #endregion
    }
}
