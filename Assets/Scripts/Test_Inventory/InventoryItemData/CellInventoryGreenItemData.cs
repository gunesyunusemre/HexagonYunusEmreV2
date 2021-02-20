using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Hexagon/Inventory/Cell Inventory Green Item Data")]
    public class CellInventoryGreenItemData : AbstractCellInventoryItemData<CellInventoryGreenMono>
    {
        public override GameObject CreateIntoInventory(CellController targetCellController)
        {
            var instantiated = 
                InstantiateAndInitializePrefab(targetCellController.Parent);
            
            Debug.Log("This Class is Cell Inventory Green Item Data");
            return instantiated.gameObject;
        }
    }
}
