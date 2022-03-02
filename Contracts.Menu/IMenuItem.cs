using System;

namespace Contracts.Menu
{
    public interface IMenuItem<TK, T>
    {
        public TK ItemIndex { get; }
        public string Label { get; }
        public T MethodToExecute { get; }
    }
}