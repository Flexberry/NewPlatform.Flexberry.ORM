namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Вспомогательные методы для тестирования.
    /// </summary>
    public static class Utils
    {
        public static View GetDefaultView(Type type)
        {
            if (type == typeof(ТипЛапы))
            {
                return Information.GetView("ТипЛапыE", type);
            }

            if (type == typeof(Лапа))
            {
                return Information.GetView("ЛапаE", type);
            }

            if (type == typeof(Перелом))
            {
                return Information.GetView("ПереломE", type);
            }

            if (type == typeof(Котенок))
            {
                return Information.GetView("КотенокE", type);
            }

            if (type == typeof(Порода))
            {
                return Information.GetView("ПородаE", type);
            }

            return Information.GetView("КошкаE", type);
        }

        /*
        /// <summary>
        /// Сравнение двух LCS
        /// </summary>
        /// <param name="lcs1"></param>
        /// <param name="lcs2"></param>
        /// <returns></returns>
        public static bool LcsEqual(LoadingCustomizationStruct lcs1, LoadingCustomizationStruct lcs2)
        {
            bool lfEq = lcs1.LimitFunction == null
                            ? lcs2.LimitFunction == null
                            : lcs1.LimitFunction.ToString() == lcs2.LimitFunction.ToString();

            bool returnTopEq = lcs1.ReturnTop == lcs2.ReturnTop;

            bool sortEq = ColumnSortEqual(lcs1.ColumnsSort, lcs2.ColumnsSort);

            bool retTypeEq = lcs1.ReturnType == lcs2.ReturnType;

            return lfEq && returnTopEq && sortEq && retTypeEq;
        }

        private static bool ColumnSortEqual(ColumnsSortDef[] csd1, ColumnsSortDef[] csd2)
        {
            if (csd1 == null)
            {
                if (csd2 == null) return true;
                if (csd2.Length == 0) return true;
                return false;
            }

            for (int i = 0; i < csd1.Length; i++)
            {
                if ((csd1[i].Name != csd2[i].Name) || (csd1[i].Sort != csd2[i].Sort))
                {
                    return false;
                }
            }

            return true;
        }
         */
    }
}
