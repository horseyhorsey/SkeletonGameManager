using SkeletonGame.Models;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SkeletonGame.Engine
{
    public interface ISkeletonGameSerializer
    {
        /// <summary>
        /// Converts a yaml file to json.
        /// </summary>
        /// <param name="yamlFile">The yaml file.</param>
        /// <returns></returns>
        string ConvertToJson(string yamlFile);

        /// <summary>
        /// Deserializes the yaml from given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yamlFile">The yaml file.</param>
        /// <returns></returns>
        T DeserializeSkeletonYaml<T>(string yamlFile, bool ignoreUnmatched = false);

        /// <summary>
        /// Serializes the game assets to yaml
        /// </summary>
        /// <param name="yamlFile">The yaml file.</param>
        /// <param name="yamlObjectGraph">The asset file object.</param>
        void SerializeYaml(string yamlFile, object yamlObjectGraph);

    }

    public class SkeletonGameSerializer : ISkeletonGameSerializer
    {
        /// <summary>
        /// Deserializes the yaml from given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yamlFile">The yaml file.</param>
        /// <returns></returns>
        public T DeserializeSkeletonYaml<T>(string yamlFile, bool ignoreUnmatched)
        {
            try
            {
                DeserializerBuilder deserializer;
                if (ignoreUnmatched)
                {
                    deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().WithNamingConvention(new CamelCaseNamingConvention());
                }
                else
                {
                    deserializer = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention());
                }

                var deserialize = deserializer.Build();
                using (TextReader reader = File.OpenText(yamlFile))
                {
                    var objects = deserialize.Deserialize<T>(reader);

                    return objects;
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Add("yaml", yamlFile);
                ex.Data.Add("err", ex.InnerException.Message);
                throw ex;
            }
        }


        public string ConvertToJson(string yamlFile)
        {
            using (TextReader reader = File.OpenText(yamlFile))
            {
                var deserializer = new DeserializerBuilder().Build();
                var yamlObject = deserializer.Deserialize(reader);

                var serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();

                return serializer.Serialize(yamlObject);
            }
        }

        public void SerializeYaml(string yamlFile, object yamlObjectGraph)
        {
            var yamlSerializer = new Serializer();

            using (TextWriter writer = File.CreateText(yamlFile))
            {
                yamlSerializer.Serialize(writer, yamlObjectGraph);
            }
        }

    }
}
