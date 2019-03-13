using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YGOProAnalyticsServer.Database.ManyToManySupport
{
    /// <summary>
    /// It provide many to many support. It just hide from the user join dbModel. You still need remember about join dbModel when you want inclue something. For more details see:
    /// <para>https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-3-hiding-as-icollection/</para>
    /// </summary>
    /// <typeparam name="TEntity">We want get this dbModel collection.</typeparam>
    /// <typeparam name="TOtherEntity">Current dbModel (this).</typeparam>
    /// <typeparam name="TJoinEntity">Join table between TEntity and TOtherEntity</typeparam>
    public class JoinCollectionFacade<TEntity, TOtherEntity, TJoinEntity>
    : ICollection<TEntity>
    where TJoinEntity : IJoinEntity<TEntity>, IJoinEntity<TOtherEntity>, new()
    {
        private readonly TOtherEntity _ownerEntity;
        private readonly ICollection<TJoinEntity> _collection;

        /// <summary>
        /// Create support for many to many relationship between TEntity and TOtherEntity.
        /// </summary>
        /// <param name="ownerEntity">Always should be equal 'this'.</param>
        /// <param name="collection">Join dbModel.</param>
        public JoinCollectionFacade(
            TOtherEntity ownerEntity,
            ICollection<TJoinEntity> collection)
        {
            _ownerEntity = ownerEntity;
            _collection = collection;
        }

        /// <summary>
        /// Get enumerator.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<TEntity> GetEnumerator()
            => _collection.Select(e => ((IJoinEntity<TEntity>)e).Navigation).GetEnumerator();

        /// <summary>
        /// Get enumerator.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        /// Add something to collection.
        /// </summary>
        /// <param name="item">Item which you want add.</param>
        public void Add(TEntity item)
        {
            var entity = new TJoinEntity();
            ((IJoinEntity<TEntity>)entity).Navigation = item;
            ((IJoinEntity<TOtherEntity>)entity).Navigation = _ownerEntity;
            _collection.Add(entity);
        }

        /// <summary>
        /// Clear collection.
        /// </summary>
        public void Clear()
            => _collection.Clear();

        /// <summary>
        /// Check if collection contains given item.
        /// </summary>
        /// <param name="item">Given item.</param>
        /// <returns>True/False</returns>
        public bool Contains(TEntity item)
            => _collection.Any(e => Equals(item, e));

        /// <summary>
        /// Copy collection to array.
        /// </summary>
        /// <param name="array">Collection of TEntity</param>
        /// <param name="arrayIndex">Array index.</param>
        public void CopyTo(TEntity[] array, int arrayIndex)
            => this.ToList().CopyTo(array, arrayIndex);

        /// <summary>
        /// Remove item from collection.
        /// </summary>
        /// <param name="item">ITem which you want remove. Remember, that item must be tracked by EF Core.</param>
        /// <returns>True if item was removed.</returns>
        public bool Remove(TEntity item)
            => _collection.Remove(
                _collection.FirstOrDefault(e => Equals(item, e)));

        /// <summary>
        /// Get information how many items are in collection.
        /// </summary>
        public int Count
            => _collection.Count;

        /// <summary>
        /// Check if collection is readonly.
        /// </summary>
        public bool IsReadOnly
            => _collection.IsReadOnly;

        /// <summary>
        /// Check if items are equals.
        /// </summary>
        /// <param name="item">Item which we want compare.</param>
        /// <param name="e">Join dbModel.</param>
        /// <returns>True if item is equal to this.</returns>
        private static bool Equals(TEntity item, TJoinEntity e)
            => Equals(((IJoinEntity<TEntity>)e).Navigation, item);
    }
}
