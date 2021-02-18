using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Hexagon/Inventory/Cell Inventory Pink Item Data")]
    public class CellInventoryPinkItemData : AbstractCellInventoryItemData<CellInventoryPinkMono>
    {

        public override GameObject CreateIntoInventory(CellController targetCellController)
        {
            var instantiated = 
                InstantiateAndInitializePrefab(targetCellController.Parent);

                Debug.Log("This Class is Cell Inventory Pink Item Data");
            return instantiated.gameObject;
        }
    }
}
