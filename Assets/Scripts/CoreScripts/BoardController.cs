using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utilies;
using Random = UnityEngine.Random;

namespace CoreScripts
{
    public class BoardController : MonoBehaviour
    {
        public static BoardController boardController;

        #region Variable

        [SerializeField] private BoardSettings _boardSettings;

        #region SwipeVariable
        private Vector2 startPosition;
        private Vector2 endPosition;
        //This variable working GetAngle Method
        #endregion


        #endregion

        #region Events
        public delegate void OnScoreUp();
        public static event OnScoreUp OnScoreUpEventHandler;
    
        public delegate void QuickSave();
        public static event QuickSave QuickSaveEventHandler;
        #endregion

        #region MonoBehaviour Function

        private void OnEnable()
        {
            if (boardController==null)
            {
                boardController = this;
            }
            Cell.GameOverEventHandler += GameOver;
        }

        private void OnDisable()
        {
            Cell.GameOverEventHandler -= GameOver;
        }

        #endregion

        #region Start Mechanics

        /*private void FillResumeHexagons()
        {
            cells = new Cell[_boardSettings.Column, _boardSettings.Row];
            for (byte i = 0; i < _boardSettings.Column; i++)
            {
                for (byte j = 0; j < _boardSettings.Row; j++)
                {
                    string location = i + "," + j;
                    cells[i,j]=InstantiateResumeCell(i, j, PlayerPrefs.GetInt(location));
                    if (j==0)
                    {
                        _boardSettings.GroundPos[i] = cells[i, j].transform.position;
                    }
                }
            }
        }*/

        #endregion

        #region EndMechanics

        private void GameOver()
        {
            _boardSettings.IsGameOver = true;
            _boardSettings.CanPlay = false;
            StartCoroutine(BreakBoard());
        }

        private IEnumerator BreakBoard()
        {
            yield return new WaitForSeconds(2f);
            MatchUtilies.UnSelectHex(_boardSettings.SelectedHex);
            TurnMechanicsSystem.TMS.ChangeRigidBodyStatus(true);
        }
    
        private void SaveBoard()
        {
            if (_boardSettings.IsGameOver)
                return;

            for (int i = 0; i < _boardSettings.Column; i++)
            {
                for (int j = 0; j < _boardSettings.Row; j++)
                {
                    string location = i + "," + j;
                    PlayerPrefs.SetInt(location, _boardSettings.Cells[i, j].ColorType);
                    if (_boardSettings.Cells[i, j].IsBomb)
                    {
                        PlayerPrefs.SetInt(i+","+j+"isBomb", 1);
                        PlayerPrefs.SetInt(i+","+j+"bombCount", _boardSettings.Cells[i, j].BombCount);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(i+","+j+"isBomb", 0);
                    }
                
                }
            }
            PlayerPrefs.Save();
        }

        #endregion
    
        #region CreatedDestroyMechanics

        private async void NewItemInit(byte i, byte j)
        {
            byte colorType = (byte)Random.Range(0, _boardSettings.Keys.Count);
        
            var obj = await CreateDestroyWork.Initialize(_boardSettings.Keys[colorType]);
            Cell thisCell = obj.GetComponent<Cell>();

            _boardSettings.Cells[i, j] = thisCell;
        
            thisCell.ColorType = (byte) colorType;
            thisCell.BombCount=Random.Range(5, 8);
            CreateBoard.createBoard.SetProperty(thisCell, i, j);
        }
    
    
        /*private Cell InstantiateResumeCell(int i, int j, int colorType)
        {
            Cell thisCell;
        
            if (PlayerPrefs.GetInt(i+","+j+"isBomb")==1)
            {
                thisCell = Instantiate(bombs[colorType],
                    i % 2 == 0 ? new Vector3(i / 1.1f, (j + 0.5f) / 1f) : new Vector3(i / 1.1f, j / 1f),
                    quaternion.identity).GetComponent<Cell>();
            
                thisCell.BombCount=PlayerPrefs.GetInt(i+","+j+"bombCount");

            }
            else
            {
            
                thisCell = Instantiate(hexagons[colorType],
                    i % 2 == 0 ? new Vector3(i / 1.1f, ((j + 0.5f) / 1f)) : new Vector3(i / 1.1f, (j / 1f)),
                    quaternion.identity).GetComponent<Cell>();
            }
            thisCell.OnChangedPosition((byte)i,(byte)j);
            thisCell.transform.parent = this.transform;
            thisCell.ColorType = (byte) colorType;
        
            return thisCell;
        }*/

        IEnumerator DestroyItems(List<Cell> items)
        {
            foreach (Cell i in items)
            {
                //Debug.Log(i.name+": "+i.ColorType);
                yield return StartCoroutine(i.transform.Scale(Vector3.zero, 0.095f));
            }

            foreach (var i in items)
            {
                CreateDestroyWork.Destroy(i.gameObject);
                yield return new WaitForSeconds(0.05f);
            } //Destroy(i.gameObject); }
        }
        #endregion

        #region MatchWork
    
