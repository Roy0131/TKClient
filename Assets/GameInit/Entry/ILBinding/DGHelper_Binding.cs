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
    unsafe class DGHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::DGHelper);
            args = new Type[]{typeof(UnityEngine.UI.Image), typeof(System.Single), typeof(System.Single), typeof(System.Int32)};
            method = type.GetMethod("DoImageFillAmount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoImageFillAmount_0);
            args = new Type[]{typeof(UnityEngine.RectTransform)};
            method = type.GetMethod("DoKill", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoKill_1);
            args = new Type[]{typeof(UnityEngine.UI.Text)};
            method = type.GetMethod("DoKill", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoKill_2);
            args = new Type[]{typeof(UnityEngine.RectTransform), typeof(UnityEngine.Vector2), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoAnchorPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoAnchorPos_3);
            args = new Type[]{typeof(UnityEngine.RectTransform), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoScale", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoScale_4);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoTextFade", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoTextFade_5);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoLocalMoveX", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoLocalMoveX_6);
            args = new Type[]{typeof(UnityEngine.RectTransform), typeof(UnityEngine.Vector3), typeof(System.Single), typeof(System.Int32)};
            method = type.GetMethod("DoScale", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoScale_7);
            args = new Type[]{typeof(UnityEngine.UI.Image)};
            method = type.GetMethod("DoKill", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoKill_8);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoMove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoMove_9);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoLocalMove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoLocalMove_10);
            args = new Type[]{typeof(UnityEngine.RectTransform), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoAnchorPosX", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoAnchorPosX_11);
            args = new Type[]{typeof(UnityEngine.Transform)};
            method = type.GetMethod("DoRotateCircle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoRotateCircle_12);
            args = new Type[]{typeof(UnityEngine.UI.Image), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoImageFade", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoImageFade_13);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(System.Single), typeof(System.Single), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("DoLocalMoveY", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoLocalMoveY_14);


        }


        static StackObject* DoImageFillAmount_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @endValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.UI.Image @target = (UnityEngine.UI.Image)typeof(UnityEngine.UI.Image).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoImageFillAmount(@target, @endValue, @duration, @easeType);

            return __ret;
        }

        static StackObject* DoKill_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.RectTransform @target = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoKill(@target);

            return __ret;
        }

        static StackObject* DoKill_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.Text @target = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoKill(@target);

            return __ret;
        }

        static StackObject* DoAnchorPos_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Vector2 @endValue = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.RectTransform @target = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoAnchorPos(@target, @endValue, @duration, @easeType, @callBack);

            return __ret;
        }

        static StackObject* DoScale_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @endValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.RectTransform @target = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoScale(@target, @endValue, @duration, @easeType, @callBack);

            return __ret;
        }

        static StackObject* DoTextFade_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @endValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.UI.Text @target = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoTextFade(@target, @endValue, @duration, @easeType, @callBack);

            return __ret;
        }

        static StackObject* DoLocalMoveX_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @targetX = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoLocalMoveX(@target, @targetX, @duration, @easeType, @callBack);

            return __ret;
        }

        static StackObject* DoScale_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Vector3 @endValue = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.RectTransform @target = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoScale(@target, @endValue, @duration, @easeType);

            return __ret;
        }

        static StackObject* DoKill_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.Image @target = (UnityEngine.UI.Image)typeof(UnityEngine.UI.Image).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoKill(@target);

            return __ret;
        }

        static StackObject* DoMove_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Vector3 @endValue = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoMove(@target, @endValue, @duration, @easeType, @action);

            return __ret;
        }

        static StackObject* DoLocalMove_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Vector3 @endValue = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoLocalMove(@target, @endValue, @duration, @easeType, @action);

            return __ret;
        }

        static StackObject* DoAnchorPosX_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @endValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.RectTransform @target = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoAnchorPosX(@target, @endValue, @duration, @easeType, @action);

            return __ret;
        }

        static StackObject* DoRotateCircle_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoRotateCircle(@target);

            return __ret;
        }

        static StackObject* DoImageFade_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @endValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.UI.Image @target = (UnityEngine.UI.Image)typeof(UnityEngine.UI.Image).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoImageFade(@target, @endValue, @duration, @easeType, @action);

            return __ret;
        }

        static StackObject* DoLocalMoveY_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @easeType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @targetY = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::DGHelper.DoLocalMoveY(@target, @targetY, @duration, @easeType, @action);

            return __ret;
        }



    }
}
