using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarBird
{
    internal static class Core
    {
        public static JarBirdEntities Context = new JarBirdEntities();
        public static Users AuthUser = null;
    }
    public partial class Products
    {
        public bool DiscountIsGreatThen10 => DiscountProcent > 10;
    }
}
