using UnityEngine;

namespace TaranaGame.Tarakan
{
    public abstract class TarakanViewBase : MonoBehaviour
    {
        public void LookAt(Vector3 position)
        {
            var dir = position - transform.position;
            dir.Normalize();  
            var rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }
    }
}