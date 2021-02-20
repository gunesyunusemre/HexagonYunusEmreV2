using System.Collections.Generic;
using CoreScripts;
using UnityEngine;

namespace Inventory
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private AbstractBaseCellInventoryItemData[] _inventoryItemDataArray;
        private List<AbstractBaseCellInventoryItemData> _instantiatedItemDataList;
        
        [SerializeField] private BoardSettings _boardSettings;
        public Transform Parent;

        private async void Start()
        {
            InitializeInventory(_inventoryItemDataArray);
        }
        
        private void OnDestroy()
        {
            ClearInventory();
        }


        private void InitializeInventory(AbstractBaseCellInventoryItemData[] inventoryDataArray)
        {
            ClearInventory();
            _instantiatedItemDataList = new List<AbstractBaseCellInventoryItemData>(_inventoryItemDataArray.Length);

            for (int i = 0; i < _boardSettings.Column; i++)
            {
                for (int j = 0; j < _boardSettings.Row; j++)
                {
                    byte colorType = (byte)Random.Range(0, _inventoryItemDataArray.Length);
                    var instantiated = Instantiate(_inventoryItemDataArray[colorType]);
                    var thisObj=instantiated.CreateIntoInventory(this);
                    _instantiatedItemDataList.Add(instantiated);
                
                    thisObj.transform.position =
                        i % 2 == 0 ? new Vector3(i / 1.1f, ((j + 0.5f) / 1f)) : new Vector3(i / 1.1f, (j / 1f));
                    
                }
            }
            
            
        }
        
        private void ClearInventory()
        {
            if (_instantiatedItemDataList!=null)
            {
                for (int i = 0; i < _instantiatedItemDataList.Count; i++)
                {
                    _instantiatedItemDataList[i].Destroy();
                }
            }
        }



    }
}
