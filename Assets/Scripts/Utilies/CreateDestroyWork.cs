using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utilies
{
    public static class CreateDestroyWork
    {

        public static async Task<GameObject> Initialize(string key)
        {
            //Debug.Log("Waiting");
            var obj = await Addressables.InstantiateAsync(key).Task;
            //Debug.Log("Waiting complete");
            return obj;
        }

        public static void Destroy(GameObject obj)
        {
            //Debug.Log("Waiting Destroy");
            Addressables.Release(obj);
            //Debug.Log("Destroy Complate");
        }


    }
}
