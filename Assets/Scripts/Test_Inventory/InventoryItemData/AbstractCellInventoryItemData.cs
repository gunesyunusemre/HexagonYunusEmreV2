using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilies;

namespace Inventory
{
    public enum InventoryItemDataType
    {
        Pink,
        Blue,
        Red,
        Yellow,
        Green
    }

    public abstract class AbstractCellInventoryItemData<T> : AbstractBaseCellInventoryItemData
    where T : AbstractCellInventoryMono
    {
        [SerializeField] protected string _itemID;
        [SerializeField] protected InventoryItemDataType _inventoryItemData;
        [SerializeField] protected T _prefab;
        
        protected T InstantiateAndInitializePrefab(Transform parent)
        {
            return Instantiate(_prefab, parent);
        }
    }
}
