// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KGameEvent" company=""></copyright>
// <summary></summary>
// ***********************************************************************
namespace Game
{
    using System;
    using K.Events;

    public class KGameEvent : EventParams
    {
        private int _id;
        private object _data;

        public override int eventID
        {
            get { return _id; }
        }

        public object data
        {
            get { return _data; }
        }

        protected KGameEvent()
        {
        }

        public static void Subscribe(int id, EventHandler<KGameEvent> handler)
        {
            KFramework.EventManager.Subscribe(id, (sender, eventParam) =>
            {
                handler(sender, (KGameEvent)eventParam);
            });
        }

        public static void Unsubscribe(int id, EventHandler<KGameEvent> handler)
        {
            KFramework.EventManager.Unsubscribe(id, null);
        }

        public static KGameEvent CreateEvent(int id, object data)
        {
            return new KGameEvent
            {
                _id = id,
                _data = data,
            };
        }
    }
}
