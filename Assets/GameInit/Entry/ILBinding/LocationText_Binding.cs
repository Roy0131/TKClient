using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class LocationText_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::LocationText);
            args = new Type[]{};
            method = type.GetMethod("IDInValid", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IDInValid_0);

            field = type.GetField("IDLanguage", flag);
            app.RegisterCLRFieldGetter(field, get_IDLanguage_0);
            app.RegisterCLRFieldSetter(field, set_IDLanguage_0);


        }


        static StackObject* IDInValid_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::LocationText instance_of_this_method = (global::LocationText)typeof(global::LocationText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IDInValid();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_IDLanguage_0(ref object o)
        {
            return ((global::LocationText)o).IDLanguage;
        }
        static void set_IDLanguage_0(ref object o, object v)
        {
            ((global::LocationText)o).IDLanguage = (System.Int32)v;
        }


    }
}
