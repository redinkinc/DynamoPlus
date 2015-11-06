using System;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    /// <summary>
    /// Data_Type that implements IEquatable to make Duplicates impossible
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public abstract class AbsElement:IEquatable<AbsElement>
    {
        /// <summary>
        /// 
        /// </summary>
        internal string Name { get; set; }
        protected int Id;
        
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objAsPart = obj as AbsElement;
            return objAsPart != null && Equals(objAsPart);
        }

        /// <summary>
        /// Needed to define this, even if I think that it is not used in our context
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(AbsElement other)
        {
            return other != null && (Name.Equals(other.Name));
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public abstract string Write();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}