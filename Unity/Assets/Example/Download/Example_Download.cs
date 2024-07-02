using System.Collections;
using Framework;
using Framework.Runtime;
using UnityEngine;

namespace Example.Download
{
    public class Example_Download : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);

            var downloadComponent = MainEntryHelper.GetComponent<DownloadComponent>();
            //
            // for (int i = 1; i <= 5; i++)
            // {
            //     var info = new DownloadInfo(i.ToString(), @"C:/Users/hp/Test",
            //         "https://pic1.zhimg.com/v2-9a47134f600f2bf23c5a7177c9c4b364_r.jpg");
            //
            //     downloadComponent.AddDownload(info);
            // }

            //https://nodejs.org/dist/v20.11.0/node-v20.11.0-x64.msi

            var info1 = new DownloadInfo(@"C:/Users/hp/Test/1", "https://download.jetbrains.com.cn/rider/JetBrains.Rider-2024.1.4.exe");
            var info2 = new DownloadInfo(@"C:/Users/hp/Test/2", "https://download.jetbrains.com.cn/rider/JetBrains.Rider-2024.1.3.exe");
            var info3 = new DownloadInfo(@"C:/Users/hp/Test/3", "https://download.jetbrains.com.cn/rider/JetBrains.Rider-2024.1.2.exe");
            var info4 = new DownloadInfo(@"C:/Users/hp/Test/4", "https://download.jetbrains.com.cn/rider/JetBrains.Rider-2024.1.1.exe");

            downloadComponent.AddDownload(info1);
            downloadComponent.AddDownload(info2);
            downloadComponent.AddDownload(info3);
            downloadComponent.AddDownload(info4);
        }

        private void Update()
        {
        }
    }
}