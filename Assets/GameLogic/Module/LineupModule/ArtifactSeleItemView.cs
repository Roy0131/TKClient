using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSeleItemView : UIBaseView
{
    private ArtifactDataVO _artifactDataVO;
    private ImageGray _imageGray1;
    private ImageGray _imageGray2;
    private ImageGray _imageGray3;
    private GameObject _selectObj;
    private Button _selectBtn;
    private Text _name;
    private Image _itemIcon;
    private Image _bjImg;
    private GameObject _kuang1;
    private GameObject _kuang2;
    private GameObject _kuang3;

    private GameObject _shenqi1;
    private UIEffectView _effect1;

    private GameObject _shenqi2;
    private UIEffectView _effect2;

    private bool _blSelected = false;
    private Material _grayMat = null;

    public RectTransform mScrollRect;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _imageGray1 = Find<ImageGray>("BJ");
        _imageGray2 = Find<ImageGray>("Obj");
        _imageGray3 = Find<ImageGray>("Btn");
        _name = Find<Text>("Btn/Text");
        _selectBtn = Find<Button>("Btn");
        _selectObj = Find("Img");
        _itemIcon = Find<Image>("Obj");
        _bjImg = Find<Image>("BJ");

        _kuang1 = Find("Kuang/Img1");
        _kuang2 = Find("Kuang/Img2");
        _kuang3 = Find("Kuang/Img3");
        _shenqi1 = Find("icon_shenqi_1");
        _effect1 = CreateUIEffect(_shenqi1, UILayerSort.PopupSortBeginner + 4);
        _shenqi2 = Find("icon_shenqi_2");
        _effect2 = CreateUIEffect(_shenqi2, UILayerSort.PopupSortBeginner + 2);
        CreateFixedEffect(_itemIcon.gameObject, UILayerSort.PopupSortBeginner + 3);
        CreateFixedEffect(_selectObj, UILayerSort.PopupSortBeginner + 5);

        m_canvasScale = GameUIMgr.Instance.UICanvas.transform.localScale.x;

        EffectClip ec = Find<EffectClip>("icon_shenqi_1");
        AdjustClipArena(ec);
        ec = Find<EffectClip>("icon_shenqi_2"); 
        AdjustClipArena(ec);

        _selectBtn.onClick.Add(OnSelect);
    }

    float m_halfWidth, m_halfHeight, m_canvasScale;
    private void AdjustClipArena(EffectClip ec)
    {
        m_halfWidth = mScrollRect.sizeDelta.x * 0.5f * m_canvasScale;
        m_halfHeight = mScrollRect.sizeDelta.y * 0.5f * m_canvasScale;

        //给shader的容器坐标变量_Area赋值
        Vector4 area = CalculateArea(mScrollRect.position);

        var particleSystems = ec.GetComponentsInChildren<ParticleSystem>();
        List<Material> lstMat = new List<Material>();
        for (int i = 0, j = particleSystems.Length; i < j; i++)
        {
            var ps = particleSystems[i];
            var mat = ps.GetComponent<Renderer>().material;
            mat.SetVector("_Area", area);
        }
    }

    //计算容器在世界坐标的Vector4，xz为左右边界的值，yw为下上边界值
    Vector4 CalculateArea(Vector3 position)
    {
        return new Vector4()
        {
            x = position.x - m_halfWidth,
            y = position.y - m_halfHeight,
            z = position.x + m_halfWidth,
            w = position.y + m_halfHeight
        };
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _artifactDataVO = args[0] as ArtifactDataVO;
        _selectObj.SetActive(false);
        if (_artifactDataVO.mArtifactData.Rank == 1)
        {
            _effect1.StopEffect();
            _effect2.StopEffect();
        }
        else if (_artifactDataVO.mArtifactData.Rank == 2)
        {
            _effect1.PlayEffect();
            _effect2.StopEffect();
        }
        else
        {
            _effect1.PlayEffect();
            _effect2.PlayEffect();
        }
        _kuang1.SetActive(_artifactDataVO.mArtifactData.Rank == 1);
        _kuang2.SetActive(_artifactDataVO.mArtifactData.Rank == 2);
        _kuang3.SetActive(_artifactDataVO.mArtifactData.Rank > 2);
        _itemIcon.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactIcon);
        _bjImg.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactBjIcon);
        if (_artifactDataVO.mArtifactData.Level == 0)
        {
            _name.text = LanguageMgr.GetLanguage(400011);
            _imageGray1.SetGray();
            //SetRawImageGray();
            _imageGray2.SetGray();
            _imageGray3.SetGray();
        }
        else if (BlSelected)
        {
            _name.text = LanguageMgr.GetLanguage(6001271);
            _imageGray1.SetNormal();
            //SetRawImageNormal();
            _imageGray2.SetNormal();
            _imageGray3.SetNormal();
        }
        else
        {
            _name.text = LanguageMgr.GetLanguage(210122);
            _imageGray1.SetNormal();
            //SetRawImageNormal();
            _imageGray2.SetNormal();
            _imageGray3.SetNormal();
        }
    }

    private void SetRawImageGray()
    {
        if(_grayMat == null)
            _grayMat = Resources.Load("mat/Gray", typeof(Material)) as Material;
        _itemIcon.material = _grayMat;
    }

    private void SetRawImageNormal()
    {
        _itemIcon.material = null;
    }

    private void OnSelect()
    {
        if (_artifactDataVO.mArtifactData.Level == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(400010));
        }
        else
        {
            if (!_blSelected)
            {
                LocalDataMgr.AddArtifactSelect(LineupSceneMgr.Instance.mLineupTeamType, _artifactDataVO.mArtifactData.Id);
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArtifactEvent.ArtifactSelect, this);
            }
            else
            {
                LocalDataMgr.AddArtifactSelect(LineupSceneMgr.Instance.mLineupTeamType, 0);
                BlSelected = false;
            }
        }
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _selectObj.SetActive(_blSelected);
            if (_blSelected)
                _name.text = LanguageMgr.GetLanguage(6001271);
            else
                _name.text = LanguageMgr.GetLanguage(210122);
        }
    }
}
