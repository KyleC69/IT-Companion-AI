// Project Name: SKAgent
// File Name: KBCurator.DocSection.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class DocSection : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private byte[] _ContentHash;

    private string _ContentMarkdown;

    private Guid _CreatedIngestionRunId;

    private Guid _DocPageId;

    private string _Heading;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private bool _IsActive;

    private int? _Level;

    private int? _OrderIndex;

    private Guid? _RemovedIngestionRunId;

    private string _SemanticUid;

    private byte[] _SemanticUidHash;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public DocSection()
    {
        _IsActive = true;
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
    public Guid DocPageId
    {
        get => _DocPageId;
        set
        {
            if (_DocPageId != value)
            {
                OnDocPageIdChanging(value);
                SendPropertyChanging("DocPageId");
                _DocPageId = value;
                SendPropertyChanged("DocPageId");
                OnDocPageIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid
    {
        get => _SemanticUid;
        set
        {
            if (_SemanticUid != value)
            {
                OnSemanticUidChanging(value);
                SendPropertyChanging("SemanticUid");
                _SemanticUid = value;
                SendPropertyChanged("SemanticUid");
                OnSemanticUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Heading
    {
        get => _Heading;
        set
        {
            if (_Heading != value)
            {
                OnHeadingChanging(value);
                SendPropertyChanging("Heading");
                _Heading = value;
                SendPropertyChanged("Heading");
                OnHeadingChanged();
            }
        }
    }

    public int? Level
    {
        get => _Level;
        set
        {
            if (_Level != value)
            {
                OnLevelChanging(value);
                SendPropertyChanging("Level");
                _Level = value;
                SendPropertyChanged("Level");
                OnLevelChanged();
            }
        }
    }

    public string ContentMarkdown
    {
        get => _ContentMarkdown;
        set
        {
            if (_ContentMarkdown != value)
            {
                OnContentMarkdownChanging(value);
                SendPropertyChanging("ContentMarkdown");
                _ContentMarkdown = value;
                SendPropertyChanged("ContentMarkdown");
                OnContentMarkdownChanged();
            }
        }
    }

    public int? OrderIndex
    {
        get => _OrderIndex;
        set
        {
            if (_OrderIndex != value)
            {
                OnOrderIndexChanging(value);
                SendPropertyChanging("OrderIndex");
                _OrderIndex = value;
                SendPropertyChanged("OrderIndex");
                OnOrderIndexChanged();
            }
        }
    }

    [NotNullValidator()]
    public int VersionNumber
    {
        get => _VersionNumber;
        set
        {
            if (_VersionNumber != value)
            {
                OnVersionNumberChanging(value);
                SendPropertyChanging("VersionNumber");
                _VersionNumber = value;
                SendPropertyChanged("VersionNumber");
                OnVersionNumberChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid CreatedIngestionRunId
    {
        get => _CreatedIngestionRunId;
        set
        {
            if (_CreatedIngestionRunId != value)
            {
                OnCreatedIngestionRunIdChanging(value);
                SendPropertyChanging("CreatedIngestionRunId");
                _CreatedIngestionRunId = value;
                SendPropertyChanged("CreatedIngestionRunId");
                OnCreatedIngestionRunIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid UpdatedIngestionRunId
    {
        get => _UpdatedIngestionRunId;
        set
        {
            if (_UpdatedIngestionRunId != value)
            {
                OnUpdatedIngestionRunIdChanging(value);
                SendPropertyChanging("UpdatedIngestionRunId");
                _UpdatedIngestionRunId = value;
                SendPropertyChanged("UpdatedIngestionRunId");
                OnUpdatedIngestionRunIdChanged();
            }
        }
    }

    public Guid? RemovedIngestionRunId
    {
        get => _RemovedIngestionRunId;
        set
        {
            if (_RemovedIngestionRunId != value)
            {
                OnRemovedIngestionRunIdChanging(value);
                SendPropertyChanging("RemovedIngestionRunId");
                _RemovedIngestionRunId = value;
                SendPropertyChanged("RemovedIngestionRunId");
                OnRemovedIngestionRunIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public DateTime ValidFromUtc
    {
        get => _ValidFromUtc;
        set
        {
            if (_ValidFromUtc != value)
            {
                OnValidFromUtcChanging(value);
                SendPropertyChanging("ValidFromUtc");
                _ValidFromUtc = value;
                SendPropertyChanged("ValidFromUtc");
                OnValidFromUtcChanged();
            }
        }
    }

    public DateTime? ValidToUtc
    {
        get => _ValidToUtc;
        set
        {
            if (_ValidToUtc != value)
            {
                OnValidToUtcChanging(value);
                SendPropertyChanging("ValidToUtc");
                _ValidToUtc = value;
                SendPropertyChanged("ValidToUtc");
                OnValidToUtcChanged();
            }
        }
    }

    [NotNullValidator()]
    public bool IsActive
    {
        get => _IsActive;
        set
        {
            if (_IsActive != value)
            {
                OnIsActiveChanging(value);
                SendPropertyChanging("IsActive");
                _IsActive = value;
                SendPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
    }

    public byte[] ContentHash
    {
        get => _ContentHash;
        set
        {
            if (_ContentHash != value)
            {
                OnContentHashChanging(value);
                SendPropertyChanging("ContentHash");
                _ContentHash = value;
                SendPropertyChanged("ContentHash");
                OnContentHashChanged();
            }
        }
    }

    public byte[] SemanticUidHash
    {
        get => _SemanticUidHash;
        set
        {
            if (_SemanticUidHash != value)
            {
                OnSemanticUidHashChanging(value);
                SendPropertyChanging("SemanticUidHash");
                _SemanticUidHash = value;
                SendPropertyChanged("SemanticUidHash");
                OnSemanticUidHashChanged();
            }
        }
    }


    public virtual IngestionRun IngestionRun
    {
        get => _IngestionRun;
        set
        {
            if (_IngestionRun != value)
            {
                OnIngestionRunChanging(value);
                SendPropertyChanging("IngestionRun");
                _IngestionRun = value;
                SendPropertyChanged("IngestionRun");
                OnIngestionRunChanged();
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
    partial void OnDocPageIdChanging(Guid value);

    partial void OnDocPageIdChanged();
    partial void OnSemanticUidChanging(string value);

    partial void OnSemanticUidChanged();
    partial void OnHeadingChanging(string value);

    partial void OnHeadingChanged();
    partial void OnLevelChanging(int? value);

    partial void OnLevelChanged();
    partial void OnContentMarkdownChanging(string value);

    partial void OnContentMarkdownChanged();
    partial void OnOrderIndexChanging(int? value);

    partial void OnOrderIndexChanged();
    partial void OnVersionNumberChanging(int value);

    partial void OnVersionNumberChanged();
    partial void OnCreatedIngestionRunIdChanging(Guid value);

    partial void OnCreatedIngestionRunIdChanged();
    partial void OnUpdatedIngestionRunIdChanging(Guid value);

    partial void OnUpdatedIngestionRunIdChanged();
    partial void OnRemovedIngestionRunIdChanging(Guid? value);

    partial void OnRemovedIngestionRunIdChanged();
    partial void OnValidFromUtcChanging(DateTime value);

    partial void OnValidFromUtcChanged();
    partial void OnValidToUtcChanging(DateTime? value);

    partial void OnValidToUtcChanged();
    partial void OnIsActiveChanging(bool value);

    partial void OnIsActiveChanged();
    partial void OnContentHashChanging(byte[] value);

    partial void OnContentHashChanged();
    partial void OnSemanticUidHashChanging(byte[] value);

    partial void OnSemanticUidHashChanged();
    partial void OnIngestionRunChanging(IngestionRun value);

    partial void OnIngestionRunChanged();

    #endregion
}