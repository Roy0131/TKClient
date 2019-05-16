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
    unsafe class UnityEngine_UI_ScrollRect_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(UnityEngine.UI.ScrollRect);
            args = new Type[]{};
            method = type.GetMethod("get_content", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_content_0);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("set_horizontalNormalizedPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_horizontalNormalizedPosition_1);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("set_scrollSensitivity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_scrollSensitivity_2);
            args = new Type[]{};
            method = type.GetMethod("get_verticalScrollbar", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_verticalScrollbar_3);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("set_verticalNormalizedPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_verticalNormalizedPosition_4);
            args = new Type[]{};
            method = type.GetMethod("get_onValueChanged", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_onValueChanged_5);


        }


        static StackObject* get_content_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.content;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_horizontalNormalizedPosition_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.horizontalNormalizedPosition = value;

            return __ret;
        }

        static StackObject* set_scrollSensitivity_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.scrollSensitivity = value;

            return __ret;
        }

        static StackObject* get_verticalScrollbar_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.verticalScrollbar;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_verticalNormalizedPosition_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.verticalNormalizedPosition = value;

            return __ret;
        }

        static StackObject* get_onValueChanged_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.ScrollRect instance_of_this_method = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.onValueChanged;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
