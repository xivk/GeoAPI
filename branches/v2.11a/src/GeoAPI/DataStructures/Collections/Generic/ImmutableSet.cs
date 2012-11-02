/* Copyright � 2002-2004 by Aidant Systems, Inc., and by Jason Smith. */
using System;
using System.Collections.Generic;

namespace GeoAPI.DataStructures.Collections.Generic
{
    /// <summary>
    /// <p>Implements an immutable (read-only) <c>Set</c> wrapper.</p>
    /// <p>Although this is advertised as immutable, it really isn't.  Anyone with access to the
    /// <c>basisSet</c> can still change the data-set.  So <c>GetHashCode()</c> is not implemented
    /// for this <c>Set</c>, as is the case for all <c>Set</c> implementations in this library.
    /// This design decision was based on the efficiency of not having to <c>Clone()</c> the 
    /// <c>basisSet</c> every time you wrap a mutable <c>Set</c>.</p>
    /// </summary>
    [Serializable]
    public sealed class ImmutableSet<T> : Set<T>
    {
        private const string ErrorMessage = "Object is immutable.";
        private ISet<T> _basisSet;

        internal ISet<T> BasisSet
        {
            get { return _basisSet; }
        }

        public override Boolean IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Constructs an immutable (read-only) <c>Set</c> wrapper.
        /// </summary>
        /// <param name="basisSet">The <c>Set</c> that is wrapped.</param>
        public ImmutableSet(ISet<T> basisSet)
        {
            _basisSet = basisSet;
        }

