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
        /// Must be EXPLICITLY implemented. It should expose TEntity navigation property.
        /// </summary>
        /// <example> 
        ///     This sample shows how TEntity navigation properties should be exposed.
        ///     <code>
        ///         public class DbModelJoin : IJoinEntity&lt;FirstDbModel&gt;, IJoinEntity&lt;SecondDbModel&gt;
        ///         {
        ///             public int FirstDbModelId { get; set; }
        ///             public FirstDbModel FirstDbModel { get; set; }
        ///             
        ///             //It is important to implement it explicitly.
        ///             FirstDbModel IJoinEntity&lt;FirstDbModel&gt;.Navigation
        ///             {
        ///                 get => FirstDbModel;
        ///                 set => FirstDbModel = value;
        ///             }
        ///             
        ///             public int SecondDbModelId { get; set; }
        ///             public SecondDbModel SecondDbModel { get; set; }
        ///             
        ///             //It is important to implement it explicitly.
        ///             SecondDbModel IJoinEntity&lt;SecondDbModel&gt;.Navigation
        ///             {
        ///                 get => SecondDbModel;
        ///                 set => SecondDbModel = value;
        ///             }
        ///         }
        ///     </code>
        /// </example>
        TEntity Navigation { get; set; }
    }
}
