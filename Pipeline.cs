using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Internal;

namespace Piper
{
    public class Pipeline<T>
    {
        
        public static Func<T, T> Pipe(params Func<T, T>[] callbacks)
        {
            return value =>
            {
                return callbacks.Aggregate(
                    value,
                    (acc, cb) =>
                    {
                        try
                        {
                            return cb(acc);
                        }
                        catch (Exception e)
                        {
                            return acc;
                        }
                    }
                );
            };
        }

        public static Func<T, T> IfNotNullProp(string prop, Func<T, T> callback)
        {
            return obj =>
            {
                var props = obj.GetType().GetProperties();
                PropertyInfo thisProp = props.First(_prop => _prop.Name == prop);
                var value = thisProp.GetValue(obj, null);

                return value != null ? callback(obj) : obj;
                
            };
        }

        public static Func<T, T> IfNotNull(Func<T, T> callback)
        {
            return obj => obj != null ? callback(obj) : obj;
        }
        
        
    }
}