using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51App
{
    public static class Util
    {
        private static Random _rng = new Random(DateTime.Now.Millisecond);

        public static T RandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            lock (_rng)
            {
                return (T)Convert.ChangeType(values.GetValue(_rng.Next(values.Length)), typeof(T));
            }
        }
    }
}
