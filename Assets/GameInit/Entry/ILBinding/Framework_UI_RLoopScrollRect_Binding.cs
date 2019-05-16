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
    unsafe class Framework_UI_RLoopScrollRect_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Framework.UI.RLoopScrollRect);
            args = new Type[]{};
            method = type.GetMethod("ClearCells", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearCells_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RefillCells", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefillCells_1);
            args = new Type[]{};
            method = type.GetMethod("get_content", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_content_2);
            args = new Type[]{typeof(UnityEngine.EventSystems.PointerEventData)};
            method = type.GetMethod("OnBeginDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnBeginDrag_3);
            args = new Type[]{typeof(UnityEngine.EventSystems.PointerEventData)};
            method = type.GetMethod("OnDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnDrag_4);
            args = new Type[]{typeof(UnityEngine.EventSystems.PointerEventData)};
            method = type.GetMethod("OnEndDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnEndDrag_5);
            args = new Type[]{typeof(System.Func<System.Int32, System.Boolean, UnityEngine.RectTransform>)};
            method = type.GetMethod("set_mCreateNewFunc", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_mCreateNewFunc_6);
            args = new Type[]{typeof(System.Action<System.Int32>)};
            method = type.GetMethod("set_mReturnAct", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_mReturnAct_7);

            field = type.GetField("totalCount", flag);
            app.RegisterCLRFieldGetter(field, get_totalCount_0);
            app.RegisterCLRFieldSetter(field, set_totalCount_0);


        }


        static StackObject* ClearCells_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ClearCells();

            return __ret;
        }

        static StackObject* RefillCells_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @offset = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefillCells(@offset);

            return __ret;
        }

        static StackObject* get_content_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.content;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* OnBeginDrag_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.EventSystems.PointerEventData @eventData = (UnityEngine.EventSystems.PointerEventData)typeof(UnityEngine.EventSystems.PointerEventData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnBeginDrag(@eventData);

            return __ret;
        }

        static StackObject* OnDrag_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.EventSystems.PointerEventData @eventData = (UnityEngine.EventSystems.PointerEventData)typeof(UnityEngine.EventSystems.PointerEventData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnDrag(@eventData);

            return __ret;
        }

        static StackObject* OnEndDrag_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.EventSystems.PointerEventData @eventData = (UnityEngine.EventSystems.PointerEventData)typeof(UnityEngine.EventSystems.PointerEventData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnEndDrag(@eventData);

            return __ret;
        }

        static StackObject* set_mCreateNewFunc_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Func<System.Int32, System.Boolean, UnityEngine.RectTransform> @value = (System.Func<System.Int32, System.Boolean, UnityEngine.RectTransform>)typeof(System.Func<System.Int32, System.Boolean, UnityEngine.RectTransform>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.mCreateNewFunc = value;

            return __ret;
        }

        static StackObject* set_mReturnAct_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32> @value = (System.Action<System.Int32>)typeof(System.Action<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Framework.UI.RLoopScrollRect instance_of_this_method = (Framework.UI.RLoopScrollRect)typeof(Framework.UI.RLoopScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.mReturnAct = value;

            return __ret;
        }


        static object get_totalCount_0(ref object o)
        {
            return ((Framework.UI.RLoopScrollRect)o).totalCount;
        }
        static void set_totalCount_0(ref object o, object v)
        {
            ((Framework.UI.RLoopScrollRect)o).totalCount = (System.Int32)v;
        }


    }
}
