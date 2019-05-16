using LitJson;
using System.Collections.Generic;

namespace IHLogic
{
    public class NativeLogicInterface :Singleton<NativeLogicInterface>
    {
        #region recharge interface logic

        public void ReSendOrder()
        {
            Dictionary<int, OrderCacheData>.ValueCollection valColl = LocalDataMgr.mDictOrderDatas.Values;
            foreach (OrderCacheData orderData in valColl)
                GameNetMgr.Instance.mGameServer.ReqCharge(orderData.mOrderChannel, orderData.mBundleId, orderData.mPayLoad, orderData.mOrderData, orderData.mCacheOrderID);
        }

        #region shop item price
        public void ReqPayPrice()
        {
            string detailValue = "";
            Dictionary<int, PayConfig> dict = PayConfig.Get();
            foreach (var kv in dict)
            {
                if (string.IsNullOrEmpty(detailValue))
                    detailValue = kv.Value.BundleID;
                else
                    detailValue += ("," + kv.Value.BundleID);
            }
            if (string.IsNullOrEmpty(detailValue))
                return;
            LogHelper.Log("[GameNative.GetPayDetail() => payvalue:" + detailValue + "]");
            GameNative.Instance.ReqPayPrice(detailValue);
        }

        public string GetShowPrice(string bundleID)
        {
            if (_dictSkuDetail.ContainsKey(bundleID))
            {
                SkuDetail detail = _dictSkuDetail[bundleID];
                if(FileConst.RunPlatform == "ios")
                {
                    if (!string.IsNullOrEmpty(detail.mPriceCurCode))
                        return detail.mShowPrice + " " + detail.mPrice;
                }
                else
                {
                    return _dictSkuDetail[bundleID].mShowPrice;
                }
            }
            else
            {
                PayConfig config = GameConfigMgr.Instance.GetStrPayConfig(bundleID);
                if (config != null)
                    return "US$" + config.RecordGold;
            }
            return "";
        }

        private static string _bundlID = null;
        private static bool _blPaying = false;
        private static string _orderID = "";
        public void Pay(string bundleId)
        {
            LoadingMgr.Instance.ShowRechargeMask();
            if (_blPaying)
                return;
            _bundlID = bundleId;
            _orderID = null;
            _blPaying = true;
            GameNative.Instance.Pay(_bundlID);
        }

        private static JsonData _detailJson = null;
        struct SkuDetail
        {
            public string mBundleID { get; set; }
            public float mPrice { get; set; }
            public string mShowPrice { get; set; }
            public string mPriceCurCode { get; set; }
        }

        private static Dictionary<string, SkuDetail> _dictSkuDetail = new Dictionary<string, SkuDetail>();
        public static void OnShopDetailResult(string detailValues)
        {
            if (string.IsNullOrEmpty(detailValues))
                return;
            //LogHelper.Log("[GameRechargeMgr.OnShopDetailResult() => detail:" + detailValues + "]");
            string[] tmp = detailValues.Split('|');
            string[] skuValue;
            SkuDetail detail;
            for (int i = 0; i < tmp.Length; i++)
            {
                //LogHelper.Log("[GameRechargeMgr.OnShopDetailResult() => index:" + i + ", value:" + tmp[i] + "]");
                skuValue = tmp[i].Split('_');
                //LogHelper.LogWarning("skuValue.count:" + skuValue.Length);
                if (skuValue.Length < 2)
                    continue;
                detail = new SkuDetail();
                detail.mBundleID = skuValue[0];
                detail.mPrice = float.Parse(skuValue[1]);
                if (skuValue.Length > 2)
                    detail.mShowPrice = skuValue[2];
                if (skuValue.Length > 3)
                    detail.mPriceCurCode = skuValue[3];
                if (_dictSkuDetail.ContainsKey(detail.mBundleID))
                    continue;
                //LogHelper.Log("onShopDetailResult() => bundleid:" + detail.mBundleID + ", price:" + detail.mPrice + ", price show:" + detail.mShowPrice + ", code:" + detail.mPriceCurCode);
                _dictSkuDetail.Add(detail.mBundleID, detail);
            }
        }
        
        public float GetPrice(string bundleId)
        {
            if (_dictSkuDetail.ContainsKey(bundleId))
                return _dictSkuDetail[bundleId].mPrice;
            return 0f;
        }
        #endregion

