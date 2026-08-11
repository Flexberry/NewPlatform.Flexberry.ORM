namespace ICSSoft.STORMNET.Tests.TestClasses.Common
{
    using System;

    using Xunit;

    public class TypeFullNameTest
    {
        [Fact]
        public void GetUsableTypeNameTest()
        {
            // Задача: есть тип bool?, который надо использовать в сгенерённом коде. У меня есть только Type в генераторе. Можно ли получить нормальное имя, к которому можно написать приведение? Например, (bool?)myVar
            Type tBool = typeof(bool?);
            Type tInt = typeof(int?);
            string str = "bool?: " + tBool.Name + " int?: " + tInt.Name;
            Console.WriteLine(str);
            Nullable<bool> b = true;
            System.Int32? i = 7;
            str = Nullable.GetUnderlyingType(tBool).FullName;
            Console.WriteLine(str);
            Type? myValue = Nullable.GetUnderlyingType(typeof(int));
            Console.WriteLine(myValue == null ? "null" : myValue.FullName);
            Assert.Null(myValue);
        }
    }
}
