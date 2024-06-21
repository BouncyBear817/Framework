// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/13 17:16:10
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections;
using Framework;
using UnityEngine;

namespace Example.WebRequest
{
    public class Example_WebRequest : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);

            MainEntry.Event.Subscribe(Framework.Runtime.WebRequestSuccessEventArgs.EventId, Handler);

            var webRequestInfo = WebRequestInfo.Create("https://car-web-api.autohome.com.cn/car/series/seires_city", null, 0, null, null);
            var wwwForm = new WWWForm();
            wwwForm.AddField("seriesids", "5679");
            wwwForm.AddField("cityid", "110100");
            MainEntry.WebRequest.AddWebRequest(webRequestInfo);
        }

        private void Handler(object sender, BaseEventArgs e)
        {
            var eventArgs = e as Framework.Runtime.WebRequestSuccessEventArgs;
            if (eventArgs != null)
            {
                Debug.Log(Utility.Converter.GetString(eventArgs.WebResponseBytes));
            }
        }
    }
}