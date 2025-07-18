using System.Reflection;

namespace Biblioteca2.Models.Common
{
    public class BuildInfo
    {
        public DateTime BuildDate { get; }

        public BuildInfo()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            BuildDate = File.GetLastWriteTime(assemblyPath);
        }

    }
}
