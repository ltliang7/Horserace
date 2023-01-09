using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    /// <summary>
    /// 事件分发器
    /// </summary>
    public class EventDispatcher
    {
        
        /// <summary>
        /// 用于报错时能提供详细信息
        /// </summary>
        private Type RefType;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="arg"></param>
        /// <returns>在事件分发后是否移除此监听</returns>
        public delegate bool EventListener(int eventId, object arg);

        private Dictionary<int,List<EventListener>> listeners = new Dictionary<int, List<EventListener>>();
        private bool dispatching = false;
        private Queue<EventEntity> waiting = new Queue<EventEntity>();

        public EventDispatcher(Type events)
        {
            if (events.BaseType != typeof (BaseEvent))
            {
                throw new Exception("EventDispatcher's RefType must be subtype of BaseEvent!");
            }
            RefType = events;
        }
        public void AddListener(int eventId, EventListener listener)
        {
            if (!Valid(eventId,"Add"))
                return;
            List<EventListener> list;
            if(!listeners.TryGetValue(eventId, out list))
            {
                list = new List<EventListener>(4);
                listeners.Add(eventId,list);
            }
            bool added = false;
            for (int i = 0; i < list.Count; i++)
            {
                //已经在监听的不重复添加
                if (list[i] == listener)
                {
                    added = true;
                    break;
                }
            }
            if(!added)
                list.Add(listener);
        }

        public void RemoveListener(int eventId, EventListener listener)
        {
            if (!Valid(eventId, "Remove"))
                return;
            List<EventListener> list;
            if (listeners.TryGetValue(eventId, out list))
            {
                for (int i = list.Count-1;i>=0; i--)
                {
                    if(list[i]==listener)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void RemoveAllListener(int eventId, EventListener listener)
        {
            if (!Valid(eventId, "RemoveAll"))
                return;
            List<EventListener> list;
            if (listeners.TryGetValue(eventId, out list))
            {
                list.Clear();
            }
        }
        public void Clear(int eventId, EventListener listener)
        {
            if (!Valid(eventId, "Clear"))
                return;
            listeners.Clear();
        }
        /// <summary>
        /// 分发事件，同时只能进行一个分发
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="arg"></param>
        public void Dispatch(int eventId,object arg = null)
        {
            if (!Valid(eventId, "Dispatch"))
                return;
            DoDisPatch(eventId,arg);
            while (waiting.Count > 0)
            {
                EventEntity w = waiting.Dequeue();
                DoDisPatch(w.eventId,w.arg);
            }
        }

        private void DoDisPatch(int eventId, object arg = null)
        {
            dispatching = true;
            List<EventListener> list;
            if (listeners.TryGetValue(eventId, out list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    EventListener el = list[i];
                    bool needremove = false;
                    try
                    {
                        needremove = el(eventId, arg);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError(e.Message +"#error when dispatch event:"+GetEventName(eventId)+"("+eventId+")"+"\n"+e.StackTrace);
                    }
                    
                    if (needremove)
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
            }
            dispatching = false;
        }
        /// <summary>
        /// 如果正在分发事件，可延迟这次分发
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="arg"></param>
        /// <returns>是否立即执行了分发</returns>
        public bool DispathDelay(int eventId, object arg)
        {
            if (dispatching)
            {
                waiting.Enqueue(new EventEntity(eventId,arg));
                return false;
            }
            else
            {
                Dispatch(eventId,arg);
                return true;
            }
        }

        private bool Valid(int eventId, string dowhat)
        {
            if (dispatching)
            {
                UnityEngine.Debug.LogError(string.Format("event {0}({1}) is dispatching, can not {2}!", eventId, GetEventName(eventId), dowhat));
                return false;
            }
            return true;
        }

        private string GetEventName(int eventId)
        {
            FieldInfo[] fs = RefType.GetFields(BindingFlags.Static|BindingFlags.GetField| BindingFlags.Public);//使用反射获取字段列表
            foreach (FieldInfo f in fs)
            {
                if ((int)f.GetValue(null) == eventId)
                {
                    return RefType.FullName+"."+f.Name;//根据比较结果找出对应字段，以在下面打印出字段名
                }
            }
            return "can not find event";
        }
        class EventEntity
        {
            public int eventId;
            public object arg;

            public EventEntity(int eventId, object arg)
            {
                this.eventId = eventId;
                this.arg = arg;
            }
        }
        /// <summary>
        /// 所有事件类需要继承此类
        /// </summary>
        public abstract class BaseEvent
        {
            protected static int id = 0;
        }
    }
}
