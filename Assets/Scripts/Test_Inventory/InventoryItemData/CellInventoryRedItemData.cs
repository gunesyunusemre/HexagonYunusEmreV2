using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Hexagon/Inventory/Cell Inventory Red Item Data")]
    public class CellInventoryRedItemData : AbstractCellInventoryItemData<CellInventoryRedMono>
    {
        public override GameObject CreateIntoInventory(CellController targetCellController)
        {
            var instantiated = 
                InstantiateAndInitializePrefab(targetCellController.Parent);

            Debug.Log("This Class is Cell Inventory Red Item Data");
            return instantiated.gameObject;
        }
    }
}
