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
    unsafe class FileConst_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::FileConst);
            args = new Type[]{};
            method = type.GetMethod("get_RunPlatform", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_RunPlatform_0);
            args = new Type[]{};
            method = type.GetMethod("get_CachePath", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CachePath_1);

            field = type.GetField("GAME_PACKAGE_VERSION", flag);
            app.RegisterCLRFieldGetter(field, get_GAME_PACKAGE_VERSION_0);
            app.RegisterCLRFieldSetter(field, set_GAME_PACKAGE_VERSION_0);


        }


        static StackObject* get_RunPlatform_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::FileConst.RunPlatform;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_CachePath_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::FileConst.CachePath;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_GAME_PACKAGE_VERSION_0(ref object o)
        {
            return global::FileConst.GAME_PACKAGE_VERSION;
        }
        static void set_GAME_PACKAGE_VERSION_0(ref object o, object v)
        {
            global::FileConst.GAME_PACKAGE_VERSION = (System.Int32)v;
        }


    }
}
