// -----------------------------------------------------------------------
// <copyright file="TreeVisitorStacksHolder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ICSSoft.STORMNET.FunctionalLanguage;

    /// <summary>
    /// Хранитель стеков для TreeVisitor.
    /// </summary>
    public class TreeVisitorStacksHolder
    {
        private readonly Stack<object> _paramsStack = new Stack<object>();

        public void PushFunction(Function function)
        {
            _paramsStack.Push((object)function);
        }

        public void PushParam(object param)
        {
            _paramsStack.Push(param);
        }

        public Function PopFunction()
        {
            object param;
            try
            {
                param = _paramsStack.Pop();
            }
            catch (Exception e)
            {
                throw new Exception("Стек параметров неожиданно оказался пуст при обходе выражения");
            }

            var f = param as Function;
            if (f == null)
            {
                throw new Exception("В этом месте ожидается параметр типа Function");
            }

            return f;
        }

        public object PopParam()
        {
            return _paramsStack.Pop();
        }

        public object PeekParam()
        {
            return _paramsStack.Peek();
        }
    }
}
