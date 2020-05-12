using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Trudograd.NuclearEdition
{
    public sealed class StaticFieldAccessor<T>
    {
        private delegate T GetValue();

        private delegate void SetValue(T value);

        private readonly GetValue _getter;
        private readonly SetValue _setter;

        public StaticFieldAccessor(FieldInfo fieldInfo)
        {
            _getter = CreateFieldGetter(fieldInfo);
            _setter = CreateFieldSetter(fieldInfo);
        }

        public T Value
        {
            get => _getter();
            set => _setter(value);
        }

        private static GetValue CreateFieldGetter(FieldInfo fieldInfo)
        {
            Type instanceType = fieldInfo.DeclaringType ?? throw new ArgumentException(nameof(fieldInfo));
            Type valueType = fieldInfo.FieldType;
            DynamicMethod method = new DynamicMethod($"StaticFieldAccessor_Get_{fieldInfo.Name}", valueType, null, instanceType, true);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldsfld, fieldInfo);
            il.Emit(OpCodes.Ret);
            return (GetValue) method.CreateDelegate(typeof(GetValue));
        }

        private static SetValue CreateFieldSetter(FieldInfo fieldInfo)
        {
            Type instanceType = fieldInfo.DeclaringType ?? throw new ArgumentException(nameof(fieldInfo));
            Type valueType = fieldInfo.FieldType;
            DynamicMethod method = new DynamicMethod($"StaticFieldAccessor_Set_{fieldInfo.Name}", typeof(void), new[] {valueType}, instanceType, true);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stsfld, fieldInfo);
            il.Emit(OpCodes.Ret);
            return (SetValue) method.CreateDelegate(typeof(SetValue));
        }
    }
}