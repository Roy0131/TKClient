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
    unsafe class TimerHeap_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::TimerHeap);
            args = new Type[]{typeof(System.UInt32)};
            method = type.GetMethod("DelTimer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DelTimer_0);
            args = new Type[]{typeof(System.UInt32), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("AddTimer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddTimer_1);
            args = new Type[]{};
            method = type.GetMethod("Tick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Tick_2);


        }


        static StackObject* DelTimer_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.UInt32 @timerId = (uint)ptr_of_this_method->Value;


            global::TimerHeap.DelTimer(@timerId);

            return __ret;
        }

        static StackObject* AddTimer_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @handler = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @interval = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.UInt32 @start = (uint)ptr_of_this_method->Value;


            var result_of_this_method = global::TimerHeap.AddTimer(@start, @interval, @handler);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = (int)result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Tick_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::TimerHeap.Tick();

            return __ret;
        }



    }
}
