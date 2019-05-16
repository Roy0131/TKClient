using ILRuntime.Runtime.Enviorment;
public class ILHelper
{

    public static void InitILRunTime(AppDomain appDomain)
    {
        BindingAdaptor(appDomain);
        InitMethodConvertor(appDomain);
        
        appDomain.DelegateManager.RegisterFunctionDelegate<Adapt_IMessage.Adaptor>();
        appDomain.DelegateManager.RegisterMethodDelegate<Adapt_IMessage.Adaptor>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
        appDomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.IMessage>();
        appDomain.DelegateManager.RegisterMethodDelegate<LocationText>();
        appDomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<System.Int32>>();        appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Boolean, UnityEngine.RectTransform>();        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32>();        appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();        appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();        appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<Adapt_IMessage.Adaptor>>();        appDomain.DelegateManager.RegisterMethodDelegate<Spine.TrackEntry>();        appDomain.DelegateManager.RegisterFunctionDelegate<Adapt_IMessage.Adaptor, Adapt_IMessage.Adaptor, System.Int32>();        appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Collections.Generic.List<System.Int32>, System.Boolean>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Boolean>();        appDomain.DelegateManager.RegisterMethodDelegate<System.String>();        appDomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Int32, System.Char, System.Char>();        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.Collections.Generic.IList<Adapt_IMessage.Adaptor>>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.IList<global::Adapt_IMessage.Adaptor>>();
        appDomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.Collections.Generic.List<global::Adapt_IMessage.Adaptor>>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.Boolean>();        appDomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.IList<System.Int32>, System.Collections.Generic.List<global::Adapt_IMessage.Adaptor>>();
        appDomain.DelegateManager.RegisterMethodDelegate<global::DragEventType, UnityEngine.EventSystems.PointerEventData, global::DragHelper>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<global::Adapt_IMessage.Adaptor>, System.Int32, System.Int32>();
        appDomain.DelegateManager.RegisterFunctionDelegate<global::Adapt_IMessage.Adaptor, System.Boolean>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.String>();
        appDomain.DelegateManager.RegisterMethodDelegate<global::RAssetBundle>();

        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
    }

    private static void BindingAdaptor(AppDomain appDomain)
    {
        appDomain.RegisterCrossBindingAdaptor(new Adapt_IMessage());

    }

    private static void InitMethodConvertor(AppDomain appDomain)
    {

        appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
            {
                return ((System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
            });
        });

        appDomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
        {
            return new System.AsyncCallback((ar) =>
            {
                ((System.Action<System.IAsyncResult>)act)(ar);
            });
        });        appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
        {
            return new System.Comparison<System.Int32>((x, y) =>
            {
                return ((System.Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
            });
        });        appDomain.DelegateManager.RegisterDelegateConvertor<Spine.AnimationState.TrackEntryDelegate>((act) =>
        {
            return new Spine.AnimationState.TrackEntryDelegate((trackEntry) =>
            {
                ((System.Action<Spine.TrackEntry>)act)(trackEntry);
            });
        });        appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
            {
                ((System.Action<System.Int32>)act)(arg0);
            });
        });

        appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<global::Adapt_IMessage.Adaptor>>((act) =>
        {
            return new System.Comparison<global::Adapt_IMessage.Adaptor>((x, y) =>
            {
                return ((System.Func<global::Adapt_IMessage.Adaptor, global::Adapt_IMessage.Adaptor, System.Int32>)act)(x, y);
            });
        });

        appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.UI.InputField.OnValidateInput>((act) =>
        {
            return new UnityEngine.UI.InputField.OnValidateInput((text, charIndex, addedChar) =>
            {
                return ((System.Func<System.String, System.Int32, System.Char, System.Char>)act)(text, charIndex, addedChar);
            });
        });        appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
            {
                ((System.Action<UnityEngine.Vector2>)act)(arg0);
            });
        });

        appDomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<global::Adapt_IMessage.Adaptor>>((act) =>
        {
            return new System.Predicate<global::Adapt_IMessage.Adaptor>((obj) =>
            {
                return ((System.Func<global::Adapt_IMessage.Adaptor, System.Boolean>)act)(obj);
            });
        });
    }
}
