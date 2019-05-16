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
    unsafe class GameEntry_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GameEntry);
            args = new Type[]{};
            method = type.GetMethod("get_mNotice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mNotice_0);
            args = new Type[]{};
            method = type.GetMethod("get_mENAnnouncement", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mENAnnouncement_1);
            args = new Type[]{};
            method = type.GetMethod("get_mCNAnnouncement", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mCNAnnouncement_2);

            field = type.GetField("Instance", flag);
            app.RegisterCLRFieldGetter(field, get_Instance_0);
            app.RegisterCLRFieldSetter(field, set_Instance_0);
            field = type.GetField("mHotLogic", flag);
            app.RegisterCLRFieldGetter(field, get_mHotLogic_1);
            app.RegisterCLRFieldSetter(field, set_mHotLogic_1);


        }


        static StackObject* get_mNotice_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::GameEntry.mNotice;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mENAnnouncement_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::GameEntry.mENAnnouncement;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mCNAnnouncement_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::GameEntry.mCNAnnouncement;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_Instance_0(ref object o)
        {
            return global::GameEntry.Instance;
        }
        static void set_Instance_0(ref object o, object v)
        {
            global::GameEntry.Instance = (global::GameEntry)v;
        }
        static object get_mHotLogic_1(ref object o)
        {
            return ((global::GameEntry)o).mHotLogic;
        }
        static void set_mHotLogic_1(ref object o, object v)
        {
            ((global::GameEntry)o).mHotLogic = (global::HotLogic)v;
        }


    }
}
