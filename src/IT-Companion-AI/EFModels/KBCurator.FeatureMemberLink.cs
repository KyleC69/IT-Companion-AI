// Project Name: SKAgent
// File Name: KBCurator.FeatureMemberLink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class FeatureMemberLink : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _FeatureId;

    private Guid _Id;

    private string _MemberUid;

    private string _Role;







    public FeatureMemberLink()
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
    public Guid FeatureId
    {
        get => _FeatureId;
        set
        {
            if (_FeatureId != value)
            {
                OnFeatureIdChanging(value);
                SendPropertyChanging("FeatureId");
                _FeatureId = value;
                SendPropertyChanged("FeatureId");
                OnFeatureIdChanged();
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

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Role
    {
        get => _Role;
        set
        {
            if (_Role != value)
            {
                OnRoleChanging(value);
                SendPropertyChanging("Role");
                _Role = value;
                SendPropertyChanged("Role");
                OnRoleChanged();
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
    partial void OnFeatureIdChanging(Guid value);

    partial void OnFeatureIdChanged();
    partial void OnMemberUidChanging(string value);

    partial void OnMemberUidChanged();
    partial void OnRoleChanging(string value);

    partial void OnRoleChanged();

    #endregion
}