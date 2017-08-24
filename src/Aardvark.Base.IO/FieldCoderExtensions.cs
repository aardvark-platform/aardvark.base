using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Aardvark.VRVis
{

    public static class FieldCoderExtensions
    {
        public static IEnumerable<FieldCoder> CreateFieldCoders(this IFieldCodeable self, params string[] members)
        {
            return CreateFieldCoders(self.GetType(), members);
        }

        public static IEnumerable<FieldCoder> CreateFieldCoders(this Type type, IEnumerable<string> members)
        {
            return CreateFieldCoders(type, members.ToArray());
        }

        public static IEnumerable<FieldCoder> CreateFieldCoders(this Type type, params string[] members)
        {
            foreach (var member in members)
            {
                var tokens = member.Split(new[] { '|' });
                if (tokens.Length == 1) yield return CreateFieldCoder(type, member, member);
                else if (tokens.Length == 2) yield return CreateFieldCoder(type, tokens[0], tokens[1]);
                else throw new ArgumentException();
            }
        }

        public static FieldCoder CreateFieldCoder(this IFieldCodeable self, string member)
        {
            return CreateFieldCoder(self, member, member);
        }

        public static FieldCoder CreateFieldCoder(this IFieldCodeable self, string name, string memberName)
        {
            return CreateFieldCoder(self.GetType(), name, memberName);
        }

        public static FieldCoder CreateFieldCoder(this Type type, string name, string memberName)
        {
            var field = type.GetField(memberName);
            if (field != null)
            {
                // template:
                // (c, o) => c.Code(ref ((Foo)o).m_myField)

                var argumentTypes = new[] { typeof(ICoder), typeof(object) };

                // create debug method
                var debugGen = EmitDebug.CreateDebugMethod(
                    string.Format("Field_{0}_{1}_{2}", s_fieldId++, type.Name, memberName), typeof(void), argumentTypes);
                EmitFieldCoder(debugGen, type, field);

                // create result lambda function
                var m = new DynamicMethod("lambda", typeof(void), argumentTypes);
                EmitFieldCoder(m.GetILGenerator(), type, field);
                var code = (Action<ICoder, object>)m.CreateDelegate(typeof(Action<ICoder, object>));
                return new FieldCoder(0, name, code);
            }

            var prop = type.GetProperty(memberName);
            if (prop != null)
            {
                // template:
                // (c, o) => 
                // {
                //     if (c.IsWriting) { var v = ((Foo)o).MyProperty; c.Code(ref v); }
                //     else { var v = 0; c.Code(ref v); ((Foo)o).MyProperty = v; }
                // }

                var argumentTypes = new[] { typeof(ICoder), typeof(object) };

                // create debug method
                var debugGen = EmitDebug.CreateDebugMethod(
                    string.Format("Property_{0}_{1}_{2}", s_propId++, type.Name, memberName), typeof(void), argumentTypes);
                EmitPropertyCoder(debugGen, type, prop);

                // create result lambda function
                var m = new DynamicMethod("lambda", typeof(void), argumentTypes);
                EmitPropertyCoder(m.GetILGenerator(), type, prop);
                var code = (Action<ICoder, object>)m.CreateDelegate(typeof(Action<ICoder, object>));
                return new FieldCoder(0, name, code);
            }

            throw new NotImplementedException();
        }

        private static void EmitFieldCoder(ILGenerator gen, Type type, FieldInfo field)
        {
            var method_Code = GetCodeMethodOverloadFor(field.FieldType);

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Castclass, type);
            gen.Emit(OpCodes.Ldflda, field);
            gen.Emit(OpCodes.Callvirt, method_Code);
            gen.Emit(OpCodes.Ret);
        }

        private static void EmitPropertyCoder(ILGenerator gen, Type type, PropertyInfo prop)
        {
            var method_IsWriting = typeof(ICoder).GetMethod(
                "get_IsWriting",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new Type[] { }, null
                );

            var method_Code = GetCodeMethodOverloadFor(prop.PropertyType);
            var typeOfCodeMethodArg = method_Code.GetParameters()[0].ParameterType;

            //var c = m.DefineParameter(1, ParameterAttributes.None, "c");
            //var o = m.DefineParameter(2, ParameterAttributes.None, "o");

            /* var v = */ gen.DeclareLocal(prop.PropertyType);
            var typeOfCodeMethodArdWithoutRef =
                Type.GetType(typeOfCodeMethodArg.FullName.Substring(0, typeOfCodeMethodArg.FullName.Length - 1));
            /* var v1 = */ gen.DeclareLocal(typeOfCodeMethodArdWithoutRef);

            var label_isReading = gen.DefineLabel();

            // if (c.IsWriting)
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Callvirt, method_IsWriting);
            gen.Emit(OpCodes.Brfalse_S, label_isReading);
            // then writing ...
            gen.Emit(OpCodes.Ldarg_1);                      // var v = ((T)o).MyProperty; c.Code(ref v);
            gen.Emit(OpCodes.Castclass, type);
            gen.Emit(OpCodes.Callvirt, prop.GetGetMethod());
            gen.Emit(OpCodes.Stloc_1);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldloca_S, 1);
            gen.Emit(OpCodes.Callvirt, method_Code);
            gen.Emit(OpCodes.Ret);
            // else reading ...
            gen.MarkLabel(label_isReading);

            // var v = default(T);
            gen.Emit(OpCodes.Ldloca, 0);                        // load address of local variable v
            gen.Emit(OpCodes.Initobj, prop.PropertyType);       // initialize v to default value

            // c.Code(ref v);                    
            gen.Emit(OpCodes.Ldarg_0);                          // c, used as this for following call
            gen.Emit(OpCodes.Ldloca, 0);                        // ref v
            gen.Emit(OpCodes.Callvirt, method_Code);            // call ... c.code(ref v)

            // ((T)o).MyProperty = v;  
            gen.Emit(OpCodes.Ldarg_1);                          // o
            gen.Emit(OpCodes.Castclass, type);                  // cast to T
            gen.Emit(OpCodes.Ldloc_0);                          // v
            //gen.Emit(OpCodes.Castclass, prop.PropertyType);
            gen.Emit(OpCodes.Callvirt, prop.GetSetMethod());    // call setter
            gen.Emit(OpCodes.Ret);
        }

        private static int s_fieldId = 0;
        private static int s_propId = 0;

        private static MethodInfo GetCodeMethodOverloadFor(Type t)
        {
            // try non-generic Code overloads first
            var mi = typeof(ICoder).GetMethod(
                "Code", BindingFlags.Instance | BindingFlags.Public, null,
                new Type[] { t.MakeByRefType() }, null
                );

            // if match exists and it is not Code(ref object obj) then we're done
            if (mi != null && mi.GetParameters()[0].ParameterType != typeof(object).MakeByRefType())
            {
                return mi;
            }

            // try Code<T>(ref T[] obj) overload
            if (t.IsArray)
            {
                var s = t.FullName.Substring(0, t.FullName.Length - 2);
                var arrayOfWhat = Type.GetType(s);
                return s_methodCodeArrayOfT.MakeGenericMethod(arrayOfWhat);
            }

            if (t.FullName.StartsWith("System.Collections.Generic.List"))
            {
                var i = t.FullName.IndexOf('[');
                var s = t.FullName.Substring(i + 2, t.FullName.Length - i - 3);
                var listOfWhat = Type.GetType(s);
                return s_methodCodeListOfT.MakeGenericMethod(listOfWhat);
            }

            // finally use Code<T>(ref T obj) overload
            return s_methodCodeT.MakeGenericMethod(t);
        }

        private static MethodInfo s_methodCodeT;
        private static MethodInfo s_methodCodeArrayOfT;
        private static MethodInfo s_methodCodeListOfT;

        static FieldCoderExtensions()
        {
            var genericCodeMethods = (
               from m in typeof(ICoder).GetMethods(
                   BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public)
               where m.IsGenericMethod
               where m.Name.StartsWith("Code")
               select new
               {
                   Method = m,
                   ParameterType = m.GetParameters()[0].ParameterType
               }
               ).ToArray();

            foreach (var m in genericCodeMethods)
            {
                switch (m.ParameterType.Name)
                {
                    case "T&": s_methodCodeT = m.Method; break;
                    case "T[]&": s_methodCodeArrayOfT = m.Method; break;
                    case "List`1&": s_methodCodeListOfT = m.Method; break;
                    default: throw new NotImplementedException();
                }
            }
        }

    }


    public static class EmitDebug
    {

        private static void BeginDebug(string debugAssemblyName)
        {
            s_assemblyName = debugAssemblyName;

            var domain = AppDomain.CurrentDomain;
            var aname = new AssemblyName(s_assemblyName);
            aname.Version = new Version(0, 0, 1);
            s_assemblyBuilder = domain.DefineDynamicAssembly(aname, AssemblyBuilderAccess.Save,
                @"C:\Data\Development\Aardvark 2008\Apps\Scratch\ScratchSm\bin\x86\Release");

            s_modBuilder = s_assemblyBuilder.DefineDynamicModule("MainModule", s_assemblyName + ".dll");

            s_typeBuilder = s_modBuilder.DefineType("Debug", TypeAttributes.Public);
        }

        public static ILGenerator CreateDebugMethod(string name, Type returnType, Type[] parameterTypes)
        {
            if (s_assemblyBuilder == null) BeginDebug("ReflectEmitDebug");

            var mb = s_typeBuilder.DefineMethod(
                name, MethodAttributes.Static | MethodAttributes.Public,
                returnType, parameterTypes);
            return mb.GetILGenerator();
        }

        public static void SaveDebug()
        {
            s_typeBuilder.CreateType();
            s_assemblyBuilder.Save(s_assemblyName + ".dll");

            s_assemblyName = null;
            s_assemblyBuilder = null;
            s_modBuilder = null;
            s_typeBuilder = null;
        }

        private static string s_assemblyName;
        private static AssemblyBuilder s_assemblyBuilder;
        private static ModuleBuilder s_modBuilder;
        private static TypeBuilder s_typeBuilder;

    }

}
