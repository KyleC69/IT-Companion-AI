// Project Name: SKAgent
// File Name: KBCurator.ReviewIssue.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class ReviewIssue : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Code;

    private string _Details;

    private Guid _Id;

    private string _RelatedMemberUid;

    private Guid _ReviewItemId;

    private string _Severity;







    public ReviewIssue()
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
    public Guid ReviewItemId
    {
        get => _ReviewItemId;
        set
        {
            if (_ReviewItemId != value)
            {
                OnReviewItemIdChanging(value);
                SendPropertyChanging("ReviewItemId");
                _ReviewItemId = value;
                SendPropertyChanged("ReviewItemId");
                OnReviewItemIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
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

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Severity
    {
        get => _Severity;
        set
        {
            if (_Severity != value)
            {
                OnSeverityChanging(value);
                SendPropertyChanging("Severity");
                _Severity = value;
                SendPropertyChanged("Severity");
                OnSeverityChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string RelatedMemberUid
    {
        get => _RelatedMemberUid;
        set
        {
            if (_RelatedMemberUid != value)
            {
                OnRelatedMemberUidChanging(value);
                SendPropertyChanging("RelatedMemberUid");
                _RelatedMemberUid = value;
                SendPropertyChanged("RelatedMemberUid");
                OnRelatedMemberUidChanged();
            }
        }
    }

    public string Details
    {
        get => _Details;
        set
        {
            if (_Details != value)
            {
                OnDetailsChanging(value);
                SendPropertyChanging("Details");
                _Details = value;
                SendPropertyChanged("Details");
                OnDetailsChanged();
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
    partial void OnReviewItemIdChanging(Guid value);

    partial void OnReviewItemIdChanged();
    partial void OnCodeChanging(string value);

    partial void OnCodeChanged();
    partial void OnSeverityChanging(string value);

    partial void OnSeverityChanged();
    partial void OnRelatedMemberUidChanging(string value);

    partial void OnRelatedMemberUidChanged();
    partial void OnDetailsChanging(string value);

    partial void OnDetailsChanged();

    #endregion
}