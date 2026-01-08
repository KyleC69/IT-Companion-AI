// Project Name: SKAgent
// File Name: KBCurator.ApiMemberDiff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class ApiMemberDiff : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private bool? _Breaking;

    private string _ChangeKind;

    private string _DetailJson;

    private Guid _Id;

    private string _MemberUid;

    private string _NewSignature;

    private string _OldSignature;

    private Guid _SnapshotDiffId;







    public ApiMemberDiff()
    {
        OnCreated();
    }







    [NotNullValidator()]
    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                OnIdChanging(value);
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
                OnIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid SnapshotDiffId
    {
        get => _SnapshotDiffId;
        set
        {
            if (_SnapshotDiffId != value)
            {
                OnSnapshotDiffIdChanging(value);
                SendPropertyChanging("SnapshotDiffId");
                _SnapshotDiffId = value;
                SendPropertyChanged("SnapshotDiffId");
                OnSnapshotDiffIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string MemberUid
    {
        get => _MemberUid;
        set
        {
            if (_MemberUid != value)
            {
                OnMemberUidChanging(value);
                SendPropertyChanging("MemberUid");
                _MemberUid = value;
                SendPropertyChanged("MemberUid");
                OnMemberUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string ChangeKind
    {
        get => _ChangeKind;
        set
        {
            if (_ChangeKind != value)
            {
                OnChangeKindChanging(value);
                SendPropertyChanging("ChangeKind");
                _ChangeKind = value;
                SendPropertyChanged("ChangeKind");
                OnChangeKindChanged();
            }
        }
    }

    public string OldSignature
    {
        get => _OldSignature;
        set
        {
            if (_OldSignature != value)
            {
                OnOldSignatureChanging(value);
                SendPropertyChanging("OldSignature");
                _OldSignature = value;
                SendPropertyChanged("OldSignature");
                OnOldSignatureChanged();
            }
        }
    }

    public string NewSignature
    {
        get => _NewSignature;
        set
        {
            if (_NewSignature != value)
            {
                OnNewSignatureChanging(value);
                SendPropertyChanging("NewSignature");
                _NewSignature = value;
                SendPropertyChanged("NewSignature");
                OnNewSignatureChanged();
            }
        }
    }

    public bool? Breaking
    {
        get => _Breaking;
        set
        {
            if (_Breaking != value)
            {
                OnBreakingChanging(value);
                SendPropertyChanging("Breaking");
                _Breaking = value;
                SendPropertyChanged("Breaking");
                OnBreakingChanged();
            }
        }
    }

    public string DetailJson
    {
        get => _DetailJson;
        set
        {
            if (_DetailJson != value)
            {
                OnDetailJsonChanging(value);
                SendPropertyChanging("DetailJson");
                _DetailJson = value;
                SendPropertyChanged("DetailJson");
                OnDetailJsonChanged();
            }
        }
    }

    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual event PropertyChangingEventHandler PropertyChanging;







    protected virtual void SendPropertyChanging()
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, emptyChangingEventArgs);
        }
    }







    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }







    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }







    #region Extensibility Method Definitions

    partial void OnCreated();
    partial void OnIdChanging(Guid value);

    partial void OnIdChanged();
    partial void OnSnapshotDiffIdChanging(Guid value);

    partial void OnSnapshotDiffIdChanged();
    partial void OnMemberUidChanging(string value);

    partial void OnMemberUidChanged();
    partial void OnChangeKindChanging(string value);

    partial void OnChangeKindChanged();
    partial void OnOldSignatureChanging(string value);

    partial void OnOldSignatureChanged();
    partial void OnNewSignatureChanging(string value);

    partial void OnNewSignatureChanged();
    partial void OnBreakingChanging(bool? value);

    partial void OnBreakingChanged();
    partial void OnDetailJsonChanging(string value);

    partial void OnDetailJsonChanged();

    #endregion
}