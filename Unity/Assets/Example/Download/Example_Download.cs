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
            
            var info1 = new DownloadInfo("node.js", @"C:/Users/hp/Test",
                "https://nodejs.org/dist/v20.11.0/node-v20.11.0-x64.msi");
            
            downloadComponent.AddDownload(info1);
        }

        private void Update()
        {
        }
    }
}