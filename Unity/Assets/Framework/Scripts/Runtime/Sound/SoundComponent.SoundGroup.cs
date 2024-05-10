/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/10 14:52:52
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using UnityEngine;

namespace Runtime
{
    public sealed partial class SoundComponent : FrameworkComponent
    {
        [Serializable]
        private sealed class SoundGroup
        {
            [SerializeField] private string mName = null;
            [SerializeField] private bool mAvoidBeingReplacedBySamePriority = false;
            [SerializeField] private bool mMute = false;
            [SerializeField, Range(0f, 1f)] private float mVolume = 1f;
            [SerializeField] private int mAgentHelperCount = 1;

            public string Name => mName;

            public bool AvoidBeingReplacedBySamePriority => mAvoidBeingReplacedBySamePriority;

            public bool Mute => mMute;

            public float Volume => mVolume;

            public int AgentHelperCount => mAgentHelperCount;
        }
    }
}