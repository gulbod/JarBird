using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarBird
{
    internal static class Core
    {
        public static JarBirdEnitities Context = new JarBirdEnitities();
        public static Users AuthUser = null;
    }
}