        public IEnumerator ControlMatch(float angle)
        {
            //Debug.Log("Match control");
            MatchInfo matchA = GetMatchInformation(_boardSettings.SelectedHex[0]);
            MatchInfo matchB = GetMatchInformation(_boardSettings.SelectedHex[1]);
            MatchInfo matchC = GetMatchInformation(_boardSettings.SelectedHex[2]);
            //Debug.Log(matchA.validMatch +":"+ matchB.validMatch +":"+matchC.validMatch);
            if (!matchA.validMatch && !matchB.validMatch && !matchC.validMatch)
            {
                if (_boardSettings.turnCounter<2)
                {
                    _boardSettings.turnCounter++;
                    _boardSettings.CanPlay = false;
                    yield return new WaitForSeconds(0.3f);
                    yield return StartCoroutine(TurnMechanicsSystem.TMS.TurnDomain(angle));
                }
                else {_boardSettings.CanPlay = true; _boardSettings.IsSelect = true; }
            
            }
            if (matchA.validMatch)
            {
                //point gain
                Player._player.Score += matchA.match.Count * 5;
                if (OnScoreUpEventHandler != null) OnScoreUpEventHandler();
                //Debug.Log(Player._player.Score);
            
                MatchUtilies.UnSelectHex(_boardSettings.SelectedHex);
                yield return StartCoroutine(DestroyItems(matchA.match));
                yield return new WaitForSeconds(_boardSettings.delayBetweenMatches);
                yield return StartCoroutine(UpdateCellsAfterMatch(matchA));
            }
            else if (matchB.validMatch)
            {
                //point gain
                Player._player.Score += matchB.match.Count * 5;
                if (OnScoreUpEventHandler != null) OnScoreUpEventHandler();
                //Debug.Log(Player._player.Score);
            
                MatchUtilies.UnSelectHex(_boardSettings.SelectedHex);
                yield return StartCoroutine(DestroyItems(matchB.match));
                yield return new WaitForSeconds(_boardSettings.delayBetweenMatches);
                yield return StartCoroutine(UpdateCellsAfterMatch(matchB));
            }
            else if (matchC.validMatch)
            {
                //point gain
                Player._player.Score += matchC.match.Count * 5;
                if (OnScoreUpEventHandler != null) OnScoreUpEventHandler();
                //Debug.Log(Player._player.Score);
            
                MatchUtilies.UnSelectHex(_boardSettings.SelectedHex);
                yield return StartCoroutine(DestroyItems(matchC.match));
                yield return new WaitForSeconds(_boardSettings.delayBetweenMatches);
                yield return StartCoroutine(UpdateCellsAfterMatch(matchC));
            }
            yield return new WaitForSeconds(_boardSettings.delayBetweenMatches);
        }
    
        public MatchInfo GetMatchInformation(Cell item)
        {
            MatchInfo m = new MatchInfo {match = null};
            List<Cell> upMatch = MatchUtilies.SearchUp(item, _boardSettings.Cells, _boardSettings.Column, _boardSettings.Row);
            List<Cell> downMatch = MatchUtilies.SearchDown(item, _boardSettings.Cells, _boardSettings.Column, _boardSettings.Row);
            List<Cell> leftRightMatch = MatchUtilies.SearchLeftRight(item, _boardSettings.Cells, _boardSettings.Column, _boardSettings.Row);
            //left
            //right
        
            if (upMatch.Count >= 3 && upMatch.Count > downMatch.Count)
            {
                //Debug.Log("Up!");
                //Up match information

                m.matchX1=MinMaxWork.GetMinimumX(upMatch);
                m.matchStartingY1 = MinMaxWork.GetMatchedMinimumY(upMatch, m.matchX1);
                m.matchEndingY1 = MinMaxWork.GetMatchedMaximumY(upMatch, m.matchX1);
                m.matchX2=MinMaxWork.GetMaximumX(upMatch);
                m.matchStartingY2 = MinMaxWork.GetMatchedMinimumY(upMatch, m.matchX2);
                m.matchEndingY2 = MinMaxWork.GetMatchedMaximumY(upMatch, m.matchX2);
            
                m.match = upMatch;
            }else if (downMatch.Count >= 3 && downMatch.Count >leftRightMatch.Count )
            {
                //Debug.Log("Down!");
                //Down match information

                m.matchX1=MinMaxWork.GetMinimumX(downMatch);
                m.matchStartingY1 = MinMaxWork.GetMatchedMinimumY(downMatch, m.matchX1);
                m.matchEndingY1 = MinMaxWork.GetMatchedMaximumY(downMatch, m.matchX1);
                m.matchX2=MinMaxWork.GetMaximumX(downMatch);
                m.matchStartingY2 = MinMaxWork.GetMatchedMinimumY(downMatch, m.matchX2);
                m.matchEndingY2 = MinMaxWork.GetMatchedMaximumY(downMatch, m.matchX2);
            
                m.match = downMatch;
            }else if (leftRightMatch.Count>=3)
            {
                //Debug.Log("Left or Right!");
                //Left Or Right match information

                m.matchX1=MinMaxWork.GetMinimumX(leftRightMatch);
                m.matchStartingY1 = MinMaxWork.GetMatchedMinimumY(leftRightMatch, m.matchX1);
                m.matchEndingY1 = MinMaxWork.GetMatchedMaximumY(leftRightMatch, m.matchX1);
                m.matchX2=MinMaxWork.GetMaximumX(leftRightMatch);
                m.matchStartingY2 = MinMaxWork.GetMatchedMinimumY(leftRightMatch, m.matchX2);
                m.matchEndingY2 = MinMaxWork.GetMatchedMaximumY(leftRightMatch, m.matchX2);
            
                m.match = leftRightMatch;
            }
            return m;
        }
    
