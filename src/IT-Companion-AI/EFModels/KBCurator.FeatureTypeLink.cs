// Project Name: SKAgent
// File Name: KBCurator.FeatureTypeLink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class FeatureTypeLink : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _FeatureId;

    private Guid _Id;

    private string _Role;

    private string _TypeUid;







    public FeatureTypeLink()
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
    public string TypeUid
    {
        get => _TypeUid;
        set
        {
            if (_TypeUid != value)
            {
                OnTypeUidChanging(value);
                SendPropertyChanging("TypeUid");
                _TypeUid = value;
                SendPropertyChanged("TypeUid");
                OnTypeUidChanged();
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
    partial void OnTypeUidChanging(string value);

    partial void OnTypeUidChanged();
    partial void OnRoleChanging(string value);

    partial void OnRoleChanged();

    #endregion
}