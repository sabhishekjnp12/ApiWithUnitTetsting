using System.Linq.Expressions;

namespace ApiWithUnitTetsting.EFCore;

/// <summary>
/// Defines a specification of a query with optional sorting and paging logic.
/// </summary>
/// <remarks>
/// The Query Specification pattern defines a query in an object. For example, in order to encapsulate a paged query that searches for some products you can create a PagedProduct specification that takes the necessary input parameters(pageNumber, pageSize, filter, etc.). Then, within any Repository method(usually a List() overload) it would accept an IQuerySpecification and run the expected query based on that specification.
/// </remarks>
/// <typeparam name="T">The type of the items to be queried.</typeparam>
public interface IQuerySpecification<T> where T : class
{
    #region Public Properties

    /// <summary>
    /// Gets the expression the queried items have to match in any case.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; set; }

    /// <summary>
    /// Gets the sort function that is currently applied.
    /// </summary>
    Func<IQueryable<T>, IOrderedQueryable<T>>? CurrentSort { get; }

    /// <summary>
    /// Gets or sets the custom (user-defined) filter expression. When <see cref="IsCustomFilterApplied"/> is <see langword="true"/> the items have to match in addition to <see cref="Criteria"/>.
    /// </summary>
    Expression<Func<T, bool>>? CustomFilter { get; set; }

    /// <summary>
    /// Gets the expression the queried items have to match as search result.
    /// </summary>
    Expression<Func<T, bool>>? CustomSearch { get; set; }

    /// <summary>
    /// Gets or sets the custom (user-defined) sort function.
    /// </summary>
    Func<IQueryable<T>, IOrderedQueryable<T>>? CustomSort { get; set; }

    /// <summary>
    /// Gets the default sort function that is applied, if the <see cref="CustomSort"/> is not active.
    /// </summary>
    Func<IQueryable<T>, IOrderedQueryable<T>>? DefaultSort { get; }

    /// <summary>
    /// Gets or sets the group function.
    /// </summary>
    Expression<Func<T, object>>? GroupExpression { get; set; }

    /// <summary>
    /// Gets the expression-based includes. Values can be added by the <see cref="AddIncludeExpression(Expression{Func{T, object}})"/> method.
    /// </summary>
    /// <remarks>
    /// Including children of children e.g. Basket.Items.Product is not possible with this method. Therefore we are using the <see cref="IncludeStrings"/> property and the <see cref="AddIncludeString(string)"/> method.
    /// </remarks>
    HashSet<Expression<Func<T, object>>>? IncludeExpressions { get; }

    /// <summary>
    /// Gets the string-based includes. Values can be added by the <see cref="AddIncludeString(string)"/> method.
    /// </summary>
    /// <remarks>
    /// String-based includes allow for including children of children  e.g. Basket.Items.Product.
    /// </remarks>
    HashSet<string>? IncludeStrings { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the filter is applied.
    /// </summary>
    bool IsCustomFilterApplied { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the custom (user-defined) sort is applied.
    /// </summary>
    bool IsCustomSortApplied { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grouping is applied.
    /// </summary>
    bool IsGroupApplied { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds an expression-based include to <see cref="IncludeExpressions"/>.
    /// </summary>
    /// <param name="includeExpression">The expression-based include to be added.</param>
    /// <remarks>
    /// Including children of children e.g. Basket.Items.Product is not possible with this method. Therefore we are using the <see cref="IncludeStrings"/> property and the <see cref="AddIncludeString(string)"/> method.
    /// </remarks>
    void AddIncludeExpression(Expression<Func<T, object>> includeExpression);

    /// <summary>
    /// Adds a string-based include to <see cref="IncludeStrings"/>.
    /// </summary>
    /// <param name="includeString">The string-based include to be added.</param>
    /// <remarks>
    /// String-based includes allow for including children of children e.g. Basket.Items.Product.
    /// </remarks>
    void AddIncludeString(string includeString);

    /// <summary>
    /// Gets an <see cref="IQueryable"/> based on a specified input-query and the instance of the <see cref="IQuerySpecification{T}"/>.
    /// </summary>
    /// <param name="inputQuery">The input query.</param>
    /// <param name="sorted">
    /// <para>Defines if the <see cref="CurrentSort"/> can or must be applied.</para>
    /// <para>If <see langword="null"/> the <see cref="CurrentSort"/> will be applied if set.</para>
    /// <para>If <see langword="true"/> the <see cref="CurrentSort"/> has to be applied if set. If <see cref="CurrentSort"/> is not defined, an <see cref="InvalidOperationException"/> wil be thrown.</para>
    /// <para>If <see langword="false"/> the <see cref="CurrentSort"/> is not needed and won't be applied.</para>
    /// </param>
    /// <param name="grouped">
    /// <para>Defines if the <see cref="GroupExpression"/> can or must be applied.</para>
    /// <para>If <see langword="null"/> the <see cref="GroupExpression"/> will be applied if set.</para>
    /// <para>If <see langword="true"/> the <see cref="GroupExpression"/> has to be applied if set. If <see cref="GroupExpression"/> is not defined, an <see cref="InvalidOperationException"/> wil be thrown.</para>
    /// <para>If <see langword="false"/> the <see cref="GroupExpression"/> is not needed and won't be applied.</para>
    /// </param>
    /// <param name="searchmatches">Defines if the <see cref="CustomSearch"/> is applied on the query.</param>
    /// <returns>The <see cref="IQueryable"/>.</returns>
    IQueryable<T> GetQuery(IQueryable<T> inputQuery, bool? sorted, bool? grouped, bool searchmatches);

    IQueryable<T> GetQuery(Microsoft.EntityFrameworkCore.ChangeTracking.LocalView<T> inputQuery, bool? sorted, bool? grouped, bool searchmatches);

    #endregion

    //bool IsPagingEnabled { get; }

    //void ApplyPaging(int skip, int take);
}