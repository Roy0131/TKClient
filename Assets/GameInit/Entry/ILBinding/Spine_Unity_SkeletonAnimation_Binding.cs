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
    unsafe class Spine_Unity_SkeletonAnimation_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Spine.Unity.SkeletonAnimation);
            args = new Type[]{};
            method = type.GetMethod("get_AnimationName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AnimationName_0);
            args = new Type[]{};
            method = type.GetMethod("get_AnimationState", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AnimationState_1);

            field = type.GetField("state", flag);
            app.RegisterCLRFieldGetter(field, get_state_0);
            app.RegisterCLRFieldSetter(field, set_state_0);
            field = type.GetField("timeScale", flag);
            app.RegisterCLRFieldGetter(field, get_timeScale_1);
            app.RegisterCLRFieldSetter(field, set_timeScale_1);


        }


        static StackObject* get_AnimationName_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Spine.Unity.SkeletonAnimation instance_of_this_method = (Spine.Unity.SkeletonAnimation)typeof(Spine.Unity.SkeletonAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AnimationName;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_AnimationState_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Spine.Unity.SkeletonAnimation instance_of_this_method = (Spine.Unity.SkeletonAnimation)typeof(Spine.Unity.SkeletonAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AnimationState;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_state_0(ref object o)
        {
            return ((Spine.Unity.SkeletonAnimation)o).state;
        }
        static void set_state_0(ref object o, object v)
        {
            ((Spine.Unity.SkeletonAnimation)o).state = (Spine.AnimationState)v;
        }
        static object get_timeScale_1(ref object o)
        {
            return ((Spine.Unity.SkeletonAnimation)o).timeScale;
        }
        static void set_timeScale_1(ref object o, object v)
        {
            ((Spine.Unity.SkeletonAnimation)o).timeScale = (System.Single)v;
        }


    }
}
