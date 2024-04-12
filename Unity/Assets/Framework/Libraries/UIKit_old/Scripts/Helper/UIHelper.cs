using UnityEngine;

namespace Framework
{
    public static class UIHelper
    {
        public static void SetActive(this Transform transform, bool value)
        {
            if (transform.gameObject != null) 
                transform.gameObject.SetActive(value);
        }
    }
}