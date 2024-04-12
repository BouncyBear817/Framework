using UnityEngine;

namespace Framework
{
    public abstract class UIPanelBehaviour : MgrBaseBehaviour
    {
        public override IManager Manager => UIManager_Old.Instance; 
    }
}