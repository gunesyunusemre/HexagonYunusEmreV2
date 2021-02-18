using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CoreScripts
{
    public class TurnMechanicsSystem : MonoBehaviour
    {
        public static TurnMechanicsSystem TMS;

        #region Variable

        [SerializeField] private BoardSettings _boardSettings;
        private bool isBegin = true;
        private List<Cell> selectedHex;
        private GameObject selectedDomain;
        private Cell[,] cells;

        #region SwipeVariable
        private Vector2 startPosition;
        private Vector2 endPosition;
        //This variable working GetAngle Method
        #endregion
    
        #endregion

        #region Event
        public delegate void OnMoveCountUp();
        public static event OnMoveCountUp OnMoveCountUpEventHandler;
        #endregion

        #region MonoBehaviour

        void Start()
        {
            if (TMS==null)
            {
                TMS = this;
            }
        }
    
        void Update()
        {
            GetAngle();
        }

        #endregion

        #region TurnMechanics
        private void GetAngle()
        {
            bool isSelect, canPlay, isGameOver;
            isSelect = _boardSettings.IsSelect;
            canPlay = _boardSettings.CanPlay;
            isGameOver = _boardSettings.IsGameOver;

            /*selectedHex = BoardController.boardController.SelectedHex;
            selectedDomain = BoardController.boardController.SelectedDomain;*/
            selectedHex = _boardSettings.SelectedHex;
            selectedDomain = _boardSettings.SelectedDomain;
        
            if (!isSelect || !canPlay || isGameOver) return;

            if (selectedHex.Count==0) return;

            if(Input.GetMouseButtonDown(0))
            {
                startPosition =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPosition =Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dis = endPosition - startPosition;
                float angle = math.atan2(dis.y,dis.x) * Mathf.Rad2Deg;
            
                if (isBegin)
                {
                    isBegin = false;
                    dis = new Vector2(0, 0);
                }
            
                if (dis.magnitude > 2f)
                {
                    _boardSettings.turnCounter = 0;
                    Player._player.MoveCounter++;
                    PlayerPrefs.SetInt("moveCounter", Player._player.MoveCounter);
                    //Debug.Log(Player._player.MoveCounter);
                    if (OnMoveCountUpEventHandler != null) OnMoveCountUpEventHandler();

                    StartCoroutine(TurnDomain(angle));
                }

                //Debug.Log(angle);
            }
        }
    
    
        public IEnumerator TurnDomain(float angle)
        {
            _boardSettings.IsSelect = false;
            _boardSettings.CanPlay = false;
        
            Cell[] targets = TurnUtilies.GetTarget(angle, selectedHex, selectedDomain, _boardSettings.Column);
        
            if (angle>0)
            {
                yield return StartCoroutine(targets[0].transform.Turn(targets[1],targets[2],500, 
                    Vector3.forward, TurnUtilies.GetMidPoint(selectedHex),targets));
            }
            else
            {
                yield return StartCoroutine(targets[0].transform.Turn(targets[1],targets[2],500, 
                    Vector3.back, TurnUtilies.GetMidPoint(selectedHex),targets));
            }

            cells = _boardSettings.Cells;
            TurnUtilies.ChangeName(targets, cells);
            yield return StartCoroutine(BoardController.boardController.ControlMatch(angle));
        }
    
        public void ChangeRigidBodyStatus(bool status)
        {
            foreach (Cell g in cells)
            {
                if (g==null)
                    continue;
            
                g.gameObject.GetComponent<Rigidbody2D>().isKinematic = !status;
            }
        }

        #endregion
    
    
    
    }
}
