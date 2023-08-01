using System;
using System.Reflection;

namespace budgetCSgharp
{ 
    class Resolver
    {
        private static volatile bool _loaded;

        public static void RegisterDependencyResolver()
        {
            if (!_loaded)
            {
                AppDomain.CurrentDomain.AssemblyResolve += OnResolve;
                _loaded = true;
            }
        }

        private static Assembly OnResolve(object sender, ResolveEventArgs args)
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            string resourceName = string.Format("{0}.{1}.dll",
                 execAssembly.GetName().Name,
                new AssemblyName(args.Name).Name);

            using (System.IO.Stream stream = execAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    int read = 0, toRead = (int)stream.Length;
                    byte[] data = new byte[toRead];
                    do
                    {
                        int n = stream.Read(data, read, data.Length - read);
                        toRead -= n;
                        read += n;
                    } while (toRead > 0);
                    return Assembly.Load(data);
                }
                return null;

            }
        }

    }
}
