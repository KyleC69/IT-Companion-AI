// Project Name: SKAgent
// File Name: KBCurator.IngestionRun.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class IngestionRun : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private ICollection<ApiFeature> _ApiFeatures;

    private ICollection<ApiMember> _ApiMembers;

    private ICollection<ApiParameter> _ApiParameters;

    private ICollection<ApiType> _ApiTypes;

    private ICollection<DocPage> _DocPages;

    private ICollection<DocSection> _DocSections;

    private Guid _Id;

    private string _Notes;

    private string _SchemaVersion;

    private DateTime _TimestampUtc;







    public IngestionRun()
    {
        _ApiFeatures = new List<ApiFeature>();
        _ApiMembers = new List<ApiMember>();
        _ApiParameters = new List<ApiParameter>();
        _ApiTypes = new List<ApiType>();
        _DocPages = new List<DocPage>();
        _DocSections = new List<DocSection>();
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
    public DateTime TimestampUtc
    {
        get => _TimestampUtc;
        set
        {
            if (_TimestampUtc != value)
            {
                OnTimestampUtcChanging(value);
                SendPropertyChanging("TimestampUtc");
                _TimestampUtc = value;
                SendPropertyChanged("TimestampUtc");
                OnTimestampUtcChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion
    {
        get => _SchemaVersion;
        set
        {
            if (_SchemaVersion != value)
            {
                OnSchemaVersionChanging(value);
                SendPropertyChanging("SchemaVersion");
                _SchemaVersion = value;
                SendPropertyChanged("SchemaVersion");
                OnSchemaVersionChanged();
            }
        }
    }

    public string Notes
    {
        get => _Notes;
        set
        {
            if (_Notes != value)
            {
                OnNotesChanging(value);
                SendPropertyChanging("Notes");
                _Notes = value;
                SendPropertyChanged("Notes");
                OnNotesChanged();
            }
        }
    }


    public virtual ICollection<ApiFeature> ApiFeatures
    {
        get => _ApiFeatures;
        set => _ApiFeatures = value;
    }


    public virtual ICollection<ApiMember> ApiMembers
    {
        get => _ApiMembers;
        set => _ApiMembers = value;
    }


    public virtual ICollection<ApiParameter> ApiParameters
    {
        get => _ApiParameters;
        set => _ApiParameters = value;
    }


    public virtual ICollection<ApiType> ApiTypes
    {
        get => _ApiTypes;
        set => _ApiTypes = value;
    }


    public virtual ICollection<DocPage> DocPages
    {
        get => _DocPages;
        set => _DocPages = value;
    }


    public virtual ICollection<DocSection> DocSections
    {
        get => _DocSections;
        set => _DocSections = value;
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
    partial void OnTimestampUtcChanging(DateTime value);

    partial void OnTimestampUtcChanged();
    partial void OnSchemaVersionChanging(string value);

    partial void OnSchemaVersionChanged();
    partial void OnNotesChanging(string value);

    partial void OnNotesChanged();

    #endregion
}