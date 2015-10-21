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

        
        
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objAsPart = obj as AbsElement;
            return objAsPart != null && Equals(objAsPart);
        }
        //Needed to define this, even if I think that it is not used in our context
        public override int GetHashCode()
        {
            return Id;
        }
        //Changed Equals to compare the names instead of the Id
        public bool Equals(AbsElement other)
        {
            return other != null && (Name.Equals(other.Name));
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public abstract string Write();
        
        public override string ToString()
        {
            return Name;
        }
    }
}