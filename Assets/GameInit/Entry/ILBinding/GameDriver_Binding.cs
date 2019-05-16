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
    unsafe class GameDriver_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GameDriver);
            args = new Type[]{};
            method = type.GetMethod("get_mPluginDispather", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mPluginDispather_0);
            args = new Type[]{};
            method = type.GetMethod("get_mBlRunHot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mBlRunHot_1);
            args = new Type[]{};
            method = type.GetMethod("get_mInitViewObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mInitViewObject_2);

            field = type.GetField("Instance", flag);
            app.RegisterCLRFieldGetter(field, get_Instance_0);
            app.RegisterCLRFieldSetter(field, set_Instance_0);
            field = type.GetField("mShowDebug", flag);
            app.RegisterCLRFieldGetter(field, get_mShowDebug_1);
            app.RegisterCLRFieldSetter(field, set_mShowDebug_1);
            field = type.GetField("ISIPHONEX", flag);
            app.RegisterCLRFieldGetter(field, get_ISIPHONEX_2);
            app.RegisterCLRFieldSetter(field, set_ISIPHONEX_2);
            field = type.GetField("UseAssetBundle", flag);
            app.RegisterCLRFieldGetter(field, get_UseAssetBundle_3);
            app.RegisterCLRFieldSetter(field, set_UseAssetBundle_3);
            field = type.GetField("m_serverName", flag);
            app.RegisterCLRFieldGetter(field, get_m_serverName_4);
            app.RegisterCLRFieldSetter(field, set_m_serverName_4);
            field = type.GetField("mInitObject", flag);
            app.RegisterCLRFieldGetter(field, get_mInitObject_5);
            app.RegisterCLRFieldSetter(field, set_mInitObject_5);
            field = type.GetField("mLoginObject", flag);
            app.RegisterCLRFieldGetter(field, get_mLoginObject_6);
            app.RegisterCLRFieldSetter(field, set_mLoginObject_6);
            field = type.GetField("mShowGuide", flag);
            app.RegisterCLRFieldGetter(field, get_mShowGuide_7);
            app.RegisterCLRFieldSetter(field, set_mShowGuide_7);


        }


        static StackObject* get_mPluginDispather_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GameDriver instance_of_this_method = (global::GameDriver)typeof(global::GameDriver).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mPluginDispather;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mBlRunHot_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GameDriver instance_of_this_method = (global::GameDriver)typeof(global::GameDriver).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mBlRunHot;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_mInitViewObject_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GameDriver instance_of_this_method = (global::GameDriver)typeof(global::GameDriver).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mInitViewObject;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_Instance_0(ref object o)
        {
            return global::GameDriver.Instance;
        }
        static void set_Instance_0(ref object o, object v)
        {
            global::GameDriver.Instance = (global::GameDriver)v;
        }
        static object get_mShowDebug_1(ref object o)
        {
            return ((global::GameDriver)o).mShowDebug;
        }
        static void set_mShowDebug_1(ref object o, object v)
        {
            ((global::GameDriver)o).mShowDebug = (System.Boolean)v;
        }
        static object get_ISIPHONEX_2(ref object o)
        {
            return global::GameDriver.ISIPHONEX;
        }
        static void set_ISIPHONEX_2(ref object o, object v)
        {
            global::GameDriver.ISIPHONEX = (System.Boolean)v;
        }
        static object get_UseAssetBundle_3(ref object o)
        {
            return ((global::GameDriver)o).UseAssetBundle;
        }
        static void set_UseAssetBundle_3(ref object o, object v)
        {
            ((global::GameDriver)o).UseAssetBundle = (System.Boolean)v;
        }
        static object get_m_serverName_4(ref object o)
        {
            return ((global::GameDriver)o).m_serverName;
        }
        static void set_m_serverName_4(ref object o, object v)
        {
            ((global::GameDriver)o).m_serverName = (global::ServerNameEnum)v;
        }
        static object get_mInitObject_5(ref object o)
        {
            return ((global::GameDriver)o).mInitObject;
        }
        static void set_mInitObject_5(ref object o, object v)
        {
            ((global::GameDriver)o).mInitObject = (UnityEngine.GameObject)v;
        }
        static object get_mLoginObject_6(ref object o)
        {
            return ((global::GameDriver)o).mLoginObject;
        }
        static void set_mLoginObject_6(ref object o, object v)
        {
            ((global::GameDriver)o).mLoginObject = (UnityEngine.GameObject)v;
        }
        static object get_mShowGuide_7(ref object o)
        {
            return ((global::GameDriver)o).mShowGuide;
        }
        static void set_mShowGuide_7(ref object o, object v)
        {
            ((global::GameDriver)o).mShowGuide = (System.Boolean)v;
        }


    }
}
