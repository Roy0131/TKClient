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
    unsafe class Framework_UI_ObjectHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Framework.UI.ObjectHelper);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Transform), typeof(System.Boolean)};
            method = type.GetMethod("AddChildToParent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddChildToParent_0);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(System.Int32)};
            method = type.GetMethod("SetObjectLayer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetObjectLayer_1);
            args = new Type[]{typeof(UnityEngine.UI.Image), typeof(UnityEngine.Sprite)};
            method = type.GetMethod("SetSprite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetSprite_2);
            args = new Type[]{typeof(UnityEngine.Transform)};
            method = type.GetMethod("PopAnimationLiner", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PopAnimationLiner_3);
            args = new Type[]{typeof(UnityEngine.UI.Button), typeof(System.Boolean)};
            method = type.GetMethod("SetEnableStatus", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetEnableStatus_4);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(System.Single)};
            method = type.GetMethod("AnimationMove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnimationMove_5);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(Framework.UI.ObjectHelper.direction)};
            method = type.GetMethod("AnimationMoveBack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnimationMoveBack_6);
            args = new Type[]{typeof(UnityEngine.Transform)};
            method = type.GetMethod("PopAnimationBack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PopAnimationBack_7);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(Framework.UI.ObjectHelper.direction)};
            method = type.GetMethod("AnimationMoveBounce", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnimationMoveBounce_8);
            args = new Type[]{typeof(UnityEngine.Transform), typeof(Framework.UI.ObjectHelper.direction)};
            method = type.GetMethod("AnimationMoveLiner", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AnimationMoveLiner_9);


        }


        static StackObject* AddChildToParent_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @blResetScale = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @parent = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Transform @child = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.AddChildToParent(@child, @parent, @blResetScale);

            return __ret;
        }

        static StackObject* SetObjectLayer_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @layer = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @transform = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.SetObjectLayer(@transform, @layer);

            return __ret;
        }

        static StackObject* SetSprite_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Sprite @sprite = (UnityEngine.Sprite)typeof(UnityEngine.Sprite).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Image @image = (UnityEngine.UI.Image)typeof(UnityEngine.UI.Image).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.SetSprite(@image, @sprite);

            return __ret;
        }

        static StackObject* PopAnimationLiner_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.PopAnimationLiner(@transObj);

            return __ret;
        }

        static StackObject* SetEnableStatus_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @enable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Button @btn = (UnityEngine.UI.Button)typeof(UnityEngine.UI.Button).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.SetEnableStatus(@btn, @enable);

            return __ret;
        }

        static StackObject* AnimationMove_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @vecDes = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Vector3 @vecOri = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.AnimationMove(@transObj, @vecOri, @vecDes, @time);

            return __ret;
        }

        static StackObject* AnimationMoveBack_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Framework.UI.ObjectHelper.direction @direction = (Framework.UI.ObjectHelper.direction)typeof(Framework.UI.ObjectHelper.direction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.AnimationMoveBack(@transObj, @direction);

            return __ret;
        }

        static StackObject* PopAnimationBack_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.PopAnimationBack(@transObj);

            return __ret;
        }

        static StackObject* AnimationMoveBounce_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Framework.UI.ObjectHelper.direction @direction = (Framework.UI.ObjectHelper.direction)typeof(Framework.UI.ObjectHelper.direction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.AnimationMoveBounce(@transObj, @direction);

            return __ret;
        }

        static StackObject* AnimationMoveLiner_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Framework.UI.ObjectHelper.direction @direction = (Framework.UI.ObjectHelper.direction)typeof(Framework.UI.ObjectHelper.direction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @transObj = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Framework.UI.ObjectHelper.AnimationMoveLiner(@transObj, @direction);

            return __ret;
        }



    }
}