        /// <summary>
        /// Adds the specified element to this set if it is not already present.
        /// </summary>
        /// <param name="o">The Object to add to the set.</param>
        /// <returns><see langword="true"/> is the Object was added, <see langword="false"/> if it was already present.</returns>
        public override Boolean Add(T o)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Adds all the elements in the specified collection to the set if they are not already present.
        /// </summary>
        /// <param name="c">A collection of objects to add to the set.</param>
        /// <returns><see langword="true"/> is the set changed as a result of this operation, <see langword="false"/> if not.</returns>
        public override Boolean AddRange(IEnumerable<T> c)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Removes all objects from the set.
        /// </summary>
        public override void Clear()
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this set contains the specified element.
        /// </summary>
        /// <param name="o">The element to look for.</param>
        /// <returns><see langword="true"/> if this set contains the specified element, <see langword="false"/> otherwise.</returns>
        public override Boolean Contains(T o)
        {
            return _basisSet.Contains(o);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the set contains all the elements in the specified collection.
        /// </summary>
        /// <param name="c">A collection of objects.</param>
        /// <returns><see langword="true"/> if the set contains all the elements in the specified collection, <see langword="false"/> otherwise.</returns>
        public override Boolean ContainsAll(IEnumerable<T> c)
        {
            return _basisSet.ContainsAll(c);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this set contains no elements.
        /// </summary>
        public override Boolean IsEmpty
        {
            get { return _basisSet.IsEmpty; }
        }


        /// <summary>
        /// Removes the specified element from the set.
        /// </summary>
        /// <param name="o">The element to be removed.</param>
        /// <returns><see langword="true"/> if the set contained the specified element, <see langword="false"/> otherwise.</returns>
        public override Boolean Remove(T o)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Remove all the specified elements from this set, if they exist in this set.
        /// </summary>
        /// <param name="c">A collection of elements to remove.</param>
        /// <returns><see langword="true"/> if the set was modified as a result of this operation.</returns>
        public override Boolean RemoveAll(IEnumerable<T> c)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Retains only the elements in this set that are contained in the specified collection.
        /// </summary>
        /// <param name="c">Collection that defines the set of elements to be retained.</param>
        /// <returns><see langword="true"/> if this set changed as a result of this operation.</returns>
        public override Boolean RetainAll(IEnumerable<T> c)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// Copies the elements in the <c>Set</c> to an array.  The type of array needs
        /// to be compatible with the objects in the <c>Set</c>, obviously.
        /// </summary>
        /// <param name="array">An array that will be the target of the copy operation.</param>
        /// <param name="index">The zero-based index where copying will start.</param>
        public override void CopyTo(T[] array, Int32 index)
        {
            _basisSet.CopyTo(array, index);
        }

        /// <summary>
        /// The number of elements contained in this collection.
        /// </summary>
        public override Int32 Count
        {
            get { return _basisSet.Count; }
        }

        /// <summary>
        /// Gets an enumerator for the elements in the <c>Set</c>.
        /// </summary>
        /// <returns>An <c>IEnumerator</c> over the elements in the <c>Set</c>.</returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return _basisSet.GetEnumerator();
        }

        /// <summary>
        /// Returns a clone of the <c>Set</c> instance.  
        /// </summary>
        /// <returns>A clone of this Object.</returns>
        public override ISet<T> Clone()
        {
            return new ImmutableSet<T>(_basisSet);
        }

        /// <summary>
        /// Performs a "union" of the two sets, where all the elements
        /// in both sets are present.  That is, the element is included if it is in either <c>a</c> or <c>b</c>.
        /// Neither this set nor the input set are modified during the operation.  The return value
        /// is a <c>Clone()</c> of this set with the extra elements added in.
        /// </summary>
        /// <param name="a">A collection of elements.</param>
        /// <returns>A new <c>Set</c> containing the union of this <c>Set</c> with the specified collection.
        /// Neither of the input objects is modified by the union.</returns>
        public override ISet<T> Union(ISet<T> a)
        {
            ISet<T> m = unwrap(this);
            return new ImmutableSet<T>(m.Union(a));
        }

        /// <summary>
        /// Performs an "intersection" of the two sets, where only the elements
        /// that are present in both sets remain.  That is, the element is included if it exists in
        /// both sets.  The <c>Intersect()</c> operation does not modify the input sets.  It returns
        /// a <c>Clone()</c> of this set with the appropriate elements removed.
        /// </summary>
        /// <param name="a">A set of elements.</param>
        /// <returns>The intersection of this set with <c>a</c>.</returns>
        public override ISet<T> Intersect(ISet<T> a)
        {
            ISet<T> m = unwrap(this);
            return new ImmutableSet<T>(m.Intersect(a));
        }

        /// <summary>
        /// Performs a "minus" of set <c>b</c> from set <c>a</c>.  This returns a set of all
        /// the elements in set <c>a</c>, removing the elements that are also in set <c>b</c>.
        /// The original sets are not modified during this operation.  The result set is a <c>Clone()</c>
        /// of this <c>Set</c> containing the elements from the operation.
        /// </summary>
        /// <param name="a">A set of elements.</param>
        /// <returns>A set containing the elements from this set with the elements in <c>a</c> removed.</returns>
        public override ISet<T> Minus(ISet<T> a)
        {
            ISet<T> m = unwrap(this);
            return new ImmutableSet<T>(m.Minus(a));
        }

        /// <summary>
        /// Performs an "exclusive-or" of the two sets, keeping only the elements that
        /// are in one of the sets, but not in both.  The original sets are not modified
        /// during this operation.  The result set is a <c>Clone()</c> of this set containing
        /// the elements from the exclusive-or operation.
        /// </summary>
        /// <param name="a">A set of elements.</param>
        /// <returns>A set containing the result of <c>a ^ b</c>.</returns>
        public override ISet<T> ExclusiveOr(ISet<T> a)
        {
            ISet<T> m = unwrap(this);
            return new ImmutableSet<T>(m.ExclusiveOr(a));
        }

        private static ISet<T> unwrap(ImmutableSet<T> set)
        {
            ISet<T> m = set;

            while (m is ImmutableSet<T>)
            {
                m = ((ImmutableSet<T>) m).BasisSet;
            }

            return m;
        }

        //public override Boolean IsSynchronized
        //{
        //    get
        //    {
        //        Set<T> set = mBasisSet as Set<T>;

        //        if (set != null)
        //        {
        //            return set.IsSynchronized;
        //        }
        //        else
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }
        //}

        //public override Object SyncRoot
        //{
        //    get
        //    {
        //        Set<T> set = mBasisSet as Set<T>;
        //        if (set != null)
        //        {
        //            return set.SyncRoot;
        //        }
        //        else
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }
        //}
    }
}