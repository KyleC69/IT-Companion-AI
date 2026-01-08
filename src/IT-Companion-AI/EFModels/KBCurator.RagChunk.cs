// Project Name: SKAgent
// File Name: KBCurator.RagChunk.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class RagChunk : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _ChunkUid;

    private string _EmbeddingVector;

    private Guid _Id;

    private string _Kind;

    private string _MetadataJson;

    private Guid _RagRunId;

    private string _Text;







    public RagChunk()
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
    public Guid RagRunId
    {
        get => _RagRunId;
        set
        {
            if (_RagRunId != value)
            {
                OnRagRunIdChanging(value);
                SendPropertyChanging("RagRunId");
                _RagRunId = value;
                SendPropertyChanged("RagRunId");
                OnRagRunIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string ChunkUid
    {
        get => _ChunkUid;
        set
        {
            if (_ChunkUid != value)
            {
                OnChunkUidChanging(value);
                SendPropertyChanging("ChunkUid");
                _ChunkUid = value;
                SendPropertyChanged("ChunkUid");
                OnChunkUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                OnKindChanging(value);
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
                OnKindChanged();
            }
        }
    }

    public string Text
    {
        get => _Text;
        set
        {
            if (_Text != value)
            {
                OnTextChanging(value);
                SendPropertyChanging("Text");
                _Text = value;
                SendPropertyChanged("Text");
                OnTextChanged();
            }
        }
    }

    public string MetadataJson
    {
        get => _MetadataJson;
        set
        {
            if (_MetadataJson != value)
            {
                OnMetadataJsonChanging(value);
                SendPropertyChanging("MetadataJson");
                _MetadataJson = value;
                SendPropertyChanged("MetadataJson");
                OnMetadataJsonChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1536, RangeBoundaryType.Inclusive)]
    public string EmbeddingVector
    {
        get => _EmbeddingVector;
        set
        {
            if (_EmbeddingVector != value)
            {
                OnEmbeddingVectorChanging(value);
                SendPropertyChanging("EmbeddingVector");
                _EmbeddingVector = value;
                SendPropertyChanged("EmbeddingVector");
                OnEmbeddingVectorChanged();
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
    partial void OnRagRunIdChanging(Guid value);

    partial void OnRagRunIdChanged();
    partial void OnChunkUidChanging(string value);

    partial void OnChunkUidChanged();
    partial void OnKindChanging(string value);

    partial void OnKindChanged();
    partial void OnTextChanging(string value);

    partial void OnTextChanged();
    partial void OnMetadataJsonChanging(string value);

    partial void OnMetadataJsonChanged();
    partial void OnEmbeddingVectorChanging(string value);

    partial void OnEmbeddingVectorChanged();

    #endregion
}