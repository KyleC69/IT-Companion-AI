// Project Name: SKAgent
// File Name: KBCurator.SourceSnapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class SourceSnapshot : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Branch;

    private string _ConfigJson;

    private Guid _Id;

    private Guid _IngestionRunId;

    private string _Language;

    private string _PackageName;

    private string _PackageVersion;

    private string _RepoCommit;

    private string _RepoUrl;

    private string _SnapshotUid;

    private byte[] _SnapshotUidHash;







    public SourceSnapshot()
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
    public Guid IngestionRunId
    {
        get => _IngestionRunId;
        set
        {
            if (_IngestionRunId != value)
            {
                OnIngestionRunIdChanging(value);
                SendPropertyChanging("IngestionRunId");
                _IngestionRunId = value;
                SendPropertyChanged("IngestionRunId");
                OnIngestionRunIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SnapshotUid
    {
        get => _SnapshotUid;
        set
        {
            if (_SnapshotUid != value)
            {
                OnSnapshotUidChanging(value);
                SendPropertyChanging("SnapshotUid");
                _SnapshotUid = value;
                SendPropertyChanged("SnapshotUid");
                OnSnapshotUidChanged();
            }
        }
    }

    public string RepoUrl
    {
        get => _RepoUrl;
        set
        {
            if (_RepoUrl != value)
            {
                OnRepoUrlChanging(value);
                SendPropertyChanging("RepoUrl");
                _RepoUrl = value;
                SendPropertyChanged("RepoUrl");
                OnRepoUrlChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Branch
    {
        get => _Branch;
        set
        {
            if (_Branch != value)
            {
                OnBranchChanging(value);
                SendPropertyChanging("Branch");
                _Branch = value;
                SendPropertyChanged("Branch");
                OnBranchChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string RepoCommit
    {
        get => _RepoCommit;
        set
        {
            if (_RepoCommit != value)
            {
                OnRepoCommitChanging(value);
                SendPropertyChanging("RepoCommit");
                _RepoCommit = value;
                SendPropertyChanged("RepoCommit");
                OnRepoCommitChanged();
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

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageName
    {
        get => _PackageName;
        set
        {
            if (_PackageName != value)
            {
                OnPackageNameChanging(value);
                SendPropertyChanging("PackageName");
                _PackageName = value;
                SendPropertyChanged("PackageName");
                OnPackageNameChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageVersion
    {
        get => _PackageVersion;
        set
        {
            if (_PackageVersion != value)
            {
                OnPackageVersionChanging(value);
                SendPropertyChanging("PackageVersion");
                _PackageVersion = value;
                SendPropertyChanged("PackageVersion");
                OnPackageVersionChanged();
            }
        }
    }

    public string ConfigJson
    {
        get => _ConfigJson;
        set
        {
            if (_ConfigJson != value)
            {
                OnConfigJsonChanging(value);
                SendPropertyChanging("ConfigJson");
                _ConfigJson = value;
                SendPropertyChanged("ConfigJson");
                OnConfigJsonChanged();
            }
        }
    }

    public byte[] SnapshotUidHash
    {
        get => _SnapshotUidHash;
        set
        {
            if (_SnapshotUidHash != value)
            {
                OnSnapshotUidHashChanging(value);
                SendPropertyChanging("SnapshotUidHash");
                _SnapshotUidHash = value;
                SendPropertyChanged("SnapshotUidHash");
                OnSnapshotUidHashChanged();
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
    partial void OnIngestionRunIdChanging(Guid value);

    partial void OnIngestionRunIdChanged();
    partial void OnSnapshotUidChanging(string value);

    partial void OnSnapshotUidChanged();
    partial void OnRepoUrlChanging(string value);

    partial void OnRepoUrlChanged();
    partial void OnBranchChanging(string value);

    partial void OnBranchChanged();
    partial void OnRepoCommitChanging(string value);

    partial void OnRepoCommitChanged();
    partial void OnLanguageChanging(string value);

    partial void OnLanguageChanged();
    partial void OnPackageNameChanging(string value);

    partial void OnPackageNameChanged();
    partial void OnPackageVersionChanging(string value);

    partial void OnPackageVersionChanged();
    partial void OnConfigJsonChanging(string value);

    partial void OnConfigJsonChanged();
    partial void OnSnapshotUidHashChanging(byte[] value);

    partial void OnSnapshotUidHashChanged();

    #endregion
}