using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Hexagon/Inventory/Cell Inventory Yellow Item Data")]
    public class CellInventoryYellowItemData : AbstractCellInventoryItemData<CellInventoryYellowMono>
    {
        public override GameObject CreateIntoInventory(CellController targetCellController)
        {
            var instantiated = 
                InstantiateAndInitializePrefab(targetCellController.Parent);

            Debug.Log("This Class is Cell Inventory Yellow Item Data");
            return instantiated.gameObject;
        }
    }
}
