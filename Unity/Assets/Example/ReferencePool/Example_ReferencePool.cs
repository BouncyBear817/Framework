using System;
using Framework;
using UnityEngine;

namespace Example.ReferencePool
{
    public class Example_ReferencePool : MonoBehaviour
    {
        private void Start()
        {
            Framework.ReferencePool.Add<Example_Reference>(5);
            var exampleReference = Framework.ReferencePool.Acquire<Example_Reference>();
            exampleReference.Show();

            var referenceInfos = Framework.ReferencePool.GetAllReferencePoolInfos();
            foreach (var info in referenceInfos)
            {
                Debug.Log($"Type ({info.Type.FullName}) has ({info.UnusedReferenceCount}) unusedReferenceCount, ({info.UsingReferenceCount}) usingReferenceCount.");
            }
            
            Framework.ReferencePool.Release(exampleReference);
            referenceInfos = Framework.ReferencePool.GetAllReferencePoolInfos();
            foreach (var info in referenceInfos)
            {
                Debug.Log($"Type ({info.Type.FullName}) has ({info.UnusedReferenceCount}) unusedReferenceCount, ({info.UsingReferenceCount}) usingReferenceCount.");
            }
        }
    }

    public class Example_Reference : IReference
    {
        public void Show()
        {
            Debug.Log("example show.");
        }
        public void Clear()
        {
            Debug.Log("example is released.");
        }
    }
}