using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TankGame
{
    public static class ExtensionMethods
    {
        public static TComponent GetOrAddComponent<TComponent>( this GameObject gameObject )
            where TComponent : Component
        {
            TComponent component = gameObject.GetComponent<TComponent>();
            if(component == null)
            {
                component = gameObject.AddComponent<TComponent>();
            }
            return component;
        }

        public static Component GetOrAddComponent (this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            
            if(component == null)
            {
                component = gameObject.AddComponent(type);
            }
            return component;
        }

        public static TComponent GetComponentInInactiveParent<TComponent>(this GameObject gameobject)
            where TComponent : Component
        {
            GameObject go = null;
            if(gameobject.transform.parent != null)
            {
                go = gameobject.transform.parent.gameObject;
            }
            TComponent component = go.GetComponent<TComponent>();
            if(component == null && go.transform.parent != null)
            {
                 component = go.GetComponentInInactiveParent<TComponent>();
            }
            return component;
        }

        public static TComponent GetComponentInHierarchy<TComponent> (this GameObject gameobject, bool includeInactive = false)
            where TComponent : Component
        {
            TComponent component = gameobject.GetComponent<TComponent>();

            if(component == null && !includeInactive)
            {
                component = gameobject.GetComponentInParent<TComponent>();
                if (component == null)
                {
                    component = gameobject.GetComponentInChildren<TComponent>();
                }
            }
            else if(component == null && includeInactive)
            {
                component = gameobject.GetComponentInInactiveParent<TComponent>();
                if(component == null)
                {
                    component = gameobject.GetComponentInChildren<TComponent>(true);
                }
            }
            return component;

        }

        public static bool AddUnique< T >(this IList<T> list, T item)
        {
            if(list.Contains(item))
            {
                return false;
            }

            list.Add(item);
            return true;
        }
    }
}
