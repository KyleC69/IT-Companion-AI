namespace ITCompanionAI.EFModels;


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








    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
            }
        }
    }





    public Guid RagRunId
    {
        get => _RagRunId;
        set
        {
            if (_RagRunId != value)
            {
                SendPropertyChanging("RagRunId");
                _RagRunId = value;
                SendPropertyChanged("RagRunId");
            }
        }
    }





    public string ChunkUid
    {
        get => _ChunkUid;
        set
        {
            if (_ChunkUid != value)
            {
                SendPropertyChanging("ChunkUid");
                _ChunkUid = value;
                SendPropertyChanged("ChunkUid");
            }
        }
    }





    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
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
                SendPropertyChanging("Text");
                _Text = value;
                SendPropertyChanged("Text");
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
                SendPropertyChanging("MetadataJson");
                _MetadataJson = value;
                SendPropertyChanged("MetadataJson");
            }
        }
    }





    public string EmbeddingVector
    {
        get => _EmbeddingVector;
        set
        {
            if (_EmbeddingVector != value)
            {
                SendPropertyChanging("EmbeddingVector");
                _EmbeddingVector = value;
                SendPropertyChanged("EmbeddingVector");
            }
        }
    }





    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual event PropertyChangingEventHandler PropertyChanging;



    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion








    protected virtual void SendPropertyChanging()
    {
        PropertyChangingEventHandler handler = PropertyChanging;
        if (handler != null)
        {
            handler(this, emptyChangingEventArgs);
        }
    }








    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChangingEventHandler handler = PropertyChanging;
        if (handler != null)
        {
            handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }








    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}