        private IEnumerator ControlMatchAllBoard()
        {
            for (int x = 0; x < _boardSettings.Column; x++)
            {
                for (int y = 0; y < _boardSettings.Row; y++)
                {
                    MatchInfo matchInfo = GetMatchInformation(_boardSettings.Cells[x, y]);
                    if (matchInfo.validMatch)
                    {
                        //point gain

                        Player._player.Score += matchInfo.match.Count * 5;
                        if (OnScoreUpEventHandler != null) OnScoreUpEventHandler();
                        //Debug.Log(Player._player.Score);

                        yield return StartCoroutine(DestroyItems(matchInfo.match));
                        yield return new WaitForSeconds(_boardSettings.delayBetweenMatches);
                        yield return StartCoroutine(UpdateCellsAfterMatch(matchInfo));
                    }
                }
            }
        }
    
        #endregion

        #region Create New Hexagons

        IEnumerator UpdateCellsAfterMatch(MatchInfo match)
        {
            int[] matchX = new int[2];
            matchX[0] = match.matchX1;
            matchX[1] = match.matchX2;
        
            int[] matchStartY = new int[2];
            matchStartY[0] = match.matchStartingY1;
            matchStartY[1] = match.matchStartingY2;
        
            int[] matchEndY = new int[2];
            matchEndY[0] = match.matchEndingY1;
            matchEndY[1] = match.matchEndingY2;
        
            //Debug.Log(match.matchX1 + " - " +match.matchStartingY1 +"/"+match.matchEndingY1);
            //Debug.Log(match.matchX2 + " - " +match.matchStartingY2 +"/"+match.matchEndingY2);
            int matchHeight;
            for (int i = 0; i < matchX.Length; i++)
            {
                matchHeight = (matchEndY[i] - matchStartY[i]) + 1;
                //Debug.Log((matchStartY[i]+matchHeight)+"-"+row);
            
                for (int j = matchStartY[i]+matchHeight; j < _boardSettings.Row; j++)
                {
                    _boardSettings.Cells[matchX[i], j-matchHeight] = _boardSettings.Cells[matchX[i], j];
                    _boardSettings.Cells[matchX[i],j-matchHeight].OnChangedPosition((byte)matchX[i],(byte)(j-matchHeight));
                    _boardSettings.Cells[matchX[i],j-matchHeight].ChangeCollider(false);
                    if (j-matchHeight!=0)
                    {
                        _boardSettings.Cells[matchX[i],j-matchHeight].ChangeBodyType(RigidbodyType2D.Dynamic);
                    }
                    else
                    {
                        StartCoroutine(_boardSettings.Cells[matchX[i], j - matchHeight].transform.Move(_boardSettings.GroundPos[matchX[i]], 0.5f));
                    }
                
                }
                yield return new WaitForSeconds(0.5f);
                for (int j = 0; j <matchHeight; j++)
                {
                    //cells[matchX[i], (row-1) - j] = InstantiateCell(matchX[i], (row-1) - j);
                    NewItemInit((byte)matchX[i], (byte)((_boardSettings.Row-1) - j));
                    MatchInfo matchInfo = GetMatchInformation(_boardSettings.Cells[matchX[i], (_boardSettings.Row-1) - j]);
                    if (matchInfo.validMatch)
                    {
                        Destroy(_boardSettings.Cells[matchX[i], (_boardSettings.Row-1) - j].gameObject);
                        //cells[matchX[i], (row-1) - j] = InstantiateCell(matchX[i], (row-1) - j);
                        NewItemInit((byte)matchX[i], (byte)((_boardSettings.Row-1) - j));
                    }
                    _boardSettings.Cells[matchX[i], (_boardSettings.Row-1) - j].ChangeCollider(false);
                }
            }

            yield return new WaitForSeconds(0.3f);
        
            for (int i = 0; i < _boardSettings.Column; i++)
            {
                for (int j = 0; j < _boardSettings.Row; j++)
                {
                    _boardSettings.Cells[i,j].ChangeCollider(true);
                    _boardSettings.Cells[i,j].ChangeBodyType(RigidbodyType2D.Static);
                }
            }
        
            yield return StartCoroutine(ControlMatchAllBoard());
            _boardSettings.CanPlay = true;
            _boardSettings.IsSelect = true;
        
            //**Save**********
            if (QuickSaveEventHandler != null) QuickSaveEventHandler();
            SaveBoard();
            //****************
        }

        #endregion
    
    }
}
