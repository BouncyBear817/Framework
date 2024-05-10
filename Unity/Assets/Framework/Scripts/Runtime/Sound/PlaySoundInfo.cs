// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/10 16:14:32
//  * Description:
//  * Modify Record:
//  *************************************************************/

using Framework;
using UnityEngine;

namespace Runtime
{
    public sealed class PlaySoundInfo : IReference
    {
        private GameObject mBindingObject;
        private Vector3 mWorldPosition;
        private object mUserData;

        public PlaySoundInfo()
        {
            mBindingObject = null;
            mWorldPosition = Vector3.zero;
            mUserData = null;
        }

        public GameObject BindingObject => mBindingObject;

        public Vector3 WorldPosition => mWorldPosition;

        public object UserData => mUserData;

        public static PlaySoundInfo Create(GameObject bindingObject, Vector3 worldPosition, object userData)
        {
            var playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
            playSoundInfo.mBindingObject = bindingObject;
            playSoundInfo.mWorldPosition = worldPosition;
            playSoundInfo.mUserData = userData;
            return playSoundInfo;
        }

        public void Clear()
        {
            mBindingObject = null;
            mWorldPosition = Vector3.zero;
            mUserData = null;
        }
    }
}