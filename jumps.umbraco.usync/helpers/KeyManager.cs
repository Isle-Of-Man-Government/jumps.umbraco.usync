using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jumps.umbraco.usync.helpers
{
    /*
    /// <summary>
    ///  class to manage the keys across installs
    ///  
    ///  this is borrowed from ContentEdition, it allows mapping
    ///  of ids between installations, but it also allows us to do
    ///  deletes.
    /// </summary>
    public class KeyManager
    {
        private static Dictionary<Guid, Guid> KeyMap ;

        static KeyManager()
        {
            LoadKeyMap();
        }


        private static void LoadKeyMap()
        {
            KeyMap = new Dictionary<Guid, Guid>();
        }


        /// <summary>
        ///  with a local key - does a look up and finds any masters if there 
        ///  are in our sync file.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Guid GetMasterKey(Guid local)
        {
            if (KeyMap.ContainsKey(local))
            {
                return KeyMap[local];
            }
            return local;
        }

        // with a master key (on import) looks to see if we have any local keys
        // mapped to it
        public static Guid GetLocalKey(Guid master)
        {
            if (KeyMap.ContainsValue(master))
            {
                return master;
            }
            return master;
        }

        public static void AddToKeyMap(Guid Source, Guid Master)
        {
            if (KeyMap.ContainsKey(Source))
            {
                KeyMap.Remove(Source);
                KeyMap.Add(Source, Master);
            }
        }
    }*/
}
