using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Persistance {
    public interface IPersistance
    {
        string Extension { get; }
        
        string FilePath { get; }

        void Save<T>( T data );

        T Load<T>();
    }
}
