using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CameraWork;
using UnityEngine;
using Utilies;
using Random = UnityEngine.Random;

namespace CoreScripts
{
    public class CreateBoard : MonoBehaviour
    {
        public static CreateBoard createBoard;
    
        #region Variable

        [SerializeField] private BoardSettings _boardSettings;
        
        private byte column, row;
    

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            createBoard = this;
            column = _boardSettings.Column;
            row = _boardSettings.Row;
            _boardSettings.GroundPos = new Vector3[column];
            
            FillHexagons();
        }

        #endregion

        #region Create
        private async void FillHexagons()
        {
            _boardSettings.Cells = new Cell[column, row];
            for (byte i = 0; i < column; i++)
            {
                for (byte j = 0; j < row; j++)
                {
                    //TODO: Scriptable olarak playerı oluşturup puanı ve bomba durumlarına eriş
                    byte colorType = (byte)Random.Range(0, _boardSettings.Keys.Count);
                    string key = _boardSettings.Keys[colorType];
                    var obj = await CreateDestroyWork.Initialize(key);
                
                    Cell thisCell = obj.GetComponent<Cell>();
                    _boardSettings.Cells[i, j] = thisCell;
                    thisCell.ColorType = colorType;
                    SetProperty(thisCell, i, j);
                }
            }

            await ClearMatchForBegin();
            //Todo: Maybe basic animation
            CamWork();
        }
    
        public void SetProperty(Cell thisCell, int i, int j)
        {
            thisCell.OnChangedPosition((byte)i,(byte)j);
            thisCell.transform.parent = this.transform;
        
            thisCell.gameObject.transform.position =
                i % 2 == 0 ? new Vector3(i / 1.1f, ((j + 0.5f) / 1f)) : new Vector3(i / 1.1f, (j / 1f));
            thisCell.BombCount=Random.Range(5, 8);

            if (j==0)
            { 
                _boardSettings.GroundPos[i] = _boardSettings.Cells[i, j].transform.position;
            }
        }

        private async Task ClearMatchForBegin()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.05f));
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    MatchInfo matchInfo = BoardController.boardController.GetMatchInformation(_boardSettings.Cells[i, j]);
                    while (matchInfo.validMatch)
                    {
                        byte colorType = (byte)Random.Range(0, _boardSettings.Keys.Count);
                        CreateDestroyWork.Destroy(_boardSettings.Cells[i, j].gameObject);
                    
                        var obj = await CreateDestroyWork.Initialize(_boardSettings.Keys[colorType]);
                        Cell thisCell = obj.GetComponent<Cell>();
                        thisCell.ColorType = colorType;
                        _boardSettings.Cells[i, j] = thisCell;
                        SetProperty(thisCell, i, j);
                        matchInfo = BoardController.boardController.GetMatchInformation(_boardSettings.Cells[i, j]);
                        await Task.Delay(TimeSpan.FromSeconds(0.05f));
                    }
                }
            }
            
        }

        #endregion

        #region SetCamera

        private void CamWork()
        {
            Vector3 camTarget =  (_boardSettings.Cells[column - 1, row - 1].transform.position - 
                                  _boardSettings.Cells[0, 0].transform.position)/2;
            camTarget.y = _boardSettings.Cells[column - 1, row - 1].transform.position.y*2 / 3;
            camTarget.z = -10;
            MoveCenter.CamScript.SetCam(camTarget);
        }

        #endregion
    
    
    }
}
