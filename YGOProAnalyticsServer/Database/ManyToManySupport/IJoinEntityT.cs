namespace YGOProAnalyticsServer.Database.ManyToManySupport
{
    /// <summary>
    /// Interfece required for provading support for many to many relationship between dbModels.
    /// <para>In join class both dbModels must EXPLICITLY implement it.</para>
    /// <para>https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-3-hiding-as-icollection/</para>
    /// </summary>
    /// <typeparam name="TEntity">DbModel.</typeparam>
    public interface IJoinEntity<TEntity>
    {
        /// <summary>
        /// Must be EXPLICITLY implemented. It should expose TEntity navigation property. For example:
        /// <para>get => tEntity;</para>
        /// <para>set => tEntity = value;</para>
        /// </summary>
        TEntity Navigation { get; set; }
    }
}
