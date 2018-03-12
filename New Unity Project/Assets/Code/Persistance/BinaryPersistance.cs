using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

namespace TankGame.Persistance
{
    public class BinaryPersistance: IPersistance
    {
        public string Extension { get{ return ".bin"; } }

        public string FilePath { get; private set; }

        public BinaryPersistance(string path)
        {
            FilePath = path + Extension;
        }

        public T Load<T>()
        {
            T data = default(T);
            if(File.Exists(FilePath))
            {
                FileStream stream = File.OpenRead(FilePath);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    var surrogateSelector = new SurrogateSelector();
                    Vector3Surrogate v3ss = new Vector3Surrogate();
                    surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3ss);
                    bf.SurrogateSelector = surrogateSelector;

                    data = (T)bf.Deserialize(stream);
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
                finally
                {
                    stream.Close();
                }
            }
            return data;
        }

        public void Save<T>( T data )
        {
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            using(FileStream stream = File.OpenWrite(FilePath))
            {
                BinaryFormatter bf = new BinaryFormatter();

                var surrogateSelector = new SurrogateSelector();
                Vector3Surrogate v3ss = new Vector3Surrogate();
                surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3ss);
                bf.SurrogateSelector = surrogateSelector;

                bf.Serialize(stream, data);
                stream.Close();
            }
        }
    }
}
