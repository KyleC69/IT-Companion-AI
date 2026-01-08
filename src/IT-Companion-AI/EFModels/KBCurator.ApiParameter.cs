// Project Name: SKAgent
// File Name: KBCurator.ApiParameter.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class ApiParameter : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _ApiMemberId;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _DefaultValueLiteral;

    private bool? _HasDefaultValue;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private bool _IsActive;

    private string _Modifier;

    private string _Name;

    private string _NullableAnnotation;

    private int? _Position;

    private Guid? _RemovedIngestionRunId;

    private byte[] _SemanticUidHash;

    private string _TypeUid;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public ApiParameter()
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
    public Guid ApiMemberId
    {
        get => _ApiMemberId;
        set
        {
            if (_ApiMemberId != value)
            {
                OnApiMemberIdChanging(value);
                SendPropertyChanging("ApiMemberId");
                _ApiMemberId = value;
                SendPropertyChanged("ApiMemberId");
                OnApiMemberIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Name
    {
        get => _Name;
        set
        {
            if (_Name != value)
            {
                OnNameChanging(value);
                SendPropertyChanging("Name");
                _Name = value;
                SendPropertyChanged("Name");
                OnNameChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
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
    public string NullableAnnotation
    {
        get => _NullableAnnotation;
        set
        {
            if (_NullableAnnotation != value)
            {
                OnNullableAnnotationChanging(value);
                SendPropertyChanging("NullableAnnotation");
                _NullableAnnotation = value;
                SendPropertyChanged("NullableAnnotation");
                OnNullableAnnotationChanged();
            }
        }
    }

    public int? Position
    {
        get => _Position;
        set
        {
            if (_Position != value)
            {
                OnPositionChanging(value);
                SendPropertyChanging("Position");
                _Position = value;
                SendPropertyChanged("Position");
                OnPositionChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Modifier
    {
        get => _Modifier;
        set
        {
            if (_Modifier != value)
            {
                OnModifierChanging(value);
                SendPropertyChanging("Modifier");
                _Modifier = value;
                SendPropertyChanged("Modifier");
                OnModifierChanged();
            }
        }
    }

    public bool? HasDefaultValue
    {
        get => _HasDefaultValue;
        set
        {
            if (_HasDefaultValue != value)
            {
                OnHasDefaultValueChanging(value);
                SendPropertyChanging("HasDefaultValue");
                _HasDefaultValue = value;
                SendPropertyChanged("HasDefaultValue");
                OnHasDefaultValueChanged();
            }
        }
    }

    public string DefaultValueLiteral
    {
        get => _DefaultValueLiteral;
        set
        {
            if (_DefaultValueLiteral != value)
            {
                OnDefaultValueLiteralChanging(value);
                SendPropertyChanging("DefaultValueLiteral");
                _DefaultValueLiteral = value;
                SendPropertyChanged("DefaultValueLiteral");
                OnDefaultValueLiteralChanged();
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
    partial void OnApiMemberIdChanging(Guid value);

    partial void OnApiMemberIdChanged();
    partial void OnNameChanging(string value);

    partial void OnNameChanged();
    partial void OnTypeUidChanging(string value);

    partial void OnTypeUidChanged();
    partial void OnNullableAnnotationChanging(string value);

    partial void OnNullableAnnotationChanged();
    partial void OnPositionChanging(int? value);

    partial void OnPositionChanged();
    partial void OnModifierChanging(string value);

    partial void OnModifierChanged();
    partial void OnHasDefaultValueChanging(bool? value);

    partial void OnHasDefaultValueChanged();
    partial void OnDefaultValueLiteralChanging(string value);

    partial void OnDefaultValueLiteralChanged();
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