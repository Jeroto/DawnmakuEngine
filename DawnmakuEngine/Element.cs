using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DawnmakuEngine
{
    public abstract class Element
    {
        public static byte POST_CREATE_SUB = 1;
        public static byte UPDATE_SUB = 2;
        public static byte PRE_RENDER_SUB = 4;

        public static List<Element> allElements = new List<Element>();
        public byte requiredSubscriptions;


        protected Entity entityAttachedTo;
        public Entity EntityAttachedTo { get { return entityAttachedTo; } set { entityAttachedTo = value; } }
        public bool enabled = true;

        public bool IsEnabled
        { 
            get
            {
                if (!enabled)
                    return false;
                if(entityAttachedTo != null)
                    return entityAttachedTo.IsEnabled;
                return false;
            }
        }

        protected virtual void OnCreate()
        {
            if((requiredSubscriptions & POST_CREATE_SUB) == POST_CREATE_SUB)
                GameMaster.gameMaster.PostCreate += PostCreate;
            OnEnableAndCreate();
        }
        public virtual void PostCreate() { GameMaster.gameMaster.PostCreate -= PostCreate; }
        public virtual void OnUpdate() { }
        public virtual void PreRender() { }
        protected virtual void OnDisableAndDestroy()
        {
            if ((requiredSubscriptions & PRE_RENDER_SUB) == PRE_RENDER_SUB)
                GameMaster.gameMaster.PreRender -= PreRender;
            if ((requiredSubscriptions & UPDATE_SUB) == UPDATE_SUB)
                GameMaster.gameMaster.OnUpdate -= OnUpdate;
        }
        protected virtual void OnEnableAndCreate()
        {
            if ((requiredSubscriptions & PRE_RENDER_SUB) == PRE_RENDER_SUB)
                GameMaster.gameMaster.PreRender += PreRender;
            if ((requiredSubscriptions & UPDATE_SUB) == UPDATE_SUB)
                GameMaster.gameMaster.OnUpdate += OnUpdate;
        }
        protected virtual void OnDestroy() { }

        public virtual void Remove() { }

        public virtual Element Clone() { GameMaster.LogError("Cloning of " + this.GetType().Name + " has not been set up yet."); return null; }

        public void Enable()
        {
            enabled = true;
            OnEnableAndCreate();
        }
        public void Disable()
        {
            enabled = false;
            OnDisableAndDestroy();
        }

        /// <summary>
        /// Subscribes to any event that is passed as true (only Update by default)
        /// </summary>
        /// <param name="postCreate">True to subscribe to the PostCreate event</param>
        /// <param name="update">True to subscribe to the Update event</param>
        /// <param name="preRender">True to subscribe to the PreRender event</param>
        public void SetRequiredSubscriptions(bool postCreate = false, bool update = true, bool preRender = false)
        {
            requiredSubscriptions = 0;
            if (postCreate)
                requiredSubscriptions += POST_CREATE_SUB;
            if (update)
                requiredSubscriptions += UPDATE_SUB;
            if (preRender)
                requiredSubscriptions += PRE_RENDER_SUB;
        }

        public virtual void AttemptDelete()
        {
            enabled = false;
            OnDisableAndDestroy();
            OnDestroy();
            allElements.Remove(this);
            GameMaster.Log("Removing " + GetType().Name);
            EntityAttachedTo.RemoveElement(this);
        }

        /// <summary>
        /// Overload constructor in each element unless it does not need to be calld on PostCreate or PreRender
        /// (or if you want to disable Update as only it is true by default)
        /// </summary>
        /// <param name="postCreate">True to subscribe to the PostCreate event</param>
        /// <param name="update">True to subscribe to the Update event</param>
        /// <param name="preRender">True to subscribe to the PreRender event</param>
        public Element(bool postCreate = false, bool update = true, bool preRender = false) //MUST be included in the constructor of all Elements for many functions to work!
        {
            allElements.Add(this);  //Unless you add this line into the constructor 
            SetRequiredSubscriptions(postCreate, update, preRender); //And you call this with the needed values
            OnCreate();             //but basing other constructors off this leaves more Wriggle room for the future
        }

        ~Element()
        {
            allElements.Remove(this);
            OnDestroy();
            OnDisableAndDestroy();
        }
    }
}
