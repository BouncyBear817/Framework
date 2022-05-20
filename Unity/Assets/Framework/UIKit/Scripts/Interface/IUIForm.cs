
namespace Framework.UIKit
{
    public interface IUIForm
    {
        int UIFormId
        {
            get;
        }

        string UIFormAssetName
        {
            get;
        }

        void OnInit(int uiFormId, string uiFormAssetName, object data);

        void OnOpen(object data);

        void OnUpdate();

        void OnClose();

        void OnRecycle();
    }
}

