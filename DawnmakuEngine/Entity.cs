using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using OpenTK;
using OpenTK.Audio.OpenAL;
using static DawnmakuEngine.DawnMath;
using OpenTK.Mathematics;

namespace DawnmakuEngine
{
    public class Entity
    {
        public static List<Entity> allEntities = new List<Entity>();

        protected Entity parent; //Stores parent object

        protected string name;
        protected Vector3 position = Vector3.Zero; //Local position
        protected Quaternion rotation = Quaternion.Identity; //Local rotation
        protected Vector3 scale = Vector3.One;    //Local scale

        protected Vector3 worldPosition;
        protected Quaternion worldRotation;
        protected Vector3 worldScale;

        public event GameMaster.ElementFunction OnMove;
        public event GameMaster.ElementFunction OnRotate;
        public event GameMaster.ElementFunction OnScale;


        protected List<Entity> children = new List<Entity>();
        protected List<Element> elements = new List<Element>();

        public bool enabled = true;

        /******************
         ***Constructors***
         ******************/
        public Entity()
        {
            allEntities.Add(this);
        }

        public Entity(string name_) : this()
        {
            Name = name_;
        }
        public Entity(string name_, Vector3 localPosition_) : this(name_)
        {
            LocalPosition = localPosition_;
        }

        public Entity(string name_, Vector3 localPosition_, Vector3 localRotation_, Vector3 localScale_) : this(name_)
        {
            LocalPosition = localPosition_;
            LocalRotationRad = localRotation_;
            LocalScale = localScale_;
        }

        public Entity(string name_, Entity parent_) : this(name_)
        {
            parent = parent_;
            parent.children.Add(this);
        }
        public Entity(string name_, Entity parent_, Vector3 localPosition_) : this(name_, parent_)
        {
            LocalPosition = localPosition_;
        }
        public Entity(string name_, Entity parent_, Vector3 localPosition_, Vector3 localRotation_, Vector3 localScale_) : this(name_,
            localPosition_, localRotation_, localScale_)
        {
            parent = parent_;
            parent.children.Add(this);
        }

