// ***********************************************************************
// Company          : KimCh
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : KimCh
// Last Modified On : KimCh
// ***********************************************************************
using System.Collections.Generic;
using K.Patterns;
using UnityEngine;

/// <summary>
/// 中介者模式(Unity)
/// A base <c>IMediator</c> implementation
/// </summary>
public class UnityMediator : MonoBehaviour, IMediator, INotifier
{
    #region Constants

    /// <summary>
    /// The name of the <c>Mediator</c>
    /// </summary>
    public const string NAME = "UnityMediator";

    #endregion

    #region Unity

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake()
    {
        facade.RegisterMediator(this);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDestroy()
    {
        facade.RemoveMediator(this.mediatorName);
    }

    #endregion

    #region IMediator Members

    /// <summary>
    /// List the <c>INotification</c> names this <c>Mediator</c> is interested in being notified of
    /// </summary>
    /// <returns>The list of <c>INotification</c> names </returns>
    public virtual IList<string> ListNotificationInterests()
    {
        return new List<string>();
    }

    /// <summary>
    /// Handle <c>INotification</c>s
    /// </summary>
    /// <param name="notification">The <c>INotification</c> instance to handle</param>
    public virtual void HandleNotification(INotification notification)
    {
    }

    /// <summary>
    /// Called by the View when the Mediator is registered
    /// </summary>
    public virtual void OnRegister()
    {
    }

    /// <summary>
    /// Called by the View when the Mediator is removed
    /// </summary>
    public virtual void OnRemove()
    {
    }

    #endregion

    #region INotifier Members

    /// <summary>
    /// Send an <c>INotification</c>
    /// </summary>
    /// <param name="notificationName">The name of the notiification to send</param>
    public virtual void SendNotification(string notificationName)
    {
        // The Facade SendNotification is thread safe, therefore this method is thread safe.
        _facade.SendNotification(notificationName);
    }

    /// <summary>
    /// Send an <c>INotification</c>
    /// </summary>
    /// <param name="notificationName">The name of the notification to send</param>
    /// <param name="body">The body of the notification</param>
    public virtual void SendNotification(string notificationName, object body)
    {
        // The Facade SendNotification is thread safe, therefore this method is thread safe.
        _facade.SendNotification(notificationName, body);
    }

    /// <summary>
    /// Send an <c>INotification</c>
    /// </summary>
    /// <param name="notificationName">The name of the notification to send</param>
    /// <param name="body">The body of the notification</param>
    /// <param name="type">The type of the notification</param>
    public virtual void SendNotification(string notificationName, object body, string type)
    {
        // The Facade SendNotification is thread safe, therefore this method is thread safe.
        _facade.SendNotification(notificationName, body, type);
    }

    #endregion

    #region Accessors

    /// <summary>
    /// The name of the <c>Mediator</c>
    /// </summary>
    /// <remarks><para>You should override this in your subclass</para></remarks>
    public virtual string mediatorName
    {
        get { return NAME; }
    }

    /// <summary>
    /// The <code>IMediator</code>'s view component.
    /// </summary>
    public virtual object viewComponent
    {
        get { return _viewComponent; }
        set { _viewComponent = value; }
    }

    /// <summary>
    /// Local reference to the Facade Singleton
    /// </summary>
    protected IFacade facade
    {
        get { return _facade; }
    }

    #endregion

    #region Members

    /// <summary>
    /// Local reference to the Facade Singleton
    /// </summary>
    private IFacade _facade = Facade.Instance;

    /// <summary>
    /// The view component being mediated
    /// </summary>
    protected object _viewComponent;

    #endregion  
}