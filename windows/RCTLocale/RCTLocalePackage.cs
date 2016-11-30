using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using ReactNative.Bridge;
using ReactNative.Modules.Core;
using ReactNative.UIManager;

namespace io.fixd.rctlocale
{
    public class RCTLocalePackage : IReactPackage
    {
        public IReadOnlyList<INativeModule> CreateNativeModules(ReactContext reactContext)
        {
            return new List<INativeModule>
            {
                new RCTLocaleModule(reactContext)
            };
        }

        public IReadOnlyList<Type> CreateJavaScriptModulesConfig()
        {
            return new List<Type>(0);
        }

        public IReadOnlyList<IViewManager> CreateViewManagers(ReactContext reactContext)
        {
            return new List<IViewManager>(0);
        }
    }
}
