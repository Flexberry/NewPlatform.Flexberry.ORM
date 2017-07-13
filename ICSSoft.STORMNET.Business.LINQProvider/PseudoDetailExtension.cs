namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// Методы-расширения linq для работы с псевдодетейлами.
    /// </summary>
    public static class PseudoDetailExtension
    {
        /// <summary>
        /// Задание ограничений на псевдодетейлы (в случае обычных детейлов лучше использовать стандартный функционал):
        /// выбрать экземпляры класса, хотя бы один псевдодетейл которого удовлетворяют условию.
        /// </summary>
        /// <param name="source"> Множество элементов класса, на псевдодетейлы которого и накладывается ограничение.  </param>
        /// <param name="view"> Имя представления детейла, по которому будет осуществляться поиск.  </param>
        /// <param name="masterLinkName"> Имя свойства в классе псевдодетейла, по которому идёт обращение к мастеру.   </param>
        /// <param name="predicate"> Ограничение на псевдодетейл.  </param>
        /// <param name="masterToDetailPseudoProperty"> Имя свойства, которое будет обозначать связь от мастера к детейлу.  </param>
        /// <typeparam name="T"> Тип объектов, к которым делается запрос.  </typeparam>
        /// <typeparam name="TP"> Тип псевдодетейла.  </typeparam>
        /// <returns> Экземпляры класса, псевдодетейлы которого удовлетворяют условию.  </returns>
        [Obsolete("В настоящее время необходимо использовать функциональность, связанную с классом PseudoDetail.")]
        public static IQueryable<T> Where<T, TP>(
            this IQueryable<T> source,
            ICSSoft.STORMNET.View view,
            string masterLinkName,
            Expression<Func<IQueryable<TP>, bool>> predicate,
            string masterToDetailPseudoProperty = null)
            where T : DataObject
            where TP : DataObject
        {
            // Поскольку дальше сложнее получать типы T и TP, то проверка представления проходит здесь
            if (Information.GetView(view.Name, typeof(TP)) == null)
            { // То есть представление с указанным именем не найдено в классе
                throw new CantFindViewException(typeof(TP), view.Name);
            }

            if (predicate.Body == null
                || !(predicate.Body is MethodCallExpression))
            {
                throw new NotSupportedException();
            }

            var methodCallExpression = (MethodCallExpression)predicate.Body;
            var pseudoDetail = new PseudoDetail<T, TP>(view, masterLinkName, masterToDetailPseudoProperty);
            if (methodCallExpression.Method.Name == "Any")
            {
                switch (methodCallExpression.Arguments.Count())
                {
                    case 1:
                        return source.Where(x => pseudoDetail.Any(y => true));
                    case 2:
                        var realLambda = (Expression<Func<TP, bool>>)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;
                        return source.Where(x => pseudoDetail.Any(realLambda));
                    default:
                        throw new NotSupportedException();
                }
            }

            if (methodCallExpression.Method.Name == "All" && methodCallExpression.Arguments.Count() == 2)
            {
                var realLambda = (Expression<Func<TP, bool>>)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;
                return source.Where(x => pseudoDetail.All(realLambda));
            }

            throw new NotSupportedException();
        }


        /// <summary>
        /// Задание ограничений на псевдодетейлы (в случае обычных детейлов лучше использовать стандартный функционал):
        /// выбрать экземпляры класса, хотя бы один псевдодетейл которого удовлетворяют условию.
        /// </summary>
        /// <param name="source"> Множество элементов класса, на псевдодетейлы которого и накладывается ограничение.  </param>
        /// <param name="view"> Имя представления детейла, по которому будет осуществляться поиск. </param>
        /// <param name="masterLink"> Свойство в классе псевдодетейла, по которому идёт обращение к мастеру.  </param>
        /// <param name="predicate"> Ограничение на псевдодетейл. </param>
        /// <param name="masterToDetailPseudoProperty"> Имя свойства, которое будет обозначать связь от мастера к детейлу. </param>
        /// <typeparam name="T"> Тип объектов, к которым делается запрос. </typeparam>
        /// <typeparam name="TP"> Тип псевдодетейла. </typeparam>
        /// <returns> Экземпляры класса, псевдодетейлы которого удовлетворяют условию.  </returns>
        [Obsolete("В настоящее время необходимо использовать функциональность, связанную с классом PseudoDetail.")]
        public static IQueryable<T> Where<T, TP>(
            this IQueryable<T> source,
            ICSSoft.STORMNET.View view,
            Expression<Func<TP, object>> masterLink,
            Expression<Func<IQueryable<TP>, bool>> predicate,
            string masterToDetailPseudoProperty = null)
            where T : DataObject
            where TP : DataObject
        {
            string masterLinkName = Information.ExtractPropertyPath(masterLink);
            return source.Where(view, masterLinkName, predicate, masterToDetailPseudoProperty);
        }

        /// <summary>
        /// Задание ограничений на детейлы (на псевдодетейлы данная функциональность не распространяется).
        /// </summary>
        /// <param name="source"> Множество элементов класса, на псевдодетейлы которого и накладывается ограничение.   </param>
        /// <param name="view"> Имя представления детейла, по которому будет осуществляться поиск.  </param>
        /// <param name="predicate"> Ограничение на псевдодетейл.  </param>
        /// <param name="masterToDetailPseudoProperty"> Имя свойства, которое будет обозначать связь от мастера к детейлу. </param>
        /// <typeparam name="T"> Тип объектов, к которым делается запрос.  </typeparam>
        /// <typeparam name="TP"> Тип псевдодетейла. </typeparam>
        /// <returns> Экземпляры класса, псевдодетейлы которого удовлетворяют условию.   </returns>
        [Obsolete("В настоящее время необходимо использовать функциональность, связанную с классом PseudoDetail.")]
        public static IQueryable<T> Where<T, TP>(
            this IQueryable<T> source,
            ICSSoft.STORMNET.View view,
            Expression<Func<IQueryable<TP>, bool>> predicate,
            string masterToDetailPseudoProperty = null)
            where T : DataObject
            where TP : DataObject
        {
            var masterLinkName = Information.GetAgregatePropertyName(typeof(TP));
            if (string.IsNullOrEmpty(masterLinkName))
            {
                throw new NotFoundAggregatorProperty();
            }

            return source.Where(view, masterLinkName, predicate, masterToDetailPseudoProperty);
        }
    }
}
