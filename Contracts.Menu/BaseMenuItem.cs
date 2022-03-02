using System;

namespace Contracts.Menu
{
    public abstract class BaseMenuItem<TK, T> : IMenuItem<TK, T>
    {
        protected BaseMenuItem(TK itemIndex, string label, T methodToExecute)
        {
            ItemIndex = itemIndex;
            Label = label;
            MethodToExecute = methodToExecute;
        }

        public TK ItemIndex { get; }
        public string Label { get; }
        public T MethodToExecute { get; }
    }
}
