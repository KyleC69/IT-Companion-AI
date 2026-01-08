// Project Name: SKAgent
// File Name: KBCurator.Sample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class Sample : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Code;

    private string _DerivedFromCodeUid;

    private string _EntryPoint;

    private string _FeatureUid;

    private Guid _Id;

    private string _Language;

    private string _PackageReferences;

    private Guid _SampleRunId;

    private string _SampleUid;

    private string _Tags;

    private string _TargetFramework;







    public Sample()
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
    public Guid SampleRunId
    {
        get => _SampleRunId;
        set
        {
            if (_SampleRunId != value)
            {
                OnSampleRunIdChanging(value);
                SendPropertyChanging("SampleRunId");
                _SampleRunId = value;
                SendPropertyChanged("SampleRunId");
                OnSampleRunIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SampleUid
    {
        get => _SampleUid;
        set
        {
            if (_SampleUid != value)
            {
                OnSampleUidChanging(value);
                SendPropertyChanging("SampleUid");
                _SampleUid = value;
                SendPropertyChanged("SampleUid");
                OnSampleUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string FeatureUid
    {
        get => _FeatureUid;
        set
        {
            if (_FeatureUid != value)
            {
                OnFeatureUidChanging(value);
                SendPropertyChanging("FeatureUid");
                _FeatureUid = value;
                SendPropertyChanged("FeatureUid");
                OnFeatureUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language
    {
        get => _Language;
        set
        {
            if (_Language != value)
            {
                OnLanguageChanging(value);
                SendPropertyChanging("Language");
                _Language = value;
                SendPropertyChanged("Language");
                OnLanguageChanged();
            }
        }
    }

    public string Code
    {
        get => _Code;
        set
        {
            if (_Code != value)
            {
                OnCodeChanging(value);
                SendPropertyChanging("Code");
                _Code = value;
                SendPropertyChanged("Code");
                OnCodeChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string EntryPoint
    {
        get => _EntryPoint;
        set
        {
            if (_EntryPoint != value)
            {
                OnEntryPointChanging(value);
                SendPropertyChanging("EntryPoint");
                _EntryPoint = value;
                SendPropertyChanged("EntryPoint");
                OnEntryPointChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string TargetFramework
    {
        get => _TargetFramework;
        set
        {
            if (_TargetFramework != value)
            {
                OnTargetFrameworkChanging(value);
                SendPropertyChanging("TargetFramework");
                _TargetFramework = value;
                SendPropertyChanged("TargetFramework");
                OnTargetFrameworkChanged();
            }
        }
    }

    public string PackageReferences
    {
        get => _PackageReferences;
        set
        {
            if (_PackageReferences != value)
            {
                OnPackageReferencesChanging(value);
                SendPropertyChanging("PackageReferences");
                _PackageReferences = value;
                SendPropertyChanged("PackageReferences");
                OnPackageReferencesChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string DerivedFromCodeUid
    {
        get => _DerivedFromCodeUid;
        set
        {
            if (_DerivedFromCodeUid != value)
            {
                OnDerivedFromCodeUidChanging(value);
                SendPropertyChanging("DerivedFromCodeUid");
                _DerivedFromCodeUid = value;
                SendPropertyChanged("DerivedFromCodeUid");
                OnDerivedFromCodeUidChanged();
            }
        }
    }

    public string Tags
    {
        get => _Tags;
        set
        {
            if (_Tags != value)
            {
                OnTagsChanging(value);
                SendPropertyChanging("Tags");
                _Tags = value;
                SendPropertyChanged("Tags");
                OnTagsChanged();
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
    partial void OnSampleRunIdChanging(Guid value);

    partial void OnSampleRunIdChanged();
    partial void OnSampleUidChanging(string value);

    partial void OnSampleUidChanged();
    partial void OnFeatureUidChanging(string value);

    partial void OnFeatureUidChanged();
    partial void OnLanguageChanging(string value);

    partial void OnLanguageChanged();
    partial void OnCodeChanging(string value);

    partial void OnCodeChanged();
    partial void OnEntryPointChanging(string value);

    partial void OnEntryPointChanged();
    partial void OnTargetFrameworkChanging(string value);

    partial void OnTargetFrameworkChanged();
    partial void OnPackageReferencesChanging(string value);

    partial void OnPackageReferencesChanged();
    partial void OnDerivedFromCodeUidChanging(string value);

    partial void OnDerivedFromCodeUidChanged();
    partial void OnTagsChanging(string value);

    partial void OnTagsChanged();

    #endregion
}