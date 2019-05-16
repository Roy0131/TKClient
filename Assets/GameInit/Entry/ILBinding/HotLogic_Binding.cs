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
    unsafe class HotLogic_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::HotLogic);

            field = type.GetField("mUpdateAction", flag);
            app.RegisterCLRFieldGetter(field, get_mUpdateAction_0);
            app.RegisterCLRFieldSetter(field, set_mUpdateAction_0);
            field = type.GetField("mLateUpteAction", flag);
            app.RegisterCLRFieldGetter(field, get_mLateUpteAction_1);
            app.RegisterCLRFieldSetter(field, set_mLateUpteAction_1);
            field = type.GetField("mKeyCodeAction", flag);
            app.RegisterCLRFieldGetter(field, get_mKeyCodeAction_2);
            app.RegisterCLRFieldSetter(field, set_mKeyCodeAction_2);
            field = type.GetField("mSoundAction", flag);
            app.RegisterCLRFieldGetter(field, get_mSoundAction_3);
            app.RegisterCLRFieldSetter(field, set_mSoundAction_3);
            field = type.GetField("mAppPauseAction", flag);
            app.RegisterCLRFieldGetter(field, get_mAppPauseAction_4);
            app.RegisterCLRFieldSetter(field, set_mAppPauseAction_4);
            field = type.GetField("mAppQuitAction", flag);
            app.RegisterCLRFieldGetter(field, get_mAppQuitAction_5);
            app.RegisterCLRFieldSetter(field, set_mAppQuitAction_5);
            field = type.GetField("mProvideContentAction", flag);
            app.RegisterCLRFieldGetter(field, get_mProvideContentAction_6);
            app.RegisterCLRFieldSetter(field, set_mProvideContentAction_6);
            field = type.GetField("mAppStoreBuyFailedAction", flag);
            app.RegisterCLRFieldGetter(field, get_mAppStoreBuyFailedAction_7);
            app.RegisterCLRFieldSetter(field, set_mAppStoreBuyFailedAction_7);
            field = type.GetField("mGooglePaySuccessAction", flag);
            app.RegisterCLRFieldGetter(field, get_mGooglePaySuccessAction_8);
            app.RegisterCLRFieldSetter(field, set_mGooglePaySuccessAction_8);
            field = type.GetField("mGooglePayFailedAction", flag);
            app.RegisterCLRFieldGetter(field, get_mGooglePayFailedAction_9);
            app.RegisterCLRFieldSetter(field, set_mGooglePayFailedAction_9);
            field = type.GetField("mFBLoginBackAction", flag);
            app.RegisterCLRFieldGetter(field, get_mFBLoginBackAction_10);
            app.RegisterCLRFieldSetter(field, set_mFBLoginBackAction_10);
            field = type.GetField("mFBLoginCancelAction", flag);
            app.RegisterCLRFieldGetter(field, get_mFBLoginCancelAction_11);
            app.RegisterCLRFieldSetter(field, set_mFBLoginCancelAction_11);
            field = type.GetField("mShopDetailAction", flag);
            app.RegisterCLRFieldGetter(field, get_mShopDetailAction_12);
            app.RegisterCLRFieldSetter(field, set_mShopDetailAction_12);
            field = type.GetField("mFBShareBackAction", flag);
            app.RegisterCLRFieldGetter(field, get_mFBShareBackAction_13);
            app.RegisterCLRFieldSetter(field, set_mFBShareBackAction_13);


        }



        static object get_mUpdateAction_0(ref object o)
        {
            return ((global::HotLogic)o).mUpdateAction;
        }
        static void set_mUpdateAction_0(ref object o, object v)
        {
            ((global::HotLogic)o).mUpdateAction = (System.Action)v;
        }
        static object get_mLateUpteAction_1(ref object o)
        {
            return ((global::HotLogic)o).mLateUpteAction;
        }
        static void set_mLateUpteAction_1(ref object o, object v)
        {
            ((global::HotLogic)o).mLateUpteAction = (System.Action)v;
        }
        static object get_mKeyCodeAction_2(ref object o)
        {
            return ((global::HotLogic)o).mKeyCodeAction;
        }
        static void set_mKeyCodeAction_2(ref object o, object v)
        {
            ((global::HotLogic)o).mKeyCodeAction = (System.Action)v;
        }
        static object get_mSoundAction_3(ref object o)
        {
            return ((global::HotLogic)o).mSoundAction;
        }
        static void set_mSoundAction_3(ref object o, object v)
        {
            ((global::HotLogic)o).mSoundAction = (System.Action)v;
        }
        static object get_mAppPauseAction_4(ref object o)
        {
            return ((global::HotLogic)o).mAppPauseAction;
        }
        static void set_mAppPauseAction_4(ref object o, object v)
        {
            ((global::HotLogic)o).mAppPauseAction = (System.Action<System.Boolean>)v;
        }
        static object get_mAppQuitAction_5(ref object o)
        {
            return ((global::HotLogic)o).mAppQuitAction;
        }
        static void set_mAppQuitAction_5(ref object o, object v)
        {
            ((global::HotLogic)o).mAppQuitAction = (System.Action)v;
        }
        static object get_mProvideContentAction_6(ref object o)
        {
            return ((global::HotLogic)o).mProvideContentAction;
        }
        static void set_mProvideContentAction_6(ref object o, object v)
        {
            ((global::HotLogic)o).mProvideContentAction = (System.Action<System.String>)v;
        }
        static object get_mAppStoreBuyFailedAction_7(ref object o)
        {
            return ((global::HotLogic)o).mAppStoreBuyFailedAction;
        }
        static void set_mAppStoreBuyFailedAction_7(ref object o, object v)
        {
            ((global::HotLogic)o).mAppStoreBuyFailedAction = (System.Action)v;
        }
        static object get_mGooglePaySuccessAction_8(ref object o)
        {
            return ((global::HotLogic)o).mGooglePaySuccessAction;
        }
        static void set_mGooglePaySuccessAction_8(ref object o, object v)
        {
            ((global::HotLogic)o).mGooglePaySuccessAction = (System.Action<System.String>)v;
        }
        static object get_mGooglePayFailedAction_9(ref object o)
        {
            return ((global::HotLogic)o).mGooglePayFailedAction;
        }
        static void set_mGooglePayFailedAction_9(ref object o, object v)
        {
            ((global::HotLogic)o).mGooglePayFailedAction = (System.Action)v;
        }
        static object get_mFBLoginBackAction_10(ref object o)
        {
            return ((global::HotLogic)o).mFBLoginBackAction;
        }
        static void set_mFBLoginBackAction_10(ref object o, object v)
        {
            ((global::HotLogic)o).mFBLoginBackAction = (System.Action<System.String>)v;
        }
        static object get_mFBLoginCancelAction_11(ref object o)
        {
            return ((global::HotLogic)o).mFBLoginCancelAction;
        }
        static void set_mFBLoginCancelAction_11(ref object o, object v)
        {
            ((global::HotLogic)o).mFBLoginCancelAction = (System.Action)v;
        }
        static object get_mShopDetailAction_12(ref object o)
        {
            return ((global::HotLogic)o).mShopDetailAction;
        }
        static void set_mShopDetailAction_12(ref object o, object v)
        {
            ((global::HotLogic)o).mShopDetailAction = (System.Action<System.String>)v;
        }
        static object get_mFBShareBackAction_13(ref object o)
        {
            return ((global::HotLogic)o).mFBShareBackAction;
        }
        static void set_mFBShareBackAction_13(ref object o, object v)
        {
            ((global::HotLogic)o).mFBShareBackAction = (System.Action<System.Int32>)v;
        }


    }
}
