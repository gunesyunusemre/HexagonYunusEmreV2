using UnityEngine;

namespace Inventory
{
    public abstract class AbstractBaseCellInventoryItemData : ScriptableObject
    {
        protected GameObject gameObject;
        public abstract GameObject CreateIntoInventory(CellController targetCellController);

        public virtual void Destroy()
        {
            Destroy(this);
        }
    }
}
