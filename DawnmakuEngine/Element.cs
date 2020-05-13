using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine
{
    public abstract class Element
    {
        public static List<Element> allElements = new List<Element>();

        protected Entity entityAttachedTo;
        public Entity EntityAttachedTo { get { return entityAttachedTo; } set { entityAttachedTo = value; } }
        public bool enabled = true;
        protected bool postCreateRan;
        public bool PostCreateRan { get { return postCreateRan; } }
        protected virtual void OnCreate() { }
        public virtual void PostCreate() { postCreateRan = true; }
        public virtual void OnUpdate() { }
        public virtual void PreRender() { }
        protected virtual void OnDisableAndDestroy() { }
        protected virtual void OnEnableAndCreate() { }
        protected virtual void OnDestroy() { }

        public virtual void Remove() { }

        public virtual Element Clone() { Console.WriteLine("Cloning of " + this.GetType().Name + " has not been set up yet."); return null; }

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

        public virtual void AttemptDelete()
        {
            enabled = false;
            OnDisableAndDestroy();
            OnDestroy();
            allElements.Remove(this);
            EntityAttachedTo.RemoveElement(this);
        }

        public Element() //MUST be included in the constructor of all Elements for many functions to work!
        {
            allElements.Add(this);  //Unless you add this line into the constructor 
            OnCreate();             //but basing other constructors off this leaves more Wriggle room for the future
            OnEnableAndCreate();
        }

        ~Element()
        {
            allElements.Remove(this);
            OnDestroy();
            OnDisableAndDestroy();
        }
    }
}
