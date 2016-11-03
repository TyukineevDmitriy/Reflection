using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;
using System.IO;
using System.Linq.Expressions;

namespace Application
{
    class Program
    {
        
        static void Main(string[] args)
        {
            foreach (var item in Directory.GetFiles(new FileInfo(@"..\..\..\Solution\").FullName, "*.dll"))
            {
                var DLLPlugin = Assembly.LoadFile(item);
                foreach (var type in DLLPlugin.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IPlugin)))
                    {
                        var plugin = (IPlugin)Activator.CreateInstance(type);
                        Console.WriteLine(plugin.Name);
                    }                        
                }
            }

            Expression<Func<double, double>> exp = x => Math.Sin(2 * (x + 5));
            Console.WriteLine(Differentiator.Differentiate(exp));
        }
    }
}
