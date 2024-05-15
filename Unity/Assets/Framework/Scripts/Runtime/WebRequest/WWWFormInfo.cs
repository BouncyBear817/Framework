/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 16:31:10
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    public sealed class WWWFormInfo : IReference
    {
        private WWWForm mWWWForm;
        private object mUserData;

        public WWWFormInfo()
        {
            mWWWForm = null;
            mUserData = null;
        }

        public WWWForm WWWForm => mWWWForm;

        public object UserData => mUserData;

        public static WWWFormInfo Create(WWWForm wwwForm, object userData)
        {
            var info = ReferencePool.Acquire<WWWFormInfo>();
            info.mWWWForm = wwwForm;
            info.mUserData = userData;
            return info;
        }

        public void Clear()
        {
            mWWWForm = null;
            mUserData = null;
        }
    }
}