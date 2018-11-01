﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NCurses.Core.Interop.Wide;

namespace NCurses.Core.Interop.Dynamic.cchar_t
{
    internal static class cchar_tBuilder
    {
        private static Type cchar_t;// = typeof(cchar_t);

        #region Reflection.Emit implementation

        private static Type nestedBufferType;

        private class SimpleTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return x.Assembly == y.Assembly &&
                    x.Namespace == y.Namespace &&
                    x.Name == y.Name;
            }

            public int GetHashCode(Type obj)
            {
                throw new NotImplementedException();
            }
        }

        internal static Type CreateType()
        {
            if (cchar_t != null)
                return cchar_t;

            FieldBuilder attrField, extField, charField, fixedElementField;
            PropertyBuilder propBuilder;
            CustomAttributeBuilder attrBuilder;
            Type attrType;
            Type[] ctorParams;
            ConstructorInfo ctor;
            ConstructorBuilder ctorBuilder, charCtorBuilder, charAttrCtorBuilder, arrCtorBuilder, arrAttrCtorBuilder;
            ILGenerator ctorIl, methodIl;
            MethodBuilder methodBuilder, charToCcharExplicit, ccharToCharExplicit, opEquality, wchEquality;
            Label lbl1, lbl2, lbl3, lbl4;
            LocalBuilder lcl0, lcl1, lcl2, lcl3, lcl4, lcl5, lcl6;
            ParameterBuilder parameterBuilder;

            //default values
            int wcharLength = Constants.CCHARW_MAX * Constants.SIZEOF_WCHAR_T;
            //TODO: is built into netcoreapp/.NET framework
            Type readOnlyAttribute = typeof(DynamicTypeBuilder).Assembly.GetType("System.Runtime.CompilerServices.IsReadOnlyAttribute");

            TypeBuilder typeBuilder = DynamicTypeBuilder.ModuleBuilder.DefineType(
                "cchar_t",
                TypeAttributes.NotPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType));
            typeBuilder.AddInterfaceImplementation(typeof(INCursesWCHAR));
            typeBuilder.AddInterfaceImplementation(typeof(INCursesChar));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<INCursesChar>));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<INCursesWCHAR>));
 
            // nested value type for the fixed buffer 
            TypeBuilder nestedTypeBuilder = typeBuilder.DefineNestedType(
                "nestedFixedBuffer",
                TypeAttributes.NestedPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType),
                PackingSize.Unspecified,
                wcharLength);

            attrType = typeof(CompilerGeneratedAttribute);
            ctor = attrType.GetConstructor(Array.Empty<Type>());
            attrBuilder = new CustomAttributeBuilder(ctor, Array.Empty<object>());
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            attrType = typeof(UnsafeValueTypeAttribute);
            ctor = attrType.GetConstructor(Array.Empty<Type>());
            attrBuilder = new CustomAttributeBuilder(ctor, Array.Empty<object>());
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            //not necessary
            attrType = typeof(StructLayoutAttribute);
            ctorParams = new Type[] { typeof(LayoutKind) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { LayoutKind.Sequential },
                new FieldInfo[] { attrType.GetTypeInfo().GetField("Size") },
                new object[] { wcharLength });
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            fixedElementField = nestedTypeBuilder.DefineField("FixedElementField", typeof(byte), FieldAttributes.Public);

            /* fields */
            attrField = typeBuilder.DefineField("attr", DynamicTypeBuilder.chtype, FieldAttributes.Public);

            charField = typeBuilder.DefineField("chars", nestedTypeBuilder.AsType(), FieldAttributes.Public);
            attrType = typeof(FixedBufferAttribute);
            ctorParams = new Type[] { typeof(Type), typeof(int) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { typeof(byte), wcharLength });
            charField.SetCustomAttribute(attrBuilder);

            extField = typeBuilder.DefineField("ext_color", typeof(int), FieldAttributes.Public);

            /* constructors */
            #region cchar_t(char c)
            charCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(char) });
            ctorIl = charCtorBuilder.GetILGenerator();

            lbl1 = ctorIl.DefineLabel();

            lcl0 = ctorIl.DeclareLocal(typeof(bool)); //completed
            lcl1 = ctorIl.DeclareLocal(typeof(char*));
            lcl2 = ctorIl.DeclareLocal(typeof(byte*));
            lcl3 = ctorIl.DeclareLocal(typeof(byte*), true);
            lcl4 = ctorIl.DeclareLocal(typeof(int)); //charsUsed
            lcl5 = ctorIl.DeclareLocal(typeof(int)); //bytesUsed
            lcl6 = ctorIl.DeclareLocal(typeof(bool));

            ctorIl.Emit(OpCodes.Nop);
            //this.attr = 0;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Conv_I8);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { typeof(ulong) }));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            //this.ext_color = 0;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Stfld, extField);
            ctorIl.Emit(OpCodes.Nop);
            //char* ch = stackalloc char[1];
            ctorIl.Emit(OpCodes.Ldc_I4_2);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Localloc);
            ctorIl.Emit(OpCodes.Stloc_1);
            //ch[0] = c;
            ctorIl.Emit(OpCodes.Ldloc_1);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stind_I2);
            //fixed (byte* b = this.chars)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Stloc_3);
            ctorIl.Emit(OpCodes.Ldloc_3);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_2);
            ctorIl.Emit(OpCodes.Nop);
            //NativeNCurses.Encoding.GetEncoder().Convert(ch, 1, b, charGlobalLength, false, out int charsUsed, out int bytesUsed, out completed);
            ctorIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetProperty("Encoding", BindingFlags.NonPublic | BindingFlags.Static).GetMethod);
            ctorIl.Emit(OpCodes.Callvirt, typeof(Encoding).GetMethod("GetEncoder"));
            ctorIl.Emit(OpCodes.Ldloc_1);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Ldloc_2);
            ctorIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ldloca, lcl4);
            ctorIl.Emit(OpCodes.Ldloca, lcl5);
            ctorIl.Emit(OpCodes.Ldloca, lcl0);
            ctorIl.Emit(OpCodes.Callvirt, typeof(Encoder).GetMethod(
                "Convert",
                new Type[] 
                {
                    typeof(char*), typeof(int),
                    typeof(byte*), typeof(int),
                    typeof(bool),
                    typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(bool).MakeByRefType()
                }));
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Nop);
            //end fixed
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_3);
            ctorIl.Emit(OpCodes.Nop);
            //if (!completed)
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ceq);
            ctorIl.Emit(OpCodes.Stloc_S, lcl6);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl6);
            ctorIl.Emit(OpCodes.Brfalse_S, lbl1);
            //throw new InvalidOperationException("Failed to convert character for marshaling");
            ctorIl.Emit(OpCodes.Ldstr, "Failed to convert character for marshaling");
            ctorIl.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetTypeInfo().GetConstructor(new Type[] { typeof(string) }));
            ctorIl.Emit(OpCodes.Throw);
            ctorIl.MarkLabel(lbl1);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region cchar_t(char c, ulong attrs)
            charAttrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(char), typeof(ulong) });
            ctorIl = charAttrCtorBuilder.GetILGenerator();

            //: this(c)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Call, charCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //this.attr = attrs;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { typeof(ulong) }));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region cchar_t(char c, ulong attrs, short pair)
            ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(char), typeof(ulong), typeof(short) });
            ctorIl = ctorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(bool));
            lbl1 = ctorIl.DefineLabel();

            //: this(c, attrs)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Call, charAttrCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            ctorIl.Emit(OpCodes.Call, typeof(OSPlatform).GetTypeInfo().GetProperty("Windows").GetMethod);
            ctorIl.Emit(OpCodes.Call, typeof(RuntimeInformation).GetTypeInfo().GetMethod("IsOSPlatform"));
            ctorIl.Emit(OpCodes.Stloc_0);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Brfalse_S, lbl1);
            //this.ext_color = pair;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Stfld, extField);
            //this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
            ctorIl.MarkLabel(lbl1);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, attrField);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetMethod("COLOR_PAIR"));
            ctorIl.Emit(OpCodes.Conv_I8);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_BitwiseOr"));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region cchar_t(ArraySegment<byte> encodedBytesChar)
            arrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(ArraySegment<byte>) });
            ctorIl = arrCtorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(int));
            lcl1 = ctorIl.DeclareLocal(typeof(bool));
            lbl1 = ctorIl.DefineLabel();
            lbl2 = ctorIl.DefineLabel();

            ctorIl.Emit(OpCodes.Nop);
            //int i = 0
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Stloc_0);
            ctorIl.Emit(OpCodes.Br_S, lbl1);
            //unsafe
            ctorIl.MarkLabel(lbl2);
            // this.chars[i]
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Add);
            //encodedBytesChar.Array[encodedBytesChar.Offset + i];
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Array").GetMethod);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Offset").GetMethod);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Ldelem_U1);
            ctorIl.Emit(OpCodes.Stind_I1);
            //i++
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Stloc_0);
            ctorIl.MarkLabel(lbl1);
            //i < encodedBytesChar.Count
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Count").GetMethod);
            ctorIl.Emit(OpCodes.Clt);
            ctorIl.Emit(OpCodes.Stloc_1);
            ctorIl.Emit(OpCodes.Ldloc_1);
            ctorIl.Emit(OpCodes.Brtrue_S, lbl2);
            ctorIl.Emit(OpCodes.Nop);
            //this.attr = 0;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Conv_I8);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { typeof(ulong) }));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            //this.ext_color = 0;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Stfld, extField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs)
            arrAttrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(ArraySegment<byte>), typeof(ulong) });
            ctorIl = arrAttrCtorBuilder.GetILGenerator();

            //: this(encodedBytesChar)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Call, arrCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //this.attr = attrs;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { typeof(ulong) }));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs, short pair)
            ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(ArraySegment<byte>), typeof(ulong), typeof(short) });
            ctorIl = ctorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(bool));
            lbl1 = ctorIl.DefineLabel();

            //: this(encodedBytesChar, attrs)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Call, arrAttrCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            ctorIl.Emit(OpCodes.Call, typeof(OSPlatform).GetTypeInfo().GetProperty("Windows").GetMethod);
            ctorIl.Emit(OpCodes.Call, typeof(RuntimeInformation).GetTypeInfo().GetMethod("IsOSPlatform"));
            ctorIl.Emit(OpCodes.Stloc_0);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Brfalse_S, lbl1);
            //this.ext_color = pair;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Stfld, extField);
            ctorIl.MarkLabel(lbl1);
            //this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, attrField);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetMethod("COLOR_PAIR"));
            ctorIl.Emit(OpCodes.Conv_I8);
            ctorIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_BitwiseOr"));
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            /* operator overrides */
            #region public static explicit operator cchar_t(char ch)
            charToCcharExplicit = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeof(char) });
            methodIl = charToCcharExplicit.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            //return new cchar_t(ch);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Newobj, charCtorBuilder);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator char(cchar_t ch)
            ccharToCharExplicit = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(char),
                new Type[] { typeBuilder.AsType() });
            methodIl = ccharToCharExplicit.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(char));
            lcl1 = methodIl.DeclareLocal(typeof(char*));
            lcl2 = methodIl.DeclareLocal(typeof(bool));
            lcl3 = methodIl.DeclareLocal(typeof(char));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();

            //char ret = '\0';
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Nop);
            //char* chArr = stackalloc char[charGlobalLength];
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)(wcharLength * 2));
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Localloc);
            methodIl.Emit(OpCodes.Stloc_1);
            //if (NativeNCurses.Encoding.GetChars(ch.chars, charGlobalLength, chArr, charGlobalLength) > 0)
            methodIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetProperty("Encoding", BindingFlags.NonPublic | BindingFlags.Static).GetMethod);
            methodIl.Emit(OpCodes.Ldarga_S, (byte)0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)(wcharLength / 2));
            methodIl.Emit(OpCodes.Callvirt, typeof(Encoding).GetMethod(
                "GetChars",
                new Type[] { typeof(byte*), typeof(int), typeof(char*), typeof(int) }));
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Cgt);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //ret = chArr[0];
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldind_U2);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            //throw new InvalidOperationException("Failed to cast to character");
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldstr, "Failed to cast to character");
            methodIl.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetTypeInfo().GetConstructor(new Type[] { typeof(string) }));
            methodIl.Emit(OpCodes.Throw);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl3);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            //TODO: fix for linux
            #region public static bool operator ==(in cchar_t wchLeft, in cchar_t wchRight)
            opEquality = typeBuilder.DefineMethod("op_Equality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });
            methodIl = opEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(byte*));
            lcl1 = methodIl.DeclareLocal(typeof(byte*));
            lcl2 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl3 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl4 = methodIl.DeclareLocal(typeof(ReadOnlySpan<>).MakeGenericType(typeof(byte))); //left
            lcl5 = methodIl.DeclareLocal(typeof(ReadOnlySpan<>).MakeGenericType(typeof(byte))); //right
            lcl6 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();

            parameterBuilder = opEquality.DefineParameter(1, ParameterAttributes.In, "wchLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = opEquality.DefineParameter(2, ParameterAttributes.In, "wchRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            //fixed(byte* leftPtr = wchLeft.chars, rightPtr = wchRight.chars)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Nop);
            //ReadOnlySpan<byte> left = new ReadOnlySpan<byte>(leftPtr, charGlobalLength);
            methodIl.Emit(OpCodes.Ldloca_S, lcl4);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(ReadOnlySpan<>).MakeGenericType(typeof(byte)).GetConstructor(new Type[] { typeof(void*), typeof(int) }));
            //ReadOnlySpan<byte> right = new ReadOnlySpan<byte>(rightPtr, charGlobalLength);
            methodIl.Emit(OpCodes.Ldloca_S, lcl5);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(ReadOnlySpan<>).MakeGenericType(typeof(byte)).GetConstructor(new Type[] { typeof(void*), typeof(int) }));
            //left.SequenceEqual(right)
            methodIl.Emit(OpCodes.Ldloca_S, lcl4);
            methodIl.Emit(OpCodes.Ldloca_S, lcl5);
            //TODO: this method returns false on linux, runs fine when not generated (cfr <cref="NCursesWCHAR.Equals"/>)
            methodIl.Emit(OpCodes.Call, typeof(MemoryExtensions).GetMethods()
                .FirstOrDefault(m => m.Name.Equals("SequenceEqual")
                    && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new Type[] { typeof(ReadOnlySpan<>), typeof(ReadOnlySpan<>) }, new SimpleTypeComparer()))
                .MakeGenericMethod(typeof(byte)));
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //wchLeft.attr == wchRight.attr
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, attrField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, attrField);
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Equality"));
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //wchLeft.ext_color == wchRight.ext_color
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_S, lcl6);
            methodIl.Emit(OpCodes.Br_S, lbl3);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldloc_S, lcl6);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region  public static bool operator !=(in cchar_t wchLeft, in cchar_t wchRight)
            methodBuilder = typeBuilder.DefineMethod("op_Inequality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(byte*));
            lcl1 = methodIl.DeclareLocal(typeof(byte*));
            lcl2 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl3 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl4 = methodIl.DeclareLocal(typeof(ReadOnlySpan<byte>)); //left
            lcl5 = methodIl.DeclareLocal(typeof(ReadOnlySpan<byte>)); //right
            lcl6 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();

            parameterBuilder = methodBuilder.DefineParameter(1, ParameterAttributes.In, "wchLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = methodBuilder.DefineParameter(2, ParameterAttributes.In, "wchRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            //fixed(byte* leftPtr = wchLeft.chars, rightPtr = wchRight.chars)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Nop);
            //ReadOnlySpan<byte> left = new ReadOnlySpan<byte>(leftPtr, charGlobalLength);
            methodIl.Emit(OpCodes.Ldloca_S, lcl4);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(ReadOnlySpan<byte>).GetConstructor(new Type[] { typeof(void*), typeof(int) }));
            //ReadOnlySpan<byte> right = new ReadOnlySpan<byte>(rightPtr, charGlobalLength);
            methodIl.Emit(OpCodes.Ldloca_S, lcl5);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(ReadOnlySpan<byte>).GetConstructor(new Type[] { typeof(void*), typeof(int) }));
            //left.SequenceEqual(right)
            methodIl.Emit(OpCodes.Ldloca_S, lcl4);
            methodIl.Emit(OpCodes.Ldloca_S, lcl5);
            methodIl.Emit(OpCodes.Call, typeof(MemoryExtensions).GetMethods()
                .FirstOrDefault(m => m.Name.Equals("SequenceEqual")
                    && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new Type[] { typeof(ReadOnlySpan<>), typeof(ReadOnlySpan<>) }, new SimpleTypeComparer()))
                .MakeGenericMethod(typeof(byte)));
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //wchLeft.attr == wchRight.attr
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, attrField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, attrField);
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Equality"));
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //wchLeft.ext_color == wchRight.ext_color
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_S, lcl6);
            methodIl.Emit(OpCodes.Br_S, lbl3);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldloc_S, lcl6);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            /* methods */
            #region public override bool Equals(object obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(object) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(object));
            lcl3 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            //if (obj is cchar_t other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            //other == this
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloca_S, lcl0);
            methodIl.Emit(OpCodes.Call, opEquality);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public bool Equals(INCursesWCHAR obj)
            wchEquality = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(INCursesWCHAR) });
            methodIl = wchEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(INCursesWCHAR));
            lcl3 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            //if (obj is cchar_t other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            //other == this
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloca_S, lcl0);
            methodIl.Emit(OpCodes.Call, opEquality);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public bool Equals(INCursesChar obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(INCursesChar) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(INCursesWCHAR));
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();

            //if (obj is INCursesWCHAR other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Isinst, typeof(INCursesWCHAR));
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldnull);
            methodIl.Emit(OpCodes.Cgt_Un);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //this.Equals(other);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Call, wchEquality);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public override int GetHashCode()
            methodBuilder = typeBuilder.DefineMethod("GetHashCode",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                typeof(int),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(int));
            lcl1 = methodIl.DeclareLocal(typeof(byte*));
            lcl2 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl3 = methodIl.DeclareLocal(typeof(int));

            lbl1 = methodIl.DefineLabel();

            //var hashCode = -946517152;
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldc_I4, -946517152);
            methodIl.Emit(OpCodes.Stloc_0);
            //hashCode = hashCode * -1521134295 + EqualityComparer<chtype.chtype>.Default.GetHashCode(this.attr);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ldc_I4, -1521134295);
            methodIl.Emit(OpCodes.Mul);
            methodIl.Emit(OpCodes.Call, typeof(EqualityComparer<>).MakeGenericType(DynamicTypeBuilder.chtype).GetProperty("Default").GetMethod);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, attrField);
            methodIl.Emit(OpCodes.Callvirt, typeof(EqualityComparer<>).MakeGenericType(DynamicTypeBuilder.chtype).GetMethod("GetHashCode", new Type[] { DynamicTypeBuilder.chtype }));
            methodIl.Emit(OpCodes.Add);
            methodIl.Emit(OpCodes.Stloc_0);
            //fixed(byte* b = this.chars)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_1);
            //hashCode = hashCode * -1521134295 + (int)b;
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ldc_I4, -1521134295);
            methodIl.Emit(OpCodes.Mul);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Conv_I4);
            methodIl.Emit(OpCodes.Add);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_2);
            //hashCode = hashCode * -1521134295 + this.ext_color.GetHashCode();
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ldc_I4, -1521134295);
            methodIl.Emit(OpCodes.Mul);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, extField);
            methodIl.Emit(OpCodes.Call, typeof(int).GetMethod("GetHashCode"));
            methodIl.Emit(OpCodes.Add);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            /* properties */
            #region Attributes
            methodBuilder = typeBuilder.DefineMethod("get_Attributes",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(ulong),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            //(ulong)(this.attr ^ (this.attr & Attrs.COLOR));
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, attrField);
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { DynamicTypeBuilder.chtype }));
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, attrField);
            methodIl.Emit(OpCodes.Ldsfld, typeof(Attrs).GetField("COLOR"));
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_BitwiseAnd", new Type[] { DynamicTypeBuilder.chtype, typeof(ulong) }));
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { DynamicTypeBuilder.chtype }));
            methodIl.Emit(OpCodes.Xor);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Attributes",
                PropertyAttributes.None,
                typeof(ulong),
                new Type[] { typeof(ulong) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Char
            methodBuilder = typeBuilder.DefineMethod("get_Char",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(char),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldobj, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Call, ccharToCharExplicit);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Char",
                PropertyAttributes.None,
                typeof(char),
                new Type[] { typeof(char) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Color
            methodBuilder = typeBuilder.DefineMethod("get_Color",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(short),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Bgt_S, lbl1);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, attrField);
            methodIl.Emit(OpCodes.Call, DynamicTypeBuilder.chtype.GetMethod("op_Implicit", new Type[] { DynamicTypeBuilder.chtype }));
            methodIl.Emit(OpCodes.Call, typeof(Constants).GetMethod("PAIR_NUMBER"));
            methodIl.Emit(OpCodes.Conv_I2);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, extField);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Conv_I2);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Color",
                PropertyAttributes.None,
                typeof(short),
                new Type[] { typeof(short) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            nestedBufferType = nestedTypeBuilder.CreateTypeInfo().AsType();
            return (cchar_t = typeBuilder.CreateTypeInfo().AsType());
        }

        #endregion
    }
}