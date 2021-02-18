using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Hexagon/Inventory/Cell Inventory Blue Item Data")]
    public class CellInventoryBlueItemData : AbstractCellInventoryItemData<CellInventoryBlueMono>
    {
        public override GameObject CreateIntoInventory(CellController targetCellController)
        {
            var instantiated = 
                InstantiateAndInitializePrefab(targetCellController.Parent);

            Debug.Log("This Class is Cell Inventory Blue Item Data");
            return instantiated.gameObject;
        }
    }
}
