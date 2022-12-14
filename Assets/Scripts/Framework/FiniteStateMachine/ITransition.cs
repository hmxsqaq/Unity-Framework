using System;

namespace Framework
{
    public interface ITransition<T> where T : Enum
    {
        bool Transition(out T type);
    }
}