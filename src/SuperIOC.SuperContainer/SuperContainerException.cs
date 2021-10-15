using System;

namespace SuperIOC.SuperContainer
{
    public class SuperContainerException : Exception
    {
        public SuperContainerException(string message) : base(message)
        {
        }

        public SuperContainerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}