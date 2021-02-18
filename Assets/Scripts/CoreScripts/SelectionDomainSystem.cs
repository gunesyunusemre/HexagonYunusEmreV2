using System.Collections.Generic;
using UnityEngine;

namespace CoreScripts
{
    public class SelectionDomainSystem : MonoBehaviour
    {

        #region Variable

        [SerializeField] private BoardSettings _boardSettings;
        private GameObject selectedDomain;
        private List<Cell> selectedHex;
        private float domainRadius;

        #endregion

        #region MonoBehaviour
        void Start()
        {
            /*selectedDomain = BoardController.boardController.SelectedDomain;
            selectedHex = BoardController.boardController.SelectedHex;
            domainRadius = BoardController.boardController.DomainRadius;*/
            selectedDomain = _boardSettings.SelectedDomain;
            selectedHex = _boardSettings.SelectedHex;
            domainRadius = _boardSettings.DomainRadius;
        }

        private void OnEnable()
        {
            Cell.OnMouseOverItemEventHandler += OnMouseOverItem;
        }

        private void OnDisable()
        {
            Cell.OnMouseOverItemEventHandler -= OnMouseOverItem;
        }
        #endregion

        #region SelectDomain
        private void OnMouseOverItem(Cell item)
        {
            bool canPlay= _boardSettings.CanPlay;
            bool isGameOver=_boardSettings.IsGameOver;

            if (!canPlay || isGameOver)
                return;

            //Debug.Log("("+item.Column+","+item.Row+")" +" Color: "+item.ColorType);
        
            SelectDomain(item);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(selectedDomain.transform.position, domainRadius);

            if (colliders.Length>3) return;

            foreach (var selected in selectedHex)
            {
                if (selected==null)
                    break;
            
                selected.SelectedBackground.SetActive(false);
            }
            selectedHex.Clear();

            for (int i = 0; i <3 ; i++)
            {
                selectedHex.Add(colliders[i].GetComponent<Cell>());
                selectedHex[i].SelectedBackground.SetActive(true);
                //Debug.Log(colliders[i].name);
            }
        
            Equalize();
        }

        private void SelectDomain(Cell item)
        {
            //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dis = 999f;
            foreach (var corner in item.Corners)
            {
                if (!(Vector2.Distance(mousePos, corner.position) < dis)) continue;
            
                Collider2D[] colliders = Physics2D.OverlapCircleAll(corner.position, domainRadius);
            
                if (colliders.Length < 3 ) continue;
            
                dis = Vector2.Distance(mousePos, corner.position);
                selectedDomain = corner.gameObject;
                _boardSettings.IsSelect = true;
            }

            //Debug.Log(selectedDomain.name);
        }

        private void Equalize()
        {
            /*BoardController.boardController.SelectedDomain = selectedDomain;
            BoardController.boardController.SelectedHex = selectedHex;*/
            _boardSettings.SelectedDomain = selectedDomain;
            _boardSettings.SelectedHex = selectedHex;
        }
    
        #endregion
    
    }
}
