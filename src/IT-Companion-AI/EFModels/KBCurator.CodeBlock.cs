// Project Name: SKAgent
// File Name: KBCurator.CodeBlock.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class CodeBlock : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Content;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _DeclaredPackages;

    private Guid _DocSectionId;

    private Guid _Id;

    private string _InlineComments;

    private bool _IsActive;

    private string _Language;

    private Guid? _RemovedIngestionRunId;

    private string _SemanticUid;

    private string _Tags;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public CodeBlock()
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
    public Guid DocSectionId
    {
        get => _DocSectionId;
        set
        {
            if (_DocSectionId != value)
            {
                OnDocSectionIdChanging(value);
                SendPropertyChanging("DocSectionId");
                _DocSectionId = value;
                SendPropertyChanged("DocSectionId");
                OnDocSectionIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
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

    public string Content
    {
        get => _Content;
        set
        {
            if (_Content != value)
            {
                OnContentChanging(value);
                SendPropertyChanging("Content");
                _Content = value;
                SendPropertyChanged("Content");
                OnContentChanged();
            }
        }
    }

    public string DeclaredPackages
    {
        get => _DeclaredPackages;
        set
        {
            if (_DeclaredPackages != value)
            {
                OnDeclaredPackagesChanging(value);
                SendPropertyChanging("DeclaredPackages");
                _DeclaredPackages = value;
                SendPropertyChanged("DeclaredPackages");
                OnDeclaredPackagesChanged();
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

    public string InlineComments
    {
        get => _InlineComments;
        set
        {
            if (_InlineComments != value)
            {
                OnInlineCommentsChanging(value);
                SendPropertyChanging("InlineComments");
                _InlineComments = value;
                SendPropertyChanged("InlineComments");
                OnInlineCommentsChanged();
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
    partial void OnDocSectionIdChanging(Guid value);

    partial void OnDocSectionIdChanged();
    partial void OnSemanticUidChanging(string value);

    partial void OnSemanticUidChanged();
    partial void OnLanguageChanging(string value);

    partial void OnLanguageChanged();
    partial void OnContentChanging(string value);

    partial void OnContentChanged();
    partial void OnDeclaredPackagesChanging(string value);

    partial void OnDeclaredPackagesChanged();
    partial void OnTagsChanging(string value);

    partial void OnTagsChanged();
    partial void OnInlineCommentsChanging(string value);

    partial void OnInlineCommentsChanged();
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

    #endregion
}