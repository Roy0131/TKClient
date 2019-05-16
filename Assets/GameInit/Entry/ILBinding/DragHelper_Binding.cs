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
    unsafe class DragHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::DragHelper);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_mRoleId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_mRoleId_0);
            args = new Type[]{};
            method = type.GetMethod("get_mRoleId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mRoleId_1);

            field = type.GetField("mDragMethod", flag);
            app.RegisterCLRFieldGetter(field, get_mDragMethod_0);
            app.RegisterCLRFieldSetter(field, set_mDragMethod_0);


        }


        static StackObject* set_mRoleId_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::DragHelper instance_of_this_method = (global::DragHelper)typeof(global::DragHelper).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.mRoleId = value;

            return __ret;
        }

        static StackObject* get_mRoleId_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::DragHelper instance_of_this_method = (global::DragHelper)typeof(global::DragHelper).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mRoleId;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_mDragMethod_0(ref object o)
        {
            return ((global::DragHelper)o).mDragMethod;
        }
        static void set_mDragMethod_0(ref object o, object v)
        {
            ((global::DragHelper)o).mDragMethod = (System.Action<global::DragEventType, UnityEngine.EventSystems.PointerEventData, global::DragHelper>)v;
        }


    }
}
