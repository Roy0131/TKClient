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
    unsafe class UIEventExtend_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::UIEventExtend);
            args = new Type[]{typeof(UnityEngine.UI.Button.ButtonClickedEvent), typeof(System.Action)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_0);
            args = new Type[]{typeof(UnityEngine.UI.Toggle.ToggleEvent), typeof(System.Action<System.Boolean>)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_1);
            args = new Type[]{typeof(UnityEngine.UI.InputField.OnChangeEvent), typeof(System.Action<System.String>)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_2);
            args = new Type[]{typeof(UnityEngine.UI.InputField.SubmitEvent), typeof(System.Action<System.String>)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_3);


        }


        static StackObject* Add_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Button.ButtonClickedEvent @buttonClickedEvent = (UnityEngine.UI.Button.ButtonClickedEvent)typeof(UnityEngine.UI.Button.ButtonClickedEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::UIEventExtend.Add(@buttonClickedEvent, @action);

            return __ret;
        }

        static StackObject* Add_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @action = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Toggle.ToggleEvent @toggleEvent = (UnityEngine.UI.Toggle.ToggleEvent)typeof(UnityEngine.UI.Toggle.ToggleEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::UIEventExtend.Add(@toggleEvent, @action);

            return __ret;
        }

        static StackObject* Add_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.String> @action = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.InputField.OnChangeEvent @onChangeEvent = (UnityEngine.UI.InputField.OnChangeEvent)typeof(UnityEngine.UI.InputField.OnChangeEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::UIEventExtend.Add(@onChangeEvent, @action);

            return __ret;
        }

        static StackObject* Add_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.String> @action = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.InputField.SubmitEvent @submitEvent = (UnityEngine.UI.InputField.SubmitEvent)typeof(UnityEngine.UI.InputField.SubmitEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::UIEventExtend.Add(@submitEvent, @action);

            return __ret;
        }



    }
}
