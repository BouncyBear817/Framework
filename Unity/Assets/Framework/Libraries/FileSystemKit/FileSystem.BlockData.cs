// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/6 11:13:59
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    public sealed partial class FileSystem : IFileSystem
    {
        /// <summary>
        /// 块数据
        /// </summary>
        private struct BlockData
        {
            public static readonly BlockData Empty = new BlockData(0, 0);

            private readonly int mStringIndex;
            private readonly int mClusterIndex;
            private readonly int mLength;

            public BlockData(int clusterIndex, int length) : this(-1, clusterIndex, length)
            {
            }

            public BlockData(int stringIndex, int clusterIndex, int length)
            {
                mStringIndex = stringIndex;
                mClusterIndex = clusterIndex;
                mLength = length;
            }

            public bool Using => mStringIndex >= 0;

            public int StringIndex => mStringIndex;

            public int ClusterIndex => mClusterIndex;

            public int Length => mLength;

            public BlockData Free()
            {
                return new BlockData(mClusterIndex, (int)GetUpBoundClusterOffset(mLength));
            }
        }
    }
}