        #region ios recharge result
        public static void ProvideContent(string value)
        {
            LogHelper.LogWarning("[GameNative.ProvideContent() => 获取商品回执：" + value + "]");
            _blPaying = false;
            int idx = LocalDataMgr.AddOrderInfo(value, "", 2, _bundlID);
            GameNetMgr.Instance.mGameServer.ReqCharge(2, _bundlID, value, "", idx);
            LoadingMgr.Instance.HideRechargeMask();
            if (LocalDataMgr.LoginChannel == GameLoginType.FACEBOOK && _dictSkuDetail != null)
            {
                if (!_dictSkuDetail.ContainsKey(_bundlID))
                    return;
                SkuDetail detail = _dictSkuDetail[_bundlID];
                string evtLog = detail.mPrice + "," + _bundlID + "," + detail.mPriceCurCode;
                GameNative.Instance.OnFaceBookEvent(evtLog);
            }
            _bundlID = "";
            //_cacheIdx++;
        }

        public static void AppStoreBuyFailed()
        {
            _bundlID = "";
            _blPaying = false;
            PopupTipsMgr.Instance.ShowTips("Recharge Canceled!!");
            LoadingMgr.Instance.HideRechargeMask();
        }
        #endregion

        public void RechargeBack(int clientIdx)
        {
            LocalDataMgr.RemoveOrderByKey(clientIdx);
        }

        #region google recharge result
        public static void GooglePaySuccess(string payData)
        {
            _blPaying = false;
            JsonData json = JsonMapper.ToObject(payData);
            string payload = json["Payload"].ToString();
            string signature = json["Signature"].ToString();
            LogHelper.Log("[GameNative.GooglePaySuccess() => pay load:" + payload + ", signature:" + signature + "]");
            //_dictPayLoad.Add(_cacheIdx, signature);
            int idx = LocalDataMgr.AddOrderInfo(payload, signature, 1, _bundlID);
            GameNetMgr.Instance.mGameServer.ReqCharge(1, _bundlID, payload, signature, idx);
            //_cacheIdx++;

            PopupTipsMgr.Instance.ShowTips("Recharge Success!!!");
            LoadingMgr.Instance.HideRechargeMask();
            if (LocalDataMgr.LoginChannel == GameLoginType.FACEBOOK && _dictSkuDetail != null)
            {
                if (!_dictSkuDetail.ContainsKey(_bundlID))
                    return;
                SkuDetail detail = _dictSkuDetail[_bundlID];
                JsonData jd = new JsonData();
                jd["Price"] = detail.mPrice;
                jd["BundleID"] = _bundlID;
                jd["Currency"] = detail.mPriceCurCode;
                GameNative.Instance.OnFaceBookEvent(jd.ToJson());
            }
            _bundlID = "";
        }

        public static void GooglePayFailed()
        {
            _blPaying = false;
            //LogHelper.Log("[GameNative.GooglePaySuccess("]");
            _bundlID = "";
            _orderID = null;
            PopupTipsMgr.Instance.ShowTips("Recharge Canceled!!!");
            LoadingMgr.Instance.HideRechargeMask();
        }
        #endregion

        #endregion

        #region facebook interface logic

        public void DoFacebookLogin()
        {
            GameNative.Instance.DoFBLogin();
        }

        public static void OnFBShareBack(int code)
        {
            if (code == 0)
            {
                PopupTipsMgr.Instance.ShowTips("分享成功");
                CarnivalDataModel.Instance.OnCarnivaleFinished(CarnivalConst.Share);
            }
            else
            {
                //PopupTipsMgr.Instance.ShowTips("分享失败, code:" + code);
            }
        }
        
        public static void OnFacebookLoginBack(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LogHelper.LogWarning("[GameNative.OnFBLoginBack() => fb sdk login back, but result was invalid!!]");
                return;
            }
            JsonData jsonData = JsonMapper.ToObject(value);
            string uid = jsonData["uid"].ToString();
            string token = jsonData["token"].ToString();
            GameLoginMgr.Instance.FaceBookLoginBack(uid, token);
        }

        public static void OnFacebookLoginCancel()
        {
            GameLoginMgr.Instance.FaceBookLoginCancel();
        }

        public void OnFcmMsgToken(string value)
        {

        }

        #endregion
    }


}
