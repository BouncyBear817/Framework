using System;
using Framework;
using UnityEngine;

namespace Example.ReferencePool
{
    public class Example_EventPool : MonoBehaviour
    {
        private void Start()
        {
            var eventPool = new EventPool<ExampleEventArgs>(EventPoolMode.AllowMultiHandler);

            var eventArgs = new ExampleEventArgs
            {
                Name = "example"
            };

            eventPool.Subscribe(eventArgs.Id, Handler);
            eventPool.Subscribe(eventArgs.Id, Handler1);
            
            eventPool.FireNow(this, eventArgs);
        }

        private void Handler1(object sender, ExampleEventArgs e)
        {
            Debug.Log(e.Name + "1");
        }

        private void Handler(object sender, ExampleEventArgs e)
        {
            Debug.Log(e.Name);
        }
    }
    
    public class ExampleEventArgs : BaseEventArgs
    {
        public override void Clear()
        {
            
        }

        public override int Id => 5;

        public string Name;
    }
}