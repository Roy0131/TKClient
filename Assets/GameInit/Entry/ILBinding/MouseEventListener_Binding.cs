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
    unsafe class MouseEventListener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::MouseEventListener);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_0);

            field = type.GetField("mMouseDown", flag);
            app.RegisterCLRFieldGetter(field, get_mMouseDown_0);
            app.RegisterCLRFieldSetter(field, set_mMouseDown_0);
            field = type.GetField("mMouseUp", flag);
            app.RegisterCLRFieldGetter(field, get_mMouseUp_1);
            app.RegisterCLRFieldSetter(field, set_mMouseUp_1);
            field = type.GetField("mMouseClick", flag);
            app.RegisterCLRFieldGetter(field, get_mMouseClick_2);
            app.RegisterCLRFieldSetter(field, set_mMouseClick_2);


        }


        static StackObject* Get_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::MouseEventListener.Get(@go);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_mMouseDown_0(ref object o)
        {
            return ((global::MouseEventListener)o).mMouseDown;
        }
        static void set_mMouseDown_0(ref object o, object v)
        {
            ((global::MouseEventListener)o).mMouseDown = (System.Action<UnityEngine.GameObject>)v;
        }
        static object get_mMouseUp_1(ref object o)
        {
            return ((global::MouseEventListener)o).mMouseUp;
        }
        static void set_mMouseUp_1(ref object o, object v)
        {
            ((global::MouseEventListener)o).mMouseUp = (System.Action<UnityEngine.GameObject>)v;
        }
        static object get_mMouseClick_2(ref object o)
        {
            return ((global::MouseEventListener)o).mMouseClick;
        }
        static void set_mMouseClick_2(ref object o, object v)
        {
            ((global::MouseEventListener)o).mMouseClick = (System.Action<UnityEngine.GameObject>)v;
        }


    }
}
