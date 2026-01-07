// Project Name: SKAgent
// File Name: KBCurator.ReviewItem.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class ReviewItem : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private Guid _ReviewRunId;

    private string _Status;

    private string _Summary;

    private string _TargetKind;

    private string _TargetUid;







    public ReviewItem()
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
    public Guid ReviewRunId
    {
        get => _ReviewRunId;
        set
        {
            if (_ReviewRunId != value)
            {
                OnReviewRunIdChanging(value);
                SendPropertyChanging("ReviewRunId");
                _ReviewRunId = value;
                SendPropertyChanged("ReviewRunId");
                OnReviewRunIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TargetKind
    {
        get => _TargetKind;
        set
        {
            if (_TargetKind != value)
            {
                OnTargetKindChanging(value);
                SendPropertyChanging("TargetKind");
                _TargetKind = value;
                SendPropertyChanged("TargetKind");
                OnTargetKindChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TargetUid
    {
        get => _TargetUid;
        set
        {
            if (_TargetUid != value)
            {
                OnTargetUidChanging(value);
                SendPropertyChanging("TargetUid");
                _TargetUid = value;
                SendPropertyChanged("TargetUid");
                OnTargetUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Status
    {
        get => _Status;
        set
        {
            if (_Status != value)
            {
                OnStatusChanging(value);
                SendPropertyChanging("Status");
                _Status = value;
                SendPropertyChanged("Status");
                OnStatusChanged();
            }
        }
    }

    public string Summary
    {
        get => _Summary;
        set
        {
            if (_Summary != value)
            {
                OnSummaryChanging(value);
                SendPropertyChanging("Summary");
                _Summary = value;
                SendPropertyChanged("Summary");
                OnSummaryChanged();
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
    partial void OnReviewRunIdChanging(Guid value);

    partial void OnReviewRunIdChanged();
    partial void OnTargetKindChanging(string value);

    partial void OnTargetKindChanged();
    partial void OnTargetUidChanging(string value);

    partial void OnTargetUidChanged();
    partial void OnStatusChanging(string value);

    partial void OnStatusChanged();
    partial void OnSummaryChanging(string value);

    partial void OnSummaryChanged();

    #endregion
}