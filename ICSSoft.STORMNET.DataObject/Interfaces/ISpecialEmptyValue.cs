using System;
using System.Collections.Generic;
using System.Text;

namespace ICSSoft.STORMNET
{
    /// <summary>
    /// Интерфейс определяет значение, рассматриваемое в качестве пустого для данного типа.
    /// </summary>
    public interface ISpecialEmptyValue
    {
        /// <summary>
        /// Является ли значение пустым.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsEmptyValue(object value);
    }
}
