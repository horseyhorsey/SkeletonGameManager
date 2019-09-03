using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SkeletonGame.Models
{
    public static class CloneExtensions
    {
        /// <summary>
        /// Performs a deep clone on serialize-able object
        /// </summary>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) // where T : ISerializable
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
