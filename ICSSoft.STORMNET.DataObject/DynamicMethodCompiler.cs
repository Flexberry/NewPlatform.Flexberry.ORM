using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ICSSoft.STORMNET
{
    /// <summary>
    /// делегат для GetProperty.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public delegate object GetHandler(object source);

    /// <summary>
    /// делегат для SetProperty.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    public delegate void SetHandler(object source, object value);

    /// <summary>
    /// Делегат для создания.
    /// </summary>
    /// <returns></returns>
    public delegate object InstantiateObjectHandler();

    /// <summary>
    /// Класс для замены рефлекшена (работает быстрее).
    /// </summary>
    public sealed class DynamicMethodCompiler
    {
        // DynamicMethodCompiler
        private DynamicMethodCompiler()
        {
        }

        // CreateInstantiateObjectDelegate
        internal static InstantiateObjectHandler CreateInstantiateObjectHandler(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            if (constructorInfo == null)
            {
                throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", type));
            }

            DynamicMethod dynamicMethod = new DynamicMethod("InstantiateObject", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(object), null, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);
            return (InstantiateObjectHandler)dynamicMethod.CreateDelegate(typeof(InstantiateObjectHandler));
        }

        // CreateGetDelegate
        internal static GetHandler CreateGetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Call, getMethodInfo);
            BoxIfNeeded(getMethodInfo.ReturnType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        // CreateGetDelegate
        internal static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            BoxIfNeeded(fieldInfo.FieldType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        // CreateSetDelegate
        internal static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            // (Колчанов Андрей) мне тут было не понятно, я разбиралсо и дописывал Trim
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator il = dynamicSet.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // (Колчанов Андрей) Запихиваем в стек инстанцию объекта
            il.Emit(OpCodes.Ldarg_1); // (Колчанов Андрей) Запихиваем в стек первый параметр, это есть устанавливаемое в свойство значение
            UnboxIfNeeded(setMethodInfo.GetParameters()[0].ParameterType, il);

            if (propertyInfo.PropertyType == typeof(string)) // (Колчанов Андрей) Этот код я перенёс из информейшна, чтобы было быстрее, чтобы Trim выполнялся в хандлере, а не в информейшне, ибо не надо будет проверять TrimmedStringStorage для каждого значения
            {
                if (Information.TrimmedStringStorage(type, propertyInfo.Name)) // (Колчанов Андрей) Проверяем наличие атрибута
                {
                    Label skiptrimlabel = il.DefineLabel(); // (Колчанов Андрей) Метка, куда будет прыгать проверка на null
                    MethodInfo miTrimInfo = typeof(string).GetMethod("Trim", new Type[] { });

                    il.Emit(OpCodes.Ldarg_1); // (Колчанов Андрей) Ещё раз запихуем в стек значение, ибо Brfalse его проверит и выбросит, а оно нам дальше понадобится.
                    il.Emit(OpCodes.Brfalse_S, skiptrimlabel); // (Колчанов Андрей) Если последнее запиханное в стек значение null, то будет переход на skiptrimlabel. Используется однобайтовый вариант Brfalse, авось будет ещё чуть-чуть быстрее
                    il.Emit(OpCodes.Call, miTrimInfo); // (Колчанов Андрей) Вызов Trim()
                    il.MarkLabel(skiptrimlabel); // (Колчанов Андрей) Метка, куда переходит Brfalse
                }
            }

            il.Emit(OpCodes.Call, setMethodInfo); // (Колчанов Андрей) Вызов аксессора Set
            il.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }

        // CreateSetDelegate
        internal static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(fieldInfo.FieldType, setGenerator);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }

        // CreateGetDynamicMethod
        private static DynamicMethod CreateGetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
        }

        // CreateSetDynamicMethod
        private static DynamicMethod CreateSetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicSet", typeof(void), new Type[] { typeof(object), typeof(object) }, type, true);
        }

        // BoxIfNeeded
        private static void BoxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        // UnboxIfNeeded
        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }
    }
}