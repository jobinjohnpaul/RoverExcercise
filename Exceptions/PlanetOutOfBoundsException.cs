using System;
using System.Collections.Generic;
using System.Text;

namespace RoverExcercise.Exceptions
{
    [Serializable]
    public class PlanetOutOfBoundsException : Exception
    {
        public PlanetOutOfBoundsException() { }
        public PlanetOutOfBoundsException(string message) : base(message) { }
        public PlanetOutOfBoundsException(string message, Exception inner) : base(message, inner) { }
        protected PlanetOutOfBoundsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
