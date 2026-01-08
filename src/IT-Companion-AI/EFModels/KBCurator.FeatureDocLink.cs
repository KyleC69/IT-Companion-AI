// Project Name: SKAgent
// File Name: KBCurator.FeatureDocLink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.Entities;


public partial class FeatureDocLink : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _DocUid;

    private Guid _FeatureId;

    private Guid _Id;

    private string _SectionUid;







    public FeatureDocLink()
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
    public string DocUid
    {
        get => _DocUid;
        set
        {
            if (_DocUid != value)
            {
                OnDocUidChanging(value);
                SendPropertyChanging("DocUid");
                _DocUid = value;
                SendPropertyChanged("DocUid");
                OnDocUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string SectionUid
    {
        get => _SectionUid;
        set
        {
            if (_SectionUid != value)
            {
                OnSectionUidChanging(value);
                SendPropertyChanging("SectionUid");
                _SectionUid = value;
                SendPropertyChanged("SectionUid");
                OnSectionUidChanged();
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
    partial void OnDocUidChanging(string value);

    partial void OnDocUidChanged();
    partial void OnSectionUidChanging(string value);

    partial void OnSectionUidChanged();

    #endregion
}