        //Destructor
        ~Entity()
        {
            int elementCount = elements.Count;
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i] = null;
            }
            elements = null;
            allEntities.Remove(this);
        }

        /******************
         ****Properties****
         ******************/
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Entity Parent
        {
            get { return parent; }
            set
            {
                if(parent != null)
                    parent.children.Remove(this);

                if (value == null)
                {
                    LocalPosition = WorldPosition;
                    LocalRotation = WorldRotation;
                    LocalScale = WorldScale;
                }
                else
                {
                    LocalRotation = WorldRotation * value.WorldRotation.Inverted();
                    LocalPosition = GetLocalPositionToObject(value);
                    Vector3 newScale = WorldScale, parentScale = value.WorldScale;
                    newScale.X /= parentScale.X;
                    newScale.Y /= parentScale.Y;
                    newScale.Z /= parentScale.Z;
                    LocalScale = newScale;
                    value.children.Add(this);
                }

                parent = value;
            }
        }
        public bool IsEnabled
        {
            get
            {
                if (!enabled)
                    return false;
                Entity currentEntity = parent;
                while(currentEntity != null)
                {
                    if (!currentEntity.enabled)
                        return false;
                    currentEntity = currentEntity.parent;
                }
                return true;
            }
        }
        public Vector3 LocalPosition
        {
            get { return position; }
            set 
            {
                position = value;
                SetWorldPosition();
                OnMove?.Invoke();
            }
        }
        public Vector3 WorldPosition
        {
            get
            {
                return worldPosition;
            }
            set
            {
                Vector3 currentWorldPosition = WorldPosition;
                if (parent != null)
                    LocalPosition += Vector3.Multiply(currentWorldPosition - value, parent.WorldScale);
                else
                    LocalPosition = value;
                SetChildrenWorldPosition();
            }
        }
        public Quaternion LocalRotation
        {   
            get { return rotation; }
            set 
            {
                rotation = value;
                SetChildrenWorldPosition();

                OnRotate?.Invoke();
            }
        }
        public Vector3 LocalRotationRad
        {
            get {return QuaternionToEuler(rotation); }
            set 
            {
                while (value.X < 0)
                    value.X += MathHelper.TwoPi;
                while (value.Y < 0)
                    value.Y += MathHelper.TwoPi;
                while (value.Z < 0)
                    value.Z += MathHelper.TwoPi;
                rotation = Quaternion.FromEulerAngles(value);
                SetChildrenWorldPosition();
                OnRotate?.Invoke();
            }
        }
        public Vector3 LocalRotationDegrees
        {
            get {
                Vector3 rot = QuaternionToEuler(rotation);
                return new Vector3(MathHelper.RadiansToDegrees(rot.X), MathHelper.RadiansToDegrees(rot.Y), MathHelper.RadiansToDegrees(rot.Z)); }
            set 
            {
                while (value.X < 0)
                    value.X += 360;
                while (value.Y < 0)
                    value.Y += 360;
                while (value.Z < 0)
                    value.Z += 360;
                rotation = Quaternion.FromEulerAngles(new Vector3(MathHelper.DegreesToRadians(value.X), MathHelper.DegreesToRadians(value.Y), MathHelper.DegreesToRadians(value.Z)));
                SetChildrenWorldPosition();
                OnRotate?.Invoke();
            }
        }
        public Vector3 WorldRotationRad
        {
            get
            {
                return QuaternionToEuler(WorldRotation);
            }
            //set { rotation = value; }
        }
        public Vector3 WorldRotationDeg
        {
            get
            {
                Vector3 euler = QuaternionToEuler(WorldRotation);
                euler.X = MathHelper.RadiansToDegrees(euler.X);
                euler.Y = MathHelper.RadiansToDegrees(euler.Y);
                euler.Z = MathHelper.RadiansToDegrees(euler.Z);
                return euler;
            }
            //set { rotation = value; }
        }
        public Quaternion WorldRotation
        {
            get
            {
                Quaternion rotation = LocalRotation;
                Entity currentEntity = parent;
                while (currentEntity != null)
                {
                    rotation *= currentEntity.LocalRotation;
                    currentEntity = currentEntity.parent;
                }
                return rotation;
            }
            //set { rotation = value; }
        }
        public Vector3 Forward{
            get { return (new Vector4(vec3Forward, 0) * Matrix4.CreateFromQuaternion(WorldRotation)).Xyz; }
        }
        public Vector3 Right{get { return Vector3.Cross(Forward, vec3Up).Normalized(); }}
        public Vector3 Up
        {
            get
            {
                Vector3 forward = Forward, right;
                right = Vector3.Cross(Forward, vec3Up).Normalized();
                return Vector3.Cross(right, forward).Normalized();
            }
        }
        public Vector3 LocalScale
        {
            get { return scale; }
            set 
            {
                scale = value;
                SetChildrenWorldPosition();
                OnScale?.Invoke();
            }
        }

        public Vector3 WorldScale
        {
            get
            {
                Vector3 scale = LocalScale;
                Entity currentEntity = parent;
                Vector4 vec4Scale = new Vector4(scale.X, scale.Y, scale.Z, 0);
                while (currentEntity != null)
                {
                    vec4Scale *= Matrix4.CreateScale(currentEntity.LocalScale);
                    currentEntity = currentEntity.parent;
                }
                return vec4Scale.Xyz;
            }
        }

        public void SetWorldPosition()
        {
            worldPosition = CalculateWorldPosition();
            SetChildrenWorldPosition();
        }
        public void SetChildrenWorldPosition()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                children[i].SetWorldPosition();
            }
        }

        public Vector3 CalculateWorldPosition()
        {
            Entity currentEntity = parent;      //Used to go up through each parent until we are at the top of the list
            Vector4 vec4Pos = new Vector4(LocalPosition, 1);   //Used in matrix multiplication
            Matrix4 modifyMatrix;
            while (currentEntity != null)
            {
                modifyMatrix = Matrix4.Identity //Identity base matrix
                    * Matrix4.CreateScale(currentEntity.LocalScale)             //Adds a scale of the parent's local scale to scale units
                    * Matrix4.CreateRotationX(currentEntity.LocalRotationRad.X)    //Rotates position based on parent's rotation
                    * Matrix4.CreateRotationY(currentEntity.LocalRotationRad.Y)    // /\
                    * Matrix4.CreateRotationZ(currentEntity.LocalRotationRad.Z)    // /\
                    * Matrix4.CreateTranslation(currentEntity.LocalPosition)   //Adds translation of the parent's position
                    ;

                vec4Pos *= modifyMatrix;    //Modifies vector with the above matrix
                currentEntity = currentEntity.parent;   //Updates to parent of parent
            }
            return vec4Pos.Xyz;
        }

        public Vector3 GetLocalPositionToObject(Entity obj)
        {
            Entity currentEntity = obj;      //Used to go up through each parent until we are at the top of the list
            Vector4 vec4Pos = new Vector4(WorldPosition, 1);   //Used in matrix multiplication
            Matrix4 modifyMatrix;
            while (currentEntity != null)
            {
                modifyMatrix = Matrix4.Identity //Identity base matrix
                    * Matrix4.CreateScale(currentEntity.LocalScale)             //Adds a scale of the parent's local scale to scale units
                    * Matrix4.CreateRotationX(currentEntity.LocalRotationRad.X)    //Rotates position based on parent's rotation
                    * Matrix4.CreateRotationY(currentEntity.LocalRotationRad.Y)    // /\
                    * Matrix4.CreateRotationZ(currentEntity.LocalRotationRad.Z)    // /\
                    * Matrix4.CreateTranslation(currentEntity.LocalPosition)   //Adds translation of the parent's position
                    ;

                vec4Pos *= modifyMatrix.Inverted();    //Modifies vector with the above matrix
                currentEntity = currentEntity.parent;   //Updates to parent of parent
            }
            return vec4Pos.Xyz;
        }

        public static Entity FindEntity(string searchName)
        {
            int count = allEntities.Count;
            for (int i = 0; i < count; i++)
            {
                if (allEntities[i].name.CompareTo(searchName) == 0)
                    return allEntities[i];
            }
            return null;
        }
        public override string ToString()
        {
            return Name + ": localpos(" + LocalPosition + "),worldpos(" + WorldPosition + "),localrot(" + LocalRotationRad + "),worldrot("
                + WorldRotation + "),localscale(" + LocalScale + "),worldscale(" + WorldScale + ")";
        }
        public string WriteElementList()
        {
            string elementString = "Elements:";
            for (int i = 0; i < elements.Count; i++)
            {
                elementString += elements[i].GetType() + ",";
            }
            return elementString;
        }

        public void AddElement(Element newElement)
        {
            newElement.EntityAttachedTo = this;
            elements.Add(newElement);
        }

        public void RemoveElement(Element toRemove)
        {
            elements.Remove(toRemove);
        }
        public void RemoveAllElements()
        {
            elements = new List<Element>();
        }
        public void DeleteAllElements()
        {
            while(elements.Count > 0)
                elements[0].AttemptDelete();
            elements = new List<Element>();
        }
        public void DeleteAllElementsOfType<T>()
        {
            Element[] elementsOfType = GetElementsAsElement<T>();
            for (int i = 0; i < elementsOfType.Length; i++)
                elementsOfType[i].AttemptDelete();
        }

        public void Enable()
        {
            int count = elements.Count;
            for (int i = 0; i < count; i++)
            {
                elements[i].Enable();
            }
            enabled = true;
        }

        public void Disable()
        {
            int count = elements.Count;
            for (int i = 0; i < count; i++)
            {
                elements[i].Disable();
            }
            enabled = false;
        }

        public T GetElement<T>()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    return (T)Convert.ChangeType(elements[i], typeof(T));
            }
            return default;
        }
        public T[] GetElements<T>()
        {
            List<T> tempList = new List<T>();
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    tempList.Add((T)Convert.ChangeType(elements[i], typeof(T)));
            }
            return tempList.ToArray();
        }
        public List<T> GetElementsInList<T>()
        {
            List<T> tempList = new List<T>();
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    tempList.Add((T)Convert.ChangeType(elements[i], typeof(T)));
            }
            return tempList;
        }
        public Element GetElementAsElement<T>()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    return elements[i];
            }
            return null;
        }
        public Element[] GetElementsAsElement<T>()
        {
            List<Element> tempList = new List<Element>();
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    tempList.Add(elements[i]);
            }
            return tempList.ToArray();
        }
        public List<Element> GetElementsAsElementsInList<T>()
        {
            List<Element> tempList = new List<Element>();
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetType() == typeof(T))
                    tempList.Add(elements[i]);
            }
            return tempList;
        }

        public Entity GetChild(int index)
        {
            return children[index];
        }
        public Entity GetLastChild()
        {
            return children[children.Count - 1];
        }
        public Entity FindChild(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name == name)
                    return children[i];
            }
            return null;
        }
        public Entity[] FindChildren(string name)
        {
            List<Entity> childrenFound = new List<Entity>();
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name == name)
                    childrenFound.Add(children[i]);
            }
            return childrenFound.ToArray();
        }
        public int ChildCount { get { return children.Count; } }

        public void LookAt()
        {

        }

        public void RotateDegrees(Vector3 axis)
        {
            axis.X = MathHelper.DegreesToRadians(axis.X);
            axis.Y = MathHelper.DegreesToRadians(axis.Y);
            axis.Z = MathHelper.DegreesToRadians(axis.Z);
            Rotate(axis);
        }

        public void RotateQuaternion(Vector3 axis, float angle)
        {
            LocalRotation = Quaternion.Multiply(rotation, Quaternion.FromAxisAngle(axis, angle));
        }
        public void SetQuaternion(Vector3 axis, float angle)
        {
            LocalRotation = Quaternion.FromAxisAngle(axis, angle);
        }
        public void Rotate(Vector3 axis)
        {
            /*
            Matrix4 rotateMatr = Matrix4.Identity * Matrix4.CreateRotationX(axis.X) *
                Matrix4.CreateRotationY(axis.Y) * Matrix4.CreateRotationZ(axis.Z);
            Vector3 tempRotation = LocalRotation;
            LocalRotation = (new Vector4(tempRotation.X, tempRotation.Y, tempRotation.Z, 0) * rotateMatr).Xyz;*/
            LocalRotationRad += axis;
        }

        public void AttemptDelete()
        {
            allEntities.Remove(this);
            Disable();
            DeleteAllElements();

            if (parent != null)
                parent.children.Remove(this);

            while(children.Count > 0)
                children[0].AttemptDelete();
        }
    }
}
