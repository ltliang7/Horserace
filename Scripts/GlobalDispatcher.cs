using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{ 
    public class GlobalDispatcher:EventDispatcher
    {
        private static GlobalDispatcher _instance;
        public static GlobalDispatcher Instance
        {
            get { return _instance; }
        }

        public static void Create()
        {
            _instance = new GlobalDispatcher(typeof(GlobalEvent));
        }
        public GlobalDispatcher(Type events) : base(events)
        {
        }
    }